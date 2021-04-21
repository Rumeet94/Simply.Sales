using System;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Settings;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Settings {
	public class DeleteSettingHandler : BaseDeleteHandler, IRequestHandler<DeleteSetting> {
		public DeleteSettingHandler(IServiceProvider serviceProvider)
			: base(serviceProvider) {
		}

		public async Task<Unit> Handle(DeleteSetting request, CancellationToken cancellationToken) {
			await Handle<Setting>(request.Id);

			return Unit.Value;
		}
	}
}

