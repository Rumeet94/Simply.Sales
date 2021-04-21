using System.Collections.Generic;

namespace Simply.Sales.BLL.Dto.Sales {
	public class ProductDto : BaseDto {
		public int CategoryId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public bool IsVisible { get; set; }

		public CategoryDto Category { get; set; }

		public IEnumerable<BasketItemDto> Baskets { get; set; }
	}
}
