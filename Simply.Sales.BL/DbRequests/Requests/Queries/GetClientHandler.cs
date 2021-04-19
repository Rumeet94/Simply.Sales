using System;
using System.Diagnostics.Contracts;
using System.Threading;
using System.Threading.Tasks;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Simply.Sales.BLL.DbRequests.Dto;
using Simply.Sales.BLL.DbRequests.Handlers.Queries;
using Simply.Sales.DLL.Models.Clients;
using Simply.Sales.DLL.Repositories;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries {
	public class GetClientHandler : IRequestHandler<GetClient, TelegramClientDto> {
		private readonly IServiceProvider _serviceProvider;

		public GetClientHandler(IServiceProvider serviceProvider) {
			Contract.Requires(serviceProvider != null);

			_serviceProvider = serviceProvider;
		}

		public async Task<TelegramClientDto> Handle(GetClient request, CancellationToken cancellationToken) {
			using var scope = _serviceProvider.CreateScope();

			var repository = scope.ServiceProvider.GetRequiredService<IRepository<TelegramClient>>();

			var client = await repository.GetSingleAsync((int)request.ChatId);

			if (client == null) {
				return null;
			}

			return new TelegramClientDto {
				ChatId = client.ChatId,
				Name = client.Name,
				PhoneNumber = client.PhoneNumber
			};
		}
	}
}
