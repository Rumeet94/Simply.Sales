using Simply.Sales.BLL.Dto;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class OrderFormCreateService : IOrderFormCreateService {
		private readonly IMessageFormFactory _messageFormFactory;
		private readonly IOrderTextCreateService _orderTextCreateService;

		public OrderFormCreateService(IMessageFormFactory messageFormFactory, IOrderTextCreateService orderTextCreateService) {
			_messageFormFactory = messageFormFactory;
			_orderTextCreateService = orderTextCreateService;
		}

		public TelegramMessage Create(ClientDto client) {
			var form = _messageFormFactory.GetOrderForm(client.Id);
			var text = _orderTextCreateService.Create(client);

			return new TelegramMessage(form, text);
		}
	}
}
