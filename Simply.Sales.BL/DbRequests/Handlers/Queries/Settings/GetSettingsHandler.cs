using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Settings;
using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Settings {
	public class GetSettingsHandler : BaseGetHandler, IRequestHandler<GetSettings, IEnumerable<SettingDto>> {
		private readonly IMapper<Setting, SettingDto> _mapper;

		public GetSettingsHandler(IServiceProvider serviceProvider, IMapper<Setting, SettingDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<SettingDto>> Handle(GetSettings request, CancellationToken cancellationToken) {
			var items = await Handle<Setting>();

			return _mapper.MapList(items);
		}
	}
}
