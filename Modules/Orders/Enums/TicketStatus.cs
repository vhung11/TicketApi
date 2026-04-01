namespace TicketApi.Modules.Orders.Enums
{
    public enum TicketStatus
    {
        Available,   // chưa ai đặt
        Reserved,    // đang giữ (trong order)
        Paid,        // đã thanh toán
        Cancelled,   // bị hủy
        Expired      // hết hạn giữ
    }
}
