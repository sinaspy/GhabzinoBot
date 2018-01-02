using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GhabzinoBot
{
    public enum UserField
    {
        None = 0,//Default

        WaterBillInquiry,
        GasBillInquiry,
        ElectricityBillInquiry,

        MciMobileBillInquiry,
        FixedLineBillInquiry,

        TrafficFinesInquery,

        Bill,
        History,
        Payments,

        PayingBills,
    }
}