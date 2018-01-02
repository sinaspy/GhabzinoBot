using System.Diagnostics;
using System.Web.Configuration;
using Telegram.Bot;

namespace Ayantech.WebService
{
    public static partial class ProjectValues
    {
        public static string ProjectName { private set; get; } = "GhabzinoBot";

        public static string PublicToken { private set; get; } = WebConfigurationManager.AppSettings["PublicToken"];
        public static TelegramBotClient Bot { private set; get; }
        public static string BotToken { private set; get; } = WebConfigurationManager.AppSettings["BotToken"];
        public static string EndpointConfigurationName { private set; get; } = WebConfigurationManager.AppSettings["EndpointConfigurationName"];
        public static string ApplicationType { private set; get; } = "TelegramBot";
        public static string ApplicationVersion { private set; get; } = "1.0.0";

        public static bool InitOtherObjects()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                Bot = new TelegramBotClient(BotToken);
            }
            catch (System.Exception e)
            {
                Log.Fatal(e.ToString(), sw.Elapsed.TotalMilliseconds);
                return false;
            }

            return true;
        }
    }
}