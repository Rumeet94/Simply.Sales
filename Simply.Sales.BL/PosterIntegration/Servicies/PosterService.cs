using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Extensions.Logging;

using Simply.Sales.BLL.Builders;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.PosterIntegration.Responces;

namespace Simply.Sales.BLL.PosterIntegration.Servicies {
	public class PosterService : IPosterService {
		private const string _requestTemplate = @"https://joinposter.com/api/{0}?token=916503:245638828af36a3ec11003a10394d376"; //кофе
		//private const string _requestTemplate = @"https://joinposter.com/api/{0}?token=735128:85620647b22eec543192e75eee636cbe"; //кальян

		private readonly IBuilder<PosterMenuResponce, PosterMenu> _menuBuilder;
		private readonly HttpClient _client;
		private readonly PosterMenu _posterMenu;
		private readonly ILogger<PosterService> _logger;

		public PosterService(
			IBuilder<PosterMenuResponce, PosterMenu> menuBuilder,
			HttpClient client,
			PosterMenu posterMenu,
			ILogger<PosterService> logger
		) {
			_menuBuilder = menuBuilder;
			_client = client;
			_posterMenu = posterMenu;
			_logger = logger;
		}

		public Task CreateOrder() {
			return null;
		}

		public Task GetMenu() {
			try {
				var categories = GetCategories();
				var products = GetProducts();

				Task.WaitAll(categories, products);

				if (categories.Result == null || products.Result == null) {
					throw new Exception();
				}
				var menu = new PosterMenuResponce(categories.Result, products.Result);
				var newMenu = _menuBuilder.Build(menu);

				_posterMenu.Categories = newMenu.Categories;
				_posterMenu.Products = newMenu.Products;
			}
			catch(Exception e) {
				_logger.LogError($"Error on Poster Api. {e.Message}");
			}

			return Task.CompletedTask;
		}

		private async Task<CategoriesResponse> GetCategories() {
			using var responce = await _client.GetAsync(string.Format(_requestTemplate, "menu.getCategories"));

			var json = await responce.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<CategoriesResponse>(json);
		}

		private async Task<ProductsResponse> GetProducts() {
			using var responce = await _client.GetAsync(string.Format(_requestTemplate, "menu.getProducts"));

			var json = await responce.EnsureSuccessStatusCode().Content.ReadAsStringAsync();

			return JsonSerializer.Deserialize<ProductsResponse>(json);
		}
	}
}
