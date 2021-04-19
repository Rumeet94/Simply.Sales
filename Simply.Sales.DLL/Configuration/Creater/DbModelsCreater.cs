using System.Diagnostics.Contracts;
using System.Linq;

using Microsoft.EntityFrameworkCore;

using Simply.Sales.DLL.Configuration.Mapper;

namespace Simply.Sales.DLL.Configuration.Creater {
	public class DbModelsCreater : IDbModelsCreater {
		private readonly IDbModelsCreaterMapper _mapper;

		public DbModelsCreater(IDbModelsCreaterMapper mapper) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public void CreateModels(ModelBuilder modelBuilder) {
			var models = _mapper.Map().ToList();

			models.ForEach(m => m.CreateModel(modelBuilder));
		}
	}
}
