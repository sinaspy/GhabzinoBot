using Newtonsoft.Json;
using Persia;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml.Serialization;

namespace Ayantech.WebService
{
    public static class Helper
    {
        public static string Decrypt(string cipherText)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var decryptedString = string.Empty;
                var vectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");
                var cipherTextBytes = Convert.FromBase64String(cipherText);
                using (var password = new PasswordDeriveBytes(ProjectValues.CryptographyKey, null))
                {
                    var keyBytes = password.GetBytes(32);
                    using (var symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, vectorBytes))
                        {
                            using (var memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    var plainTextBytes = new byte[cipherTextBytes.Length];
                                    var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    decryptedString = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }

                //Log.Trace("decrypting successfully completed.", sw.Elapsed.TotalMilliseconds);
                return decryptedString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string GetCalledUrl()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var calledUrl = $"{HttpContext.Current.Request.HttpMethod} {HttpContext.Current.Request.Url.OriginalString}";

                //Log.Trace("get consumer called Url successfully completed.", sw.Elapsed.TotalMilliseconds);
                return calledUrl;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string GetIpAddress()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var ipAddress = HttpContext.Current.Request.Headers["REMOTE_ADDR"] ?? HttpContext.Current.Request.UserHostAddress;

                //Log.Trace("get consumer IP address successfully completed.", sw.Elapsed.TotalMilliseconds);
                return ipAddress;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string GetUserAgent()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var userAgent = HttpContext.Current.Request.UserAgent;

                //Log.Trace("get consumer user agent successfully completed.", sw.Elapsed.TotalMilliseconds);
                return userAgent;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string GetUserName(string identity)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                //userName = string.IsNullOrEmpty(identity) ? ProjectValues.Consumer : JsonWebToken.Verification(identity, ProjectValues.SecretKey, true) ? JsonWebToken.Decode(identity, ProjectValues.SecretKey).Payload?.UserName : identity;
                var userName = "Unknown";

                //Log.Trace("get consumer user name successfully completed.", sw.Elapsed.TotalMilliseconds);
                return userName;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static DataSet NullCheck(this DataSet dataSet)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                foreach (DataTable dataTable in dataSet.Tables)
                    foreach (DataRow dataRow in dataTable.Rows)
                        foreach (DataColumn dataColumn in dataTable.Columns)
                            if (dataRow.IsNull(dataColumn))
                            {
                                if (dataColumn.DataType.IsValueType) dataRow[dataColumn] = Activator.CreateInstance(dataColumn.DataType);
                                else if (dataColumn.DataType == typeof(bool)) dataRow[dataColumn] = false;
                                else if (dataColumn.DataType == typeof(Guid)) dataRow[dataColumn] = Guid.Empty;
                                else if (dataColumn.DataType == typeof(string)) dataRow[dataColumn] = string.Empty;
                                else if (dataColumn.DataType == typeof(DateTime)) dataRow[dataColumn] = DateTime.MaxValue;
                                else if (dataColumn.DataType == typeof(int) || dataColumn.DataType == typeof(byte) || dataColumn.DataType == typeof(short) || dataColumn.DataType == typeof(long) || dataColumn.DataType == typeof(float) || dataColumn.DataType == typeof(double)) dataRow[dataColumn] = 0;
                                else dataRow[dataColumn] = null;
                            }

                //Log.Trace("converting the data base null values successfully completed.", sw.Elapsed.TotalMilliseconds);
                return dataSet;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return dataSet;
            }
        }
        public static T[] RemoveAt<T>(this T[] source, int index)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var instance = new T[source.Length - 1];
                if (index > 0)
                    Array.Copy(source, 0, instance, 0, index);

                if (index < source.Length - 1)
                    Array.Copy(source, index + 1, instance, index, source.Length - index - 1);

                //Log.Trace("removing from array member successfully completed.", sw.Elapsed.TotalMilliseconds);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return default(T[]);
            }
        }
        public static string ToEnglishNumber(this string number)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var arabicDigits = new string[10] { "٠", "١", "٢", "٣", "٤", "٥", "٦", "٧", "٨", "٩" };
                var persianDigits = new string[10] { "۰", "۱", "۲", "۳", "۴", "۵", "۶", "۷", "۸", "۹" };
                var englishDigits = new string[10] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
                if (!string.IsNullOrEmpty(number))
                    for (int i = 0; i < persianDigits.Length; i++)
                        number = number.Replace(persianDigits[i], englishDigits[i]).Replace(arabicDigits[i], englishDigits[i]);

                var englishNumber = number;
                //Log.Trace("converting to english number successfully completed.", sw.Elapsed.TotalMilliseconds);
                return englishNumber;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static T ToEnum<T>(this string stringValue) where T : struct
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var enumValue = (T)Enum.Parse(typeof(T), stringValue, true);

                //Log.Trace("converting string to enum successfully completed.", sw.Elapsed.TotalMilliseconds);
                return enumValue;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return default(T);
            }
        }
        public static string ToPersianNumber<T>(this T number)
        {
            var sw = Stopwatch.StartNew();

            var numberString = number.ToString();

            try
            {
                var persianNumber = string.Empty;
                if (!string.IsNullOrEmpty(numberString))
                    persianNumber = PersianWord.ToPersianString(numberString);

                //Log.Trace("converting to persian number successfully completed.", sw.Elapsed.TotalMilliseconds);
                return persianNumber;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string ToSafePersianString(this string persianString)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var safePersianString = string.Empty;
                if (!string.IsNullOrEmpty(persianString))
                    safePersianString = persianString.Replace("ي", "ی").Replace("ئ", "ی").Replace("ك", "ک");

                //Log.Trace("converting to safe persian string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return safePersianString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
        public static string ToSha256Hash(this string plainTextValue)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var hashValue = string.Empty;
                var data = Encoding.UTF8.GetBytes(plainTextValue);
                using (var shaM = new SHA256Managed())
                {
                    var result = shaM.ComputeHash(data);
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
        public static T XmlDeserializer<T>(string xmlString)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var instance = default(T);
                var xmlSerializer = new XmlSerializer(typeof(T));
                using (var stringReader = new StringReader(xmlString))
                    instance = (T)xmlSerializer.Deserialize(stringReader);

                //Log.Trace("desalinizing xml string successfully completed.", sw.Elapsed.TotalMilliseconds);
                return instance;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return default(T);
            }
        }
        public static string XmlSerializer<T>(T xmlObject)
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var xmlString = string.Empty;
                var xmlSerializer = new XmlSerializer(xmlObject.GetType());
                using (var textWriter = new StringWriter())
                {
                    xmlSerializer.Serialize(textWriter, xmlObject);
                    xmlString = textWriter.ToString();
                }

                //Log.Trace("serializing xml object successfully completed.", sw.Elapsed.TotalMilliseconds);
                return xmlString;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }


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
        //private static KeyboardButton[][] CreateKeyboard(KeyboardType keyboardType)
        //{
        //    KeyboardButton[][] keyboard = null;

        //    switch (keyboardType)
        //    {
        //        case KeyboardType.None:
        //            //ERROR
        //            break;
        //        case KeyboardType.MainMenu:
        //            keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnWaterBillInquiry, btnGasBillInquiry, btnElectricityBillInquiry }, new KeyboardButton[] { btnTrafficFinesInquiry, btnMciMobileBillInquiry, btnFixedLineBillInquiry }, new KeyboardButton[] { btnBills, btnHistory, btnRecipt }, };
        //            break;
        //        case KeyboardType.ReturnToMainMenu:
        //            keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnReturnToMainMenu, }, };
        //            break;
        //        case KeyboardType.GoToPaymentPageSingle:
        //            keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToPaymentPage }, new KeyboardButton[] { btnReturnToMainMenu } };
        //            break;
        //        case KeyboardType.GoToPaymentPageForPhones:
        //            keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToPaymentPage }, new KeyboardButton[] { btnReturnToMainMenu } };
        //            break;
        //        case KeyboardType.AddPayment:
        //            keyboard = new KeyboardButton[][] { new KeyboardButton[] { btnGoToOnlinePayment, }, new KeyboardButton[] { btnInlineReturnToMainMenu, }, };
        //            break;

        //        default:
        //            //ERROR
        //            break;
        //    }

        //    return keyboard;
        //}
        //private static InlineKeyboardButton[][] CreateInlineKeyboard(KeyboardType keyboardType, params string[] optionalUrls)
        //{
        //    InlineKeyboardButton[][] keyboard = null;

        //    switch (keyboardType)
        //    {
        //        case KeyboardType.None:
        //            //ERROR
        //            break;
        //        case KeyboardType.MainMenu:
        //            keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineWaterBillInquiry, btnInlineGasBillInquiry, btnInlineElectricityBillInquiry, }, new InlineKeyboardButton[] { btnInlineTrafficFinesInquiry, btnInlineMciMobileBillInquiry, btnInlineFixedLineBillInquiry, }, new InlineKeyboardButton[] { btnInlineBills, btnInlineHistory, btnInlineRecipt, }, };
        //            break;
        //        case KeyboardType.ReturnToMainMenu:
        //            keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineReturnToMainMenu, }, };
        //            break;
        //        case KeyboardType.GoToPaymentPageSingle:
        //            btnInlineGoToPaymentPage.Url = optionalUrls?[0];
        //            keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineGoToPaymentPage, }, };
        //            break;
        //        case KeyboardType.GoToPaymentPageForPhones:
        //            btnInlineGoToPaymentPageFullTerm.Url = optionalUrls?[0];
        //            btnInlineGoToPaymentPageMidTerm.Url = optionalUrls?[1];
        //            keyboard = new InlineKeyboardButton[][] { new InlineKeyboardButton[] { btnInlineGoToPaymentPageFullTerm, }, new InlineKeyboardButton[] { btnInlineGoToPaymentPageMidTerm, }, };
        //            break;

        //        default:
        //            //ERROR
        //            break;
        //    }

        //    return keyboard;
        //}

    }
}