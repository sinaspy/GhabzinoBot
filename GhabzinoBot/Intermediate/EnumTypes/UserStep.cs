using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhabzinoBot
{
    public enum WaterBillInquiryStep//1000
    {
        None = 1000,//default
        Inquiring=1100,
        Inqueried=1200,
    }
    public enum GasBillInquiryStep//2000
    {
        None = 2000,//default
        Inquiring = 2100,
        Inqueried = 2200,
    }
    public enum ElectricityBillInquiryStep//3000
    {
        None = 3000,//default
        Inquiring = 3100,
        Inqueried = 3200,
    }
    public enum MciMobileBillInquiryStep//4000
    {
        None = 4000,//default
        Inquiring = 4100,
        Inqueried = 4200,
    }
    public enum FixedLineBillInquiryStep//5000
    {
        None = 5000,//default
        Inquiring = 5100,
        Inqueried = 5200,
    }
    public enum TrafficFinesInquiryStep//6000
    {
        None = 6000,//default
        Inquiring = 6100,
        Inqueried = 6200,
    }
}
