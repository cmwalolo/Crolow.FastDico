using Crolow.FastDico.Dawg;
using Crolow.FastDico.Dicos;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.Models.Models.ScrabbleApi.Entities;
using Crolow.FastDico.ScrabbleApi.Components;
using Crolow.FastDico.ScrabbleApi.Components.BoardSolver;
using Crolow.FastDico.ScrabbleApi.Components.Rounds;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.ScrabbleApi.GameObjects;
using Crolow.FastDico.ScrabbleApi.Utils;
using Crolow.FastDico.Search;
using Crolow.FastDico.Utils;
using System.Diagnostics;
using System.Text;
using System.Xml.Linq;

namespace Crolow.FastDico.ScrabbleApi;

public partial class ScrabbleAI
{
    private CurrentGame currentGame;

    public ScrabbleAI(string configsFile, string configName)
    {
        var playConfiguration = new ConfigReader().ReadConfiguration(configsFile, configName);
        this.currentGame = new CurrentGame();
        this.currentGame.Configuration = playConfiguration;

        currentGame.GameConfig = playConfiguration.SelectedConfig;
        GadDagCompiler gaddag = new GadDagCompiler();
        gaddag.ReadFromFile(playConfiguration.SelectedConfig.GaddagFile);

        currentGame.Board = new Board(this.currentGame);
        currentGame.LetterBag = new LetterBag(this.currentGame);
        currentGame.Rack = new PlayerRack();
        currentGame.Dico = gaddag;
        currentGame.GameConfig = playConfiguration.SelectedConfig;
        currentGame.PivotBuilder = new PivotBuilder(currentGame.Board, gaddag.Root, playConfiguration);
    }

    public void StartGame()
    {
        using (StopWatcher stopwatch = new StopWatcher("Game started"))
        {
            NextRound(true);
        }
    }
    private void NextRound(bool firstMove)
    {
        var boardSolver = new BoardSolver(currentGame);
        boardSolver.Initialize();

        XRoundValidator validator = new XRoundValidator(currentGame);
        validator.Initialize();

        PlayedRounds playedRounds = null;
        PlayerRack originalRack = new PlayerRack(currentGame.Rack);
        var letters = new List<Tile>();
        while (true)
        {
            letters = validator.InitializeLetters();
            // End Test
            if (letters == null)
            {
                EndGame();
                return;
            }

            var filters = validator.InitializeFilters();
            playedRounds = boardSolver.Solve(letters, filters);

            if (playedRounds.Tops.Any())
            {
                var round = validator.ValidateRound(playedRounds, letters, originalRack);


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

        }

        PlayedRound selectedRound = validator.FinalizeRound(playedRounds);
        if (selectedRound == null)
        {
            EndGame();
            return;
        }

        currentGame.Board.SetRound(selectedRound);
        currentGame.RoundsPlayed.Add(selectedRound);
        selectedRound = new PlayedRound();
        currentGame.Round++;
        currentGame.LetterBag.DebugBag();
        NextRound(false);
    }


    private void EndGame()
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
        sb.AppendLine("<th><td>#</td><td>Rack</td><td>Word</td><td>pos</td><td>pts</td></th>");
        int ndx = 1;
        foreach (var r in currentGame.RoundsPlayed)
        {
            sb.AppendLine("<tr>");
            sb.AppendLine($"<td>{ndx++}</td>");
            sb.AppendLine($"<td>{r.Rack.ToString()}</td>");
            sb.AppendLine($"<td>{r.GetWord(true)}</td>");
            sb.AppendLine($"<td>{r.GetPosition()}</td>");
            sb.AppendLine($"<td>{r.Points}</td>");
            sb.AppendLine("</tr>");
        }

        sb.AppendLine("</table>");

        sb.AppendLine("<table>");
        sb.AppendLine("<tr><td></td>");
        for (int col = 1; col < currentGame.Board.CurrentBoard[0].SizeH - 1; col++)
        {
            sb.AppendLine($"<td class='border'>{col}</td>");
        }
        sb.AppendLine("</tr>");

        for (int x = 1; x < currentGame.Board.CurrentBoard[0].SizeH - 1; x++)
        {
            var cc = ((char)(x + 64));
            sb.AppendLine($"<tr><td class='border'>{cc}</td>");
            for (int y = 1; y < currentGame.Board.CurrentBoard[0].SizeH - 1; y++)
            {
                var sq = currentGame.Board.GetSquare(0, y, x);
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