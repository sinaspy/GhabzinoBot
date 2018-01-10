using Ayantech.WebService;
using System;
using System.Diagnostics;
using System.Web.Http;

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
                var updateHandler = new UpdateHandler
                {
                    Update = update
                };
                updateHandler.Bot_OnUpdate();
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