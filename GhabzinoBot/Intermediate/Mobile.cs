using Ayantech.WebService;
using PhoneNumbers;
using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace GhabzinoBot
{
    public class Mobile
    {
        private string _internationalNumber;
        public string Number { set; get; }
        public bool NumberContentIsValid { set; get; }
        public string InternationalNumber { set { _internationalNumber = value; } get { return _internationalNumber ?? (_internationalNumber = InitiateInternationalNumber()); } }


        public bool IsNumberContentValid()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var len = new Regex(@"^.{3,18}$");
                var digit = new Regex(@"^[0-9]*$");
                Number = Number.StartsWith(@"+") ? Number.Substring(1) : Number;
                Number = Number.Replace(" ", string.Empty);
                if (!len.IsMatch(Number))
                {
                    NumberContentIsValid = false;
                }
                else if (!digit.IsMatch(Number))
                {
                    NumberContentIsValid = false;
                }
                else
                {
                    var phoneUtil = PhoneNumberUtil.GetInstance();
                    var mobile = phoneUtil.Parse(Number, "IR");
                    if (phoneUtil.IsValidNumber(mobile) != true)
                    {
                        NumberContentIsValid = false;
                    }
                    else if (phoneUtil.GetNumberType(mobile) != PhoneNumberType.MOBILE)
                    {
                        NumberContentIsValid = false;
                    }
                    else
                        NumberContentIsValid = true;
                }

                Log.Trace("mobile content validation completed successfully.", sw.Elapsed.TotalMilliseconds);
                return true;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return false;
            }
        }
        private string InitiateInternationalNumber()
        {
            var sw = Stopwatch.StartNew();

            try
            {
                var phoneUtil = PhoneNumberUtil.GetInstance();
                var phone = phoneUtil.Parse(Number, "IR");
                var internationalNumber = phoneUtil.Format(phone, PhoneNumberFormat.INTERNATIONAL);

                Log.Trace("calculating international number completed successfully.", sw.Elapsed.TotalMilliseconds);
                return internationalNumber;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex.ToString(), sw.Elapsed.TotalMilliseconds);
                return null;
            }
        }
    }
}