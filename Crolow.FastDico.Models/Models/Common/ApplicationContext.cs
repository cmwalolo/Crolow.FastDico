using Crolow.FastDico.Common.Models.Common.Entities;
using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.FastDico.Common.Models.ScrabbleApi.Entities;

namespace Crolow.FastDico.Common.Models.Common
{
    public class ApplicationContext
    {
        public static LetterConfigModel DefaultLetterConfig { get; set; }
        public static ToppingConfigurationContainer CurrentConfiguration { get; set; }
        public static User User { get; set; }
    }
}
