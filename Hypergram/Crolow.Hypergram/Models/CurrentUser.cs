using Kalow.Hypergram.Logic.Models.GameSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiBlazorWeb.Shared.Models
{
    public class CurrentUser
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public HypergramUser User { get; set; }
    }
}
