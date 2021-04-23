using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Settings;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Settings {
	public class GetSetting : IRequest<SettingDto> {
		public GetSetting(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
