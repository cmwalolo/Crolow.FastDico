using Crolow.FastDico.Common.Models.ScrabbleApi;
using Crolow.TopMachine.Data.Bridge.Entities;
using Crolow.TopMachine.Data.Bridge.Entities.ScrabbleApi;

namespace Crolow.FastDico.Common.Models.Common
{
    public class ApplicationContext
    {
        public static ILetterConfigModel DefaultLetterConfig { get; set; }
        public static ToppingConfigurationContainer CurrentConfiguration { get; set; }
        public static IUser User { get; set; }
    }
}
