namespace Crolow.FastDico.ScrabbleApi.GameObjects
{
    public class SolverFilters
    {
        public bool PickallResults { get; set; }
        public List<int>[] Filters { get; set; } = new List<int>[2];

        public SolverFilters()
        {
            Filters[0] = new List<int>();
            Filters[1] = new List<int>();
        }
    }
}
