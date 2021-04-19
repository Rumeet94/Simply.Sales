using Simply.Sales.DLL.Configuration.Creater.Strategies;
using System.Collections.Generic;

namespace Simply.Sales.DLL.Configuration.Mapper {
	public interface IDbModelsCreaterMapper {
		IEnumerable<IDbModelCreaterStrategy> Map();
	}
}
