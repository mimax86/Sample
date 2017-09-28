using System.ComponentModel;
using System.Runtime.Serialization;

namespace Transactions.Statics
{
    [DataContract]
    public enum TransferType
    {
        [EnumMember]
        [Description("All Types")]
        All = 0,

        [EnumMember]
        [Description("Transfer Buy Type 1")]
        TransferBuyType1 = 1,

        [EnumMember]
        [Description("Transfer Buy Type 2")]
        TransferBuyType2 = 2,

        [EnumMember]
        [Description("Transfer Sell Type 1")]
        TransferSellType1 = 3,

        [EnumMember]
        [Description("Transfer Sell Type 2")]
        TransferSellType2 = 4,

        [EnumMember]
        [Description("Transfer Adjustment Type")]
        TransferAdjustmentType = 5,

        [EnumMember]
        [Description("Transfer Type 6")]
        TransferType6 = 6,

        [EnumMember]
        [Description("Transfer Type 7")]
        TransferType7 = 7,

        [EnumMember]
        [Description("Transfer Type 8")]
        TransferType8 = 8,

        [EnumMember]
        [Description("Transfer Type 9")]
        TransferType9 = 9,

        [EnumMember]
        [Description("Transfer Type 10")]
        TransferType10 = 10,
    }
}