using Microsoft.EntityFrameworkCore;

namespace Simply.Sales.DLL.Configuration.Creater.Strategies {
	public interface IDbModelCreaterStrategy {
		void CreateModel(ModelBuilder modelBuilder);
	}
}
