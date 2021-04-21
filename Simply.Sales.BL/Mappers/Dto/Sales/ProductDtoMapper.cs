﻿using System.Diagnostics.Contracts;
using System.Linq;

using Simply.Sales.BLL.Dto.Sales;
using Simply.Sales.DLL.Models.Sales;

namespace Simply.Sales.BLL.Mappers.Dto.Sales {
	public class ProductDtoMapper : IMapper<Product, ProductDto> {
		private readonly IMapper<Category, CategoryDto> _categoryDtoMapper;
		private readonly IMapper<BasketItem, BasketItemDto> _basketDtoMapper;

		public ProductDtoMapper(
			IMapper<Category, CategoryDto> categoryDtoMapper,
			IMapper<BasketItem, BasketItemDto> basketDtoMapper
		) {
			Contract.Requires(categoryDtoMapper != null);
			Contract.Requires(basketDtoMapper != null);
			_categoryDtoMapper = categoryDtoMapper;
			_basketDtoMapper = basketDtoMapper;
		}

		public Product BackMap(ProductDto source) =>
			new Product {
				Id = source.Id,
				CategoryId = source.CategoryId,
				Name = source.Name,
				Price = source.Price,
				IsVisible = source.IsVisible,
				Category = _categoryDtoMapper.BackMap(source.Category),
				Baskets = source.Baskets.Select(b => _basketDtoMapper.BackMap(b))
			};

		public ProductDto Map(Product source) =>
			new ProductDto {
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
