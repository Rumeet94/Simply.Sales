using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Clients {
	public class UpdateTelegramClientHanlder : BaseUpdateHandler, IRequestHandler<UpdateTelegramClient> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _mapper;

		public UpdateTelegramClientHanlder(IServiceProvider serviceProvider, IMapper<TelegramClient, TelegramClientDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateTelegramClient request, CancellationToken cancellationToken) {
			var client = _mapper.BackMap(request.Dto);

			await Handle(client);

			return Unit.Value;
		}
	}
}
