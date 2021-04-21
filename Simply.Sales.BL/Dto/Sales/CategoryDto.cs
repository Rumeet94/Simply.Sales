using System.Collections.Generic;

namespace Simply.Sales.BLL.Dto.Sales {
	public class CategoryDto : BaseDto {
		public string Name { get; set; }

		public bool IsVisible { get; set; }

		public IEnumerable<ProductDto> Products { get; set; }
	}
}
