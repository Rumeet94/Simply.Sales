using System.Collections.Generic;

using MediatR;

using Simply.Sales.BLL.Dto.Settings;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Settings {
	public class GetSettings : IRequest<IEnumerable<SettingDto>> {
	}
}
