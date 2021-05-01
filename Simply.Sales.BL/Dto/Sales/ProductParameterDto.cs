using System.Collections.Generic;

namespace Simply.Sales.BLL.Dto.Sales {
	public class ProductParameterDto : BaseDto {
		public int ProductId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public bool IsVisible { get; set; }

		public ProductDto Product { get; set; }

		public IEnumerable<BasketItemDto> Baskets { get; set; }
	}
}
