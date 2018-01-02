using System;
using System.Diagnostics;
using System.Web.Http;
using Ayantech.WebService;

namespace GhabzinoBot
{
    public class CoreController : ApiController
    {
        [HttpPost]
        public Input Test(Input input)
        {
            var sw = Stopwatch.StartNew();
            try
            {
                Log.Test(Helper.JsonSerializer(input));

                return new Input { FirstName = "Mohammad", LastName = "Montazeri", ID = "" };
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds, "Public");
                return null;
            }
        }
        public class Input
        {
            public string FirstName { set; get; }
            public string LastName { set; get; }
            public string FullName { set; get; }
            public string ID { set; get; }
        }
    }
}
