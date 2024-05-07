using System.Runtime.Serialization;

namespace OnlineShoppingApp.Common
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,
        [EnumMember(Value = "Payment_Received")]
        PaymentReceived,
        [EnumMember(Value = "Payment_Failed")]
        PaymentFailed,
    }
}
