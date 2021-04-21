using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.BLL.Mappers;
using Simply.Sales.DLL.Models.Clients;

namespace Simply.Sales.BLL.DbRequests.Handlers.Commands.Clients.Action {
	public class UpdateClientActionHanlder : BaseUpdateHandler, IRequestHandler<UpdateClientAction> {
		private readonly IMapper<ClientAction, ClientActionDto> _mapper;

		public UpdateClientActionHanlder(IServiceProvider serviceProvider, IMapper<ClientAction, ClientActionDto> mapper)
			: base(serviceProvider) {
			Contract.Requires(mapper != null);

			_mapper = mapper;
		}

		public async Task<Unit> Handle(UpdateClientAction request, CancellationToken cancellationToken) {
			var action = _mapper.BackMap(request.Dto);

			await Handle(action);

			return Unit.Value;
		}
	}
}
