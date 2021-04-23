using System.Linq;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class ProductDtoMapper : IMapper<Product, ProductDto> {
		private readonly IMapper<Category, CategoryDto> _categoryDtoMapper;
		private readonly IMapper<BasketItem, BasketItemDto> _basketDtoMapper;

		public ProductDtoMapper() {
			_categoryDtoMapper = new CategoryDtoMapper();
			_basketDtoMapper = new BasketItemDtoMapper();
		}

		public Product BackMap(ProductDto source) {
			if (source == null) {
				return null;
			}

			return new Product {
				Id = source.Id,
				CategoryId = source.CategoryId,
				Name = source.Name,
				Price = source.Price,
				IsVisible = source.IsVisible,
				Category = _categoryDtoMapper.BackMap(source.Category),
				Baskets = source.Baskets.Select(b => _basketDtoMapper.BackMap(b))
			};
		}

		public ProductDto Map(Product source) {
			if (source == null) {
				return null;
			}

			return new ProductDto {
				Id = source.Id,
				CategoryId = source.CategoryId,
				Name = source.Name,
				Price = source.Price,
				IsVisible = source.IsVisible,
				Category = _categoryDtoMapper.Map(source.Category),
				Baskets = source.Baskets.Select(b => _basketDtoMapper.Map(b))
			};
		}
	}
}
