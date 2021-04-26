using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using AutoMapper;

using MediatR;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Clients;
using Simply.Sales.BLL.Dto.Clients;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Handlers.Queries.Clients.Clients {
	public class GetClientByTelegramChatIdHandler : IRequestHandler<GetClientByTelegramChatId, TelegramClientDto> {
		private readonly IServiceProvider _serviceProvider;
		private readonly IMapper _mapper;

		public GetClientByTelegramChatIdHandler(IServiceProvider serviceProvider, IMapper mapper) {
			Contract.Requires(mapper != null);

			_serviceProvider = serviceProvider;
			_mapper = mapper;
		}

		public async Task<TelegramClientDto> Handle(GetClientByTelegramChatId request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IDbRepository<TelegramClient>>();
			var clients = await repository.GetAsync();
			var client = clients.ToList().FirstOrDefault(c => c.ChatId == request.ChatId);
			
			return _mapper.Map<TelegramClientDto>(client);
		}
	}
}