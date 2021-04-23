using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients {
	public class GetClientsHandler : BaseGetHandler, IRequestHandler<GetClients, IEnumerable<TelegramClientDto>> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _mapper;

		public GetClientsHandler(IServiceProvider serviceProvider, IMapper<TelegramClient, TelegramClientDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<TelegramClientDto>> Handle(GetClients request, CancellationToken cancellationToken) {
			var items = await Handle<TelegramClient>();

			return _mapper.MapList(items);
		}
	}
}