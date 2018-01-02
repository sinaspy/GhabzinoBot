using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayantech.WebService;

namespace GhabzinoBot
{
    public class UserInfo
    {
        public int UserID { get; set; }
        public string Mobile { get; set; }
        public string Token { get; set; } = ProjectValues.PublicToken;
        public UserState UserState { get; set; }
        public UserField UserField { get; set; }
        public WaterBillInquiryStep WaterBillInquiryStep { get; set; }
        public GasBillInquiryStep GasBillInquiryStep { get; set; }
        public ElectricityBillInquiryStep ElectricityBillInquiryStep { get; set; }
        public MciMobileBillInquiryStep MciMobileBillInquiryStep { get; set; }
        public FixedLineBillInquiryStep FixedLineBillInquiryStep { get; set; }
        public TrafficFinesInquiryStep TrafficFinesBillInquiryStep { get; set; }
    }
}
