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
	public class AddTelegramClientHandler : BaseCreateHandler, IRequestHandler<AddTelegramClient> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _mapper;

		public AddTelegramClientHandler(IServiceProvider serviceProvider, IMapper<TelegramClient, TelegramClientDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(AddTelegramClient request, CancellationToken cancellationToken) {
			var client = _mapper.BackMap(request.Dto);

			await Handle(client);

			return Unit.Value;
		}
	}
}
