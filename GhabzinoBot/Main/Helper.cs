using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using Persia;

namespace Ayantech.WebService
{
    public static partial class Helper
    {
        public static T JsonDeserializer<T>(string jsonString)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var instance = JsonConvert.DeserializeObject<T>(jsonString);

                //Log.Trace("desalinizing json string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return default(T);
            }
        }
        public static string JsonSerializer<T>(T jsonObject)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var jsonString = JsonConvert.SerializeObject(jsonObject);

                //Log.Trace("serializing json object successfully completed.", sw.Elapsed.TotalMilliseconds);
                return jsonString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string ToPersianDateTime(DateTime dateTime)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var solarDate = Calendar.ConvertToPersian(dateTime);
                var persianDateTime = solarDate.ToString();

                var time = dateTime.ToString("HH:mm");
                if (!string.Equals(time, "00:00"))
                    persianDateTime = persianDateTime + " " + time;

                //Log.Trace("converting to persian date time successfully completed.", sw.Elapsed.TotalMilliseconds);
                return persianDateTime;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string ToMD5Hash(this string plainTextValue)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var hashValue = string.Empty;
                var data = Encoding.ASCII.GetBytes(plainTextValue);
                using (var md5 = MD5.Create())
                {
                    var result = md5.ComputeHash(data);
                    hashValue = BitConverter.ToString(result).Replace("-", string.Empty);
                }

                //Log.Trace("converting to sha256 hash value completed successfully.", sw.Elapsed.TotalMilliseconds);
                return hashValue;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static DateTime ToGregorianDateTime(string date)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var dateParts = date.Split('/');
                var gregorianDateTime = Calendar.ConvertToGregorian(Convert.ToInt32(dateParts[0]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[2]), DateType.Persian);

                //Log.Trace("converting to gregorian date time successfully completed.", sw.Elapsed.TotalMilliseconds);
                return gregorianDateTime;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return DateTime.MaxValue;
            }
        }
        public static string ToTomanCurrency(this long IrrCurrency)
        {
            var tomanString = string.Empty;

            if (IrrCurrency >= 10)
            {
                tomanString = string.Format("{0:n0}", IrrCurrency / 10);
            }

            return tomanString;
        }
    }
}