using Crolow.FastDico.Common.Models.ScrabbleApi.Game;
using Crolow.FastDico.ScrabbleApi.Extensions;
using System.Text;

public class PrintGame
{
    public static void PrintGrid(CurrentGame game)
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
        foreach (var r in game.GameObjects.RoundsPlayed)
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
        for (int col = 1; col < game.GameObjects.Board.CurrentBoard[0].SizeH - 1; col++)
        {
            sb.AppendLine($"<td class='border'>{col}</td>");
        }
        sb.AppendLine("</tr>");

        for (int y = 1; y < game.GameObjects.Board.CurrentBoard[0].SizeH - 1; y++)
        {
            var cc = ((char)(y + 64));
            sb.AppendLine($"<tr><td class='border'>{cc}</td>");
            for (int x = 1; x < game.GameObjects.Board.CurrentBoard[0].SizeV - 1; x++)
            {
                var sq = game.GameObjects.Board.GetSquare(0, x, y);
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
        System.IO.File.WriteAllText($"C:\\dev\\Crolow.FastDico\\output-{game.GameObjects.Round}.html", sb.ToString());
#endif
        // We are done 
    }
}

