using System;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using Microsoft.Extensions.DependencyInjection;

using Simply.Sales.BLL.Dto;
using Simply.Sales.DLL.Context;
using Simply.Sales.DLL.Models.Clients;

using Telegram.Bot.Types;

namespace Simply.Sales.BLL.Servicies.Clients {
	public class ClientDbService : IClientDbService {
		private readonly SalesDbContext _dbContext;
		private readonly IMapper _mapper;

		public ClientDbService(IServiceProvider serviceProvider, IMapper mapper) {
			var scope = serviceProvider.CreateScope();

			_dbContext = scope.ServiceProvider.GetRequiredService<SalesDbContext>();
			_mapper = mapper;
		}

		public ClientDto Get(long chatId) {
			var client = _dbContext.Clients.FirstOrDefault(c => c.ChatId == chatId);

			return _mapper.Map<ClientDto>(client);
		}

		public async Task<int> Registration(Contact contact) {
			var data = new Client() {
				ChatId = contact.UserId,
				Name = contact.FirstName,
				DateRegistered = DateTime.Now,
				PhoneNumber = contact.PhoneNumber
			};
			var client = await _dbContext.Clients.AddAsync(data);

			await _dbContext.SaveChangesAsync();

			return client.Entity.Id;
		}
	}
}
