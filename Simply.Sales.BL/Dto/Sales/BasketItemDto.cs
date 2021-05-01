namespace Simply.Sales.BLL.Dto.Sales {
	public class BasketItemDto : BaseDto {
		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public int? ProductParameterId { get; set; }

		public ProductDto Product { get; set; }

		public OrderDto Order { get; set; }

		public ProductParameterDto ProductParameter { get; set; }
	}
}
