using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Actions {
	public class GetClientActionsHanlder : BaseGetHandler, IRequestHandler<GetClientActions, IEnumerable<ClientActionDto>> {
		private readonly IMapper<ClientAction, ClientActionDto> _mapper;

		public GetClientActionsHanlder(IServiceProvider serviceProvider, IMapper<ClientAction, ClientActionDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<IEnumerable<ClientActionDto>> Handle(GetClientActions request, CancellationToken cancellationToken) {
			var items = await Handle<ClientAction>();

			return _mapper.MapList(items);
		}
	}
}