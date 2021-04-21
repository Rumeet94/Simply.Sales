namespace Simply.Sales.BLL.Dto.Sales {
	public class BasketItemDto : BaseDto {
		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public ProductDto Product { get; set; }

		public OrderDto Order { get; set; }
	}
}
