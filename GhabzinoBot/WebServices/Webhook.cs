using System;
using System.Diagnostics;
using System.Web.Http;
using Ayantech.WebService;

namespace GhabzinoBot
{
    public class WebhookController : ApiController
    {
        [HttpPost]
        public void GetUpdate(Telegram.Bot.Types.Update update)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                Log.Info(Helper.JsonSerializer(update), sw.Elapsed.TotalMilliseconds, update.Id.ToString());
                UpdateHandler updateHandler = new UpdateHandler();
                updateHandler.Bot_OnUpdate(update);
                return;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds, update.Id.ToString());
                return;
            }
        }
    }
}