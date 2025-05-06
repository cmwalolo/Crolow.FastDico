using Crolow.FastDico.Models.Common.Entities;
using Crolow.FastDico.Models.ScrabbleApi;
using Crolow.FastDico.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Models.Common
{
    public class ApplicationContext
    {
        public static LetterConfigModel DefaultLetterConfig { get; set; }
        public static GameConfigurationContainer CurrentConfiguration { get; set; }
        public static User User { get; set; }
    }
}
