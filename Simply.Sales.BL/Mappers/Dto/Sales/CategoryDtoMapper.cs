using System.Linq;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class CategoryDtoMapper : IMapper<Category, CategoryDto> {
		private readonly IMapper<Product, ProductDto> _productDtoMapper;

		public CategoryDtoMapper() {
			_productDtoMapper = new ProductDtoMapper();
		}

		public Category BackMap(CategoryDto source) {
			if (source == null) {
				return null;
			}

			return new Category {
				Id = source.Id,
				Name = source.Name,
				IsVisible = source.IsVisible,
				Products = source.Products.Select(p => _productDtoMapper.BackMap(p))
			};
		}

		public CategoryDto Map(Category source) {
			if (source == null) {
				return null;
			}

			return new CategoryDto {
				Id = source.Id,
				Name = source.Name,
				IsVisible = source.IsVisible,
				Products = source.Products.Select(p => _productDtoMapper.Map(p))
			};
		}
	}
}
