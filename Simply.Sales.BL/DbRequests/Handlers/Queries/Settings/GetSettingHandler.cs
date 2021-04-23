using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Settings;
using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Settings {
	public class GetSettingHandler : BaseGetSingleHandler, IRequestHandler<GetSetting, SettingDto> {
		private readonly IMapper<Setting, SettingDto> _mapper;

		public GetSettingHandler(IServiceProvider serviceProvider, IMapper<Setting, SettingDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<SettingDto> Handle(GetSetting request, CancellationToken cancellationToken) {
			var item = await Handle<Setting>(request.Id);

			return _mapper.Map(item);
		}
	}
}
