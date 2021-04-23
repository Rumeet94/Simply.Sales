using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients {
	public class GetClientHandler : BaseGetSingleHandler, IRequestHandler<GetClient, TelegramClientDto> {
		private readonly IMapper<TelegramClient, TelegramClientDto> _mapper;

		public GetClientHandler(IServiceProvider serviceProvider, IMapper<TelegramClient, TelegramClientDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<TelegramClientDto> Handle(GetClient request, CancellationToken cancellationToken) {
			var client = await Handle<TelegramClient>(request.Id);

			return _mapper.Map(client);
		}
	}
}