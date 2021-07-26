namespace Simply.Sales.BLL.Dto.Orders {
	public class BasketItemDto : BaseDto {
		public int OrderId { get; set; }

		public string Data { get; set; }

		public OrderDto Order { get; set; }
	}
}
