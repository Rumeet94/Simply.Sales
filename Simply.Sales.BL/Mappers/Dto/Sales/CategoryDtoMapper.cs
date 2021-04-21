using System.Diagnostics.Contracts;
using System.Linq;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class CategoryDtoMapper : IMapper<Category, CategoryDto> {
		private readonly IMapper<Product, ProductDto> _productDtoMapper;

		public CategoryDtoMapper(IMapper<Product, ProductDto> productDtoMapper) {
			Contract.Requires(productDtoMapper != null);

			_productDtoMapper = productDtoMapper;
		}

		public Category BackMap(CategoryDto source) =>
			new Category {
				Id = source.Id,
				Name = source.Name,
				IsVisible = source.IsVisible,
				Products = source.Products.Select(p => _productDtoMapper.BackMap(p))
			};

		public CategoryDto Map(Category source) =>
			new CategoryDto {
				Id = source.Id,
				Name = source.Name,
				IsVisible = source.IsVisible,
				Products = source.Products.Select(p => _productDtoMapper.Map(p))
			};
	}
}
