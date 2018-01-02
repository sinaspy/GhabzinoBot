using Ayantech.WebService;
using GhabzinoBot.GhabzinoService;
using Newtonsoft.Json;

namespace GhabzinoBot
{
    public static class GhabzinoCoreApi
    {
        public static WaterBillInquiryOutput WaterHandler(string token, string waterBillId)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.WaterBillInquiry(
                new WaterBillInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new WaterBillInquiryInputParams() { WaterBillID = waterBillId }
                });
            client.Close();

            return webserviceResult;
        }
        public static GasBillInquiryOutput GasHandler(string token, string participateCode)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.GasBillInquiry(
                new GasBillInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new GasBillInquiryInputParams() { ParticipateCode = participateCode }
                });
            client.Close();

            return webserviceResult;
        }
        public static ElectricityBillInquiryOutput ElectricityHandler(string token, string electricityBillId)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.ElectricityBillInquiry(
                new ElectricityBillInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new ElectricityBillInquiryInputParams() { ElectricityBillID = electricityBillId }
                });
            client.Close();

            return webserviceResult;
        }
        public static MCIMobileBillInquiryOutput MciMobileHandler(string token, string mobileNumber)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.MCIMobileBillInquiry(
                new MCIMobileBillInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new MCIMobileBillInquiryInputParams() { MobileNumber = mobileNumber }
                });
            client.Close();

            return webserviceResult;
        }
        public static FixedLineBillInquiryOutput FixedLineHandler(string token, string fixedNumber)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.FixedLineBillInquiry(
                new FixedLineBillInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new FixedLineBillInquiryInputParams() { FixedLineNumber = fixedNumber }
                });
            client.Close();

            return webserviceResult;
        }
        public static TrafficFinesInquiryOutput TrafficFinesHandler(string token, string barcode)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.TrafficFinesInquiry(
                new TrafficFinesInquiryInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = new TrafficFinesInquiryInputParams() { BarCode = barcode }
                });
            client.Close();

            return webserviceResult;
        }
        public static RequestActivationCodeOutput RequestActivationCode(string mobileNumber)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.RequestActivationCode(
                new RequestActivationCodeInput()
                {
                    Parameters = new RequestActivationCodeInputParams() { ApplicationType = ProjectValues.ApplicationType, ApplicationVersion = ProjectValues.ApplicationVersion, MobileNumber = mobileNumber }
                });
            client.Close();

            return webserviceResult;
        }
        public static LoginOutput Login(string mobileNumber, string acticationCode)
        {
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.Login(
                new LoginInput()
                {
                    Parameters = new LoginInputParams() { ApplicationType = ProjectValues.ApplicationType, ApplicationVersion = ProjectValues.ApplicationVersion, MobileNumber = mobileNumber, ActivationCode = acticationCode }
                });
            client.Close();

            return webserviceResult;
        }
        public static ReportNewPaymentOutput ReportNewPayment(string token,/*int parametersLength,*/ params ReportNewPaymentInputParams[] reportNewPaymentInputParams)
        {
            DataHandler.Log(JsonConvert.SerializeObject(reportNewPaymentInputParams));//???
            CoreClient client = new CoreClient(ProjectValues.EndpointConfigurationName);

            var webserviceResult = client.ReportNewPayment(
                new ReportNewPaymentInput()
                {
                    Identity = new WebServiceIdentity() { Token = token },
                    Parameters = reportNewPaymentInputParams,
                });
            client.Close();

            return webserviceResult;
        }
    }
}
