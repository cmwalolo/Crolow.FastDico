using Crolow.FastDico.Common.Interfaces.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Extensions;
using Crolow.FastDico.ScrabbleApi.Utils;
using System.Text;

namespace Crolow.FastDico.ScrabbleApi;

public partial class ScrabbleAI : IScrabbleAI
{
    private CurrentGame CurrentGame;

    public ScrabbleAI(CurrentGame currentGame)
    {
        this.CurrentGame = currentGame;
    }

    public void StartGame()
    {
        using (StopWatcher stopwatch = new StopWatcher("Game started"))
        {
            NextRound(true);
        }
    }
    public void NextRound(bool firstMove)
    {
        CurrentGame.BoardSolver.Initialize();
        CurrentGame.Validator.Initialize();

        PlayedRounds playedRounds = null;
        var letters = new List<Tile>();

        // We create a copy of the rack and the back to
        // Start freshly each iteration
        var originalRack = new PlayerRack(CurrentGame.Rack);
        var originalBag = new LetterBag(CurrentGame.LetterBag);

        while (true)
        {
            CurrentGame.Validator.InitializeRound();
            letters = CurrentGame.Validator.InitializeLetters();
            // End Test
            if (letters == null)
            {
                EndGame();
                return;
            }

            var filters = CurrentGame.Validator.InitializeFilters();
            playedRounds = CurrentGame.BoardSolver.Solve(letters, filters);

            if (playedRounds.Tops.Any())
            {
                var round = CurrentGame.Validator.ValidateRound(playedRounds, letters, CurrentGame.BoardSolver);
                if (round != null)
                {
                    playedRounds = round;
                    break;
                }
            }
            else
            {
                EndGame();
                return;
            }

            CurrentGame.Rack = originalRack;
            CurrentGame.LetterBag = originalBag;
        }

        PlayableSolution selectedRound = CurrentGame.Validator.FinalizeRound(playedRounds);
        if (selectedRound == null)
        {
            EndGame();
            return;
        }

        CurrentGame.Board.SetRound(selectedRound);
        CurrentGame.RoundsPlayed.Add(selectedRound);
        selectedRound = new PlayableSolution();
        CurrentGame.Round++;
#if DEBUG
        CurrentGame.LetterBag.DebugBag(CurrentGame.Rack);
#endif
        NextRound(false);
    }

    public void EndGame()
    {
#if DEBUG
        PrintGrid();
#endif
    }

    public void PrintGrid()
    {
#if DEBUG
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("<link rel=\"stylesheet\" href=\"grid.css\" />");
        sb.AppendLine("</head>");

        sb.AppendLine("<body><div id='grid'>");

        sb.AppendLine("<table class='results' style='float:right'>");
        sb.AppendLine("<tr><th>#</th><th>Rack</th><th>Word</th><th>pos</th><th>pts</th></tr>");
        int ndx = 1;
        foreach (var r in CurrentGame.RoundsPlayed)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{ndx++}</td>");
            sb.AppendLine($"<td>{r.Rack.GetString()}</td>");
            sb.AppendLine($"<td>{r.GetWord(true)}</td>");
            sb.AppendLine($"<td>{r.GetPosition()}</td>");
            sb.AppendLine($"<td>{r.Points}</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table>");

        sb.AppendLine("<table class='board'>");
        sb.AppendLine("<tr><td></td>");
        for (int col = 1; col < CurrentGame.Board.CurrentBoard[0].SizeH - 1; col++)
        {
            sb.AppendLine($"<td class='border'>{col}</td>");
        }
        sb.AppendLine("</tr>");

        for (int x = 1; x < CurrentGame.Board.CurrentBoard[0].SizeH - 1; x++)
        {
            var cc = ((char)(x + 64));
            sb.AppendLine($"<tr><td class='border'>{cc}</td>");
            for (int y = 1; y < CurrentGame.Board.CurrentBoard[0].SizeH - 1; y++)
            {
                var sq = CurrentGame.Board.GetSquare(0, y, x);
                var cclass = $"cell cell-{sq.LetterMultiplier} cell{sq.WordMultiplier}";

                if (sq.Status == 1)
                {
                    cclass += sq.CurrentLetter.IsJoker ? " tileJoker" : " tile";
                }

                sb.AppendLine($"<td class='{cclass}'>");
                if (sq.Status == 1)
                {
                    var c = (char)(sq.CurrentLetter.Letter + 97);
                    sb.AppendLine(char.ToUpper(c).ToString());
                }
                sb.AppendLine("</td>");
            }
            sb.AppendLine("</tr>");
        }
        sb.AppendLine("</table>");

        sb.AppendLine("</grid></body></html>");
        System.IO.File.WriteAllText("output.html", sb.ToString());
#endif
        // We are done 
    }


}