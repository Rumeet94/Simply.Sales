using System.Collections.Generic;

using Simply.Sales.DLL.Configuration.Creater.Strategies;
using Simply.Sales.DLL.Configuration.Creater.Strategies.Clients;
using Simply.Sales.DLL.Configuration.Creater.Strategies.Sales;
using Simply.Sales.DLL.Configuration.Creater.Strategies.Settings;

namespace Simply.Sales.DLL.Configuration.Mapper {
	public class DbModelsCreaterMapper : IDbModelsCreaterMapper {
		public IEnumerable<IDbModelCreaterStrategy> Map() {
			yield return new TelegramClientDbModelCreaterStrategy();
			yield return new CategoryDbModelCreaterStrategy();
			yield return new ProductDbModelCreaterStrategy();
			yield return new SettingDbModelCreaterStrategy();
			yield return new BasketDbModelCreaterStrategy();
			yield return new OrderDbModelCreaterStrategy();
			yield return new ClientActionDbModelCreaterStrategy();
		}
	}
}
