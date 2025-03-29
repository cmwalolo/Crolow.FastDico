#undef DEBUGPIVOT  

using Crolow.FastDico.Dicos;
using Crolow.FastDico.GadDag;
using Crolow.FastDico.ScrabbleApi.Config;
using Crolow.FastDico.Utils;
using System.Text;


namespace Crolow.FastDico.ScrabbleApi.GameObjects
{
    public class PivotBuilder
    {
        private static Dictionary<string, uint> PivotCache = new Dictionary<string, uint>();
        private Board board;
        private GadDagSearch searcher;
        private LetterNode letterNode;
        private PlayConfiguration playConfiguration;

        public PivotBuilder(Board board, LetterNode rootNode, PlayConfiguration playConfiguration)
        {
            this.board = board;
            this.searcher = new GadDagSearch(rootNode);
            this.letterNode = rootNode;
            this.playConfiguration = playConfiguration;
        }

        public uint[] GetMask(int x, int y, int direction)
        {
            return board.GetSquare(direction, x, y).Pivots;
        }

        public void Build()
        {
            Build(0, 1);
            Build(1, 0);
        }

        public void Build(int grid, int targetGrid)
        {
            for (int y = 1; y < board.CurrentBoard[0].SizeV - 1; y++)
            {
                var squares = new Square[board.CurrentBoard[0].SizeH];

                for (int x = 0; x < board.CurrentBoard[0].SizeH; x++)
                {
                    squares[x] = board.GetSquare(grid, x, y);
                    squares[x].ResetPivot(targetGrid, 0);
                }
                var runs = MaskLeftToRightHorizontal(grid, targetGrid, squares, y);
            }

        }

        private struct Run(bool toRight, int start, int end)
        {
            public bool toRight = toRight;
            public int start = start, end = end;
        }

        private List<Run> MaskLeftToRightHorizontal(int grid, int targetGrid, Square[] squares, int y)
        {
            int startX = 1;
            int endX = 0;

            var runs = new List<Run>();

            if (squares.Count(p => p.CurrentLetter != null) == 0)
            {
                return null;
            }

            // We first calculate runs of adjacent letters
            do
            {
                while (!squares[startX].IsBorder && squares[startX].CurrentLetter == null)
                {
                    startX++;
                }

                endX = startX;
                if (!squares[startX].IsBorder)
                {
                    var run = new Run(true, startX, endX);

                    // We are looking for adjactent letters for the pivots to the left
                    while (!squares[endX].IsBorder && squares[endX + 1].CurrentLetter != null)
                    {
                        endX++;
                    }

                    run.end = endX;
                    if (startX > 1)
                    {
                        run.start = startX - 1;
                        runs.Add(run);
                    }

                    // Ok we try now to extend to the right.. But it's possible
                    // That we add a letter in middle of two runs. so we need to 
                    // end the run 
                    if (!squares[endX].IsBorder)
                    {
                        while (!squares[endX].IsBorder && squares[endX + 1].CurrentLetter != null)
                        {
                            endX++;
                        }
                    }

                    if (endX < squares.Length - 1)
                    {
                        run = new Run(false, startX, endX + 1);
                        runs.Add(run);
                    }
                }

                startX = endX + 1;
            } while (!squares[endX].IsBorder);

            runs.ForEach(run =>
            {
                Solve(grid, targetGrid, run, squares);
            });

            return runs;

        }

        private void Solve(int grid, int targetGrid, Run run, Square[] squares)
        {
            var start = run.start;
            var end = run.end;

            var bytes = new byte[end - start + 1];
            uint key = 0;
            var points = 0;
            var pivot = uint.MaxValue;
            var pivotPosition = 0;
            Square pivotSquare = null;

            for (int i = start; i <= end; i++)
            {
                var sq = squares[i];
                if (sq.CurrentLetter == null)
                {
                    bytes[i - start] = DawgUtils.JokerByte;
                    pivotSquare = squares[i];
                    pivotPosition = i - start;
                }
                else
                {
                    bytes[i - start] = sq.CurrentLetter.Letter;
                    points += sq.CurrentLetter.Points;
                }
            }
            var results = SearchByPattern(bytes);

            pivotSquare.ResetPivot(targetGrid, 0, 0);
            if (results.Any())
            {
                foreach (var result in results)
                {
                    pivotSquare.SetPivot(result[pivotPosition], targetGrid, points);
                }
            }

#if DEBUGPIVOT
            var pattern = DawgUtils.ConvertBytesToWord(bytes.ToList());
            var mask = pivotSquare.Pivots[targetGrid];
            var pts = pivotSquare.PivotPoints[targetGrid];
            Console.WriteLine($"raccord : {pattern} {mask} {pts}");
#endif

        }

        public List<byte[]> SearchByPattern(byte[] bytePattern)
        {
            // Convert the pattern into bytes
            List<byte[]> results = new List<byte[]>();
            SearchByPatternRecursive(letterNode, bytePattern, 0, new byte[bytePattern.Length], results);
            return results;
        }

        private void SearchByPatternRecursive(LetterNode currentNode, byte[] bytePattern, int patternIndex, byte[] currentWord, List<byte[]> results)
        {
            // Base case: Reached the end of the pattern
            if (patternIndex == bytePattern.Length)
            {
                if (currentNode.IsEnd)
                {
                    results.Add(currentWord.ToArray());
                }
                return;
            }

            byte currentByte = bytePattern[patternIndex];

            if (currentByte == DawgUtils.JokerByte) // '?' wildcard
            {
                // Match exactly one character
                foreach (var child in currentNode.Children)
                {
                    if (child.Letter != DawgUtils.PivotByte)
                    {
                        currentWord[patternIndex] = child.Letter;
                        SearchByPatternRecursive(child, bytePattern, patternIndex + 1, currentWord, results);
                        currentWord[patternIndex] = DawgUtils.JokerByte;
                    }
                }
            }
            else
            {
                // Match the exact character
                var nextNode = currentNode.Children.Where(p => p.Letter != DawgUtils.PivotByte && p.Letter == currentByte);
                if (nextNode.Any() && currentByte != DawgUtils.PivotByte)
                {
                    currentWord[patternIndex] = currentByte;
                    SearchByPatternRecursive(nextNode.First(), bytePattern, patternIndex + 1, currentWord, results);
                    currentWord[patternIndex] = DawgUtils.JokerByte; // Backtrack
                }
            }
        }
    }
}
