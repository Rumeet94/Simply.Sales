using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Db;
using Simply.Sales.DLL.Models.Sales;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Db {
	public class InitDbHandler : IRequestHandler<InitDb> {
		private const string _categoryScript = @"
			INSERT INTO Categories (Name, IsVisible, ImageUrl)
			VALUES 
				('Капучино', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Американо', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Раф', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Моккачино', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Латте', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Десерты', true, 'https://drive.google.com/file/d/1ilrzqkqWiFHLYsB9IFNpwE59-5fsoeR6/view?usp=sharing'),
				('Турецкий чай', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Горячий шоколад', true, 'https://drive.google.com/file/d/1G0gXolMMqikEWRSG7MbbJmTZh-h5I0PM/view?usp=sharing'),
				('Сэндвич', true, 'https://drive.google.com/file/d/1F3IqVcpNjhGVC07UoUw7rmCuV5eMc-69/view?usp=sharing')";
		private const string _productScript = @"
			INSERT INTO Products (CategoryId, Name, Price, IsVisible)
            VALUES
                (1, '200 мл', 100, true),
                (1, '300 мл', 130, true),
                (1, '400 мл', 150, true),
   
                (2, '200 мл', 80, true),
                (2, '300 мл', 110, true),
   
                (3, '200 мл', 120, true),
                (3, '300 мл', 150, true),
                (3, '400 мл', 175, true),
    
                (4, '200 мл', 110, true),
                (4, '300 мл', 140, true),
                (4, '400 мл', 160, true),
    
                (5, '200 мл', 90, true),
                (5, '300 мл', 120, true),
                (5, '400 мл', 140, true),
    
                (6, 'Вишневое лакомство', 30, true),
                (6, 'Чоко чоко', 20, true),
                (6, 'Кекс сырник', 15, true),
                (6, 'Французская булочка', 13, true),
    
                (7, 'Фруктовый 300 мл', 120, true),
                (7, 'Фруктовый 400 мл', 150, true),
                (7, 'Энерджи 300 мл', 120, true),
                (7, 'Энерджи 400 мл', 150, true),
                (7, 'Витаминный 300 мл', 120, true),
                (7, 'Витаминный 400 мл', 150, true),
                (7, 'Релакс 300 мл', 120, true),
                (7, 'Релакс 400 мл', 150, true),
                (7, 'С бергамотом 300 мл', 120, true),
                (7, 'С бергамотом 400 мл', 150, true),
                (7, 'love tea 300 мл', 120, true),
                (7, 'Love tea 400 мл', 150, true),
    
                (8, '300 мл', 100, true),
                (8, '400 мл', 130, true),
    
                (9, 'с курицей', 120, true),
                (9, 'с рыбой', 120, true),
                (9, 'с ветчиной', 120, true)";
        private const string _productParameterScript = @"
            CREATE TEMP TABLE ProductParametersName_temp(Alias);
            INSERT INTO ProductParametersName_temp (Alias)
            VALUES 
                ('Карамель'),
                ('Кокос'),
                ('Шоколад'),
                ('Ирландский крем'),
                ('Мята'),  
                ('Апельсин'),
                ('Ваниль'),
                ('Лесной орех'),
                ('Клубника'),
                ('Вишня'),
                ('Персик'),
                ('Арбуз'),
                ('Черника'),
                ('Бабл гам'),
                ('Малина'),
                ('Мохито'),
                ('Банан');

            INSERT INTO ProductParameters (ProductId, Name, IsVisible, Price)
            SELECT
                product.Id as ProductId,
                parameter.Alias as Name,
                1 as IsVisible,
                0 as Proce
            FROM Products AS product
            CROSS JOIN ProductParametersName_temp AS parameter
            WHERE product.Id <= 14;

            DROP TABLE ProductParametersName_temp";

		private readonly IServiceProvider _serviceProvider;

		public InitDbHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task<Unit> Handle(InitDb request, CancellationToken cancellationToken) {
			try {
				using var scope = _serviceProvider.CreateScope();

				var categoryRepository = scope.ServiceProvider.GetRequiredService<IDbRepository<Category>>();
				var productRepository = scope.ServiceProvider.GetRequiredService<IDbRepository<Product>>();
				var productParameterRepository = scope.ServiceProvider.GetRequiredService<IDbRepository<ProductParameter>>();

                var categories = await categoryRepository.GetAsync();
                var products = await productRepository.GetAsync();
                var parameters = await productParameterRepository.GetAsync();

                if (!categories.ToList().Any()) {
                    await categoryRepository.ExecuteSqlScript(_categoryScript);
                }

                if (!products.ToList().Any()) {
                    await productRepository.ExecuteSqlScript(_productScript);
                }

                if (!parameters.ToList().Any()) {
                    await productParameterRepository.ExecuteSqlScript(_productParameterScript);
                }
            }
			catch (Exception e) {
				throw e.InnerException;
			}

			return Unit.Value;
		}
	}
}
