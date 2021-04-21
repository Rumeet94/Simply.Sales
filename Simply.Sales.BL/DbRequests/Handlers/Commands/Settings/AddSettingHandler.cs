using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Settings;
using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Settings {
	public class AddSettingHandler : BaseCreateHandler, IRequestHandler<AddSetting> {
		private readonly IMapper<Setting, SettingDto> _mapper;

		public AddSettingHandler(IServiceProvider serviceProvider, IMapper<Setting, SettingDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddSetting request, CancellationToken cancellationToken) {
			var setting = _mapper.BackMap(request.Dto);

			await Handle(setting);

			return Unit.Value;
		}
	}
}
