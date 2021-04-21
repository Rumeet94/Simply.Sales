using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Settings {
	public class GetSetting : IRequest<Setting> {
		public GetSetting(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
