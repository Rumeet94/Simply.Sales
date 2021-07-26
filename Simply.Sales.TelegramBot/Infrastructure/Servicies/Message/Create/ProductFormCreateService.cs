using System.Linq;

using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.TelegramBot.Infrastructure.Factories.Messages.Form;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Callback;
using Simply.Sales.TelegramBot.Infrastructure.Items.Keyboards;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class ProductFormCreateService : IProductFormCreateService {
		private const string _text = "Используя констурктор, соберите свой продукт";

		private readonly IMessageFormFactory _messageFormFactory;
		private readonly PosterMenu _menu;

		public ProductFormCreateService(IMessageFormFactory messageFormFactory, PosterMenu menu) {
			_messageFormFactory = messageFormFactory;
			_menu = menu;
		}

		public TelegramMessage Create(FormCallback callback) {
			var photoUrl = _menu.Categories.FirstOrDefault(c => c.Id == callback.CategoryId)?.PhotoUrl;
			var form = _messageFormFactory.GetProductForm(callback);

			return new TelegramMessage(form, _text, photoUrl);
		}
	}
}