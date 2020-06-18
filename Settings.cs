using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netcore3_api_basicproject
{
    public static class Settings
    {
        public static string Secret = "fedaf7d8863b48e197b9287d492b708e";
    }


    public class AppSettings
    {
        public int ExpirationTime { get; set; }
        public string ValidIssuer { get; set; }
        public string ValidAudience { get; set; }
    }
}
