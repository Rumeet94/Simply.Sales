using Microsoft.EntityFrameworkCore;

namespace Simply.Sales.DLL.Configuration.Creater {
	public interface IDbModelsCreater {
		void CreateModels(ModelBuilder modelBuilder);
	}
}
