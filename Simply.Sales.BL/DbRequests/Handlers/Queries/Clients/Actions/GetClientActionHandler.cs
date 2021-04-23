using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Actions {
	public class GetClientActionHandler : BaseGetSingleHandler, IRequestHandler<GetClientAction, ClientActionDto> {
		private readonly IMapper<ClientAction, ClientActionDto> _mapper;

		public GetClientActionHandler(IServiceProvider serviceProvider, IMapper<ClientAction, ClientActionDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<ClientActionDto> Handle(GetClientAction request, CancellationToken cancellationToken) {
			var item = await Handle<ClientAction>(request.Id);

			return _mapper.Map(item);
		}
	}
}
