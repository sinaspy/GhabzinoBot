using Ayantech.WebService;

namespace GhabzinoBot
{
    public class UserInfo
    {
        public long UserId { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; } = ProjectValues.PublicToken;
        public UserState UserState { get; set; }
        public UserField UserField { get; set; }
        public WaterBillInquiryStep WaterBillInquiryStep { get; set; }
        public GasBillInquiryStep GasBillInquiryStep { get; set; }
        public ElectricityBillInquiryStep ElectricityBillInquiryStep { get; set; }
        public MciMobileBillInquiryStep MciMobileBillInquiryStep { get; set; }
        public FixedLineBillInquiryStep FixedLineBillInquiryStep { get; set; }
        public TrafficFinesInquiryStep TrafficFinesInquiryStep { get; set; }
        public BillStep BillStep { get; set; }
        public HistoryStep HistoryStep { get; set; }
    }
}
