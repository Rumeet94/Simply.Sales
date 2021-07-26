using System.Linq;
using System.Text;
using System.Text.Json;

using Simply.Sales.BLL.Dto;
using Simply.Sales.BLL.PosterIntegration.Items;
using Simply.Sales.BLL.Servicies.Basket;
using Simply.Sales.BLL.Servicies.Delivery;
using Simply.Sales.BLL.Servicies.Orders;
using Simply.Sales.TelegramBot.Infrastructure.Enums;
using Simply.Sales.TelegramBot.Infrastructure.Items.Form;

namespace Simply.Sales.TelegramBot.Infrastructure.Servicies.Message.Create {
	public class OrderTextCreateService : IOrderTextCreateService {
		private readonly IOrderDbService _orderService;
		private readonly IBasketDbService _basketService;
		private readonly IDeliveryDbService _deliveryDbService;
		private readonly PosterMenu _menu;

		public OrderTextCreateService(
			IOrderDbService orderService,
			IBasketDbService basketService,
			IDeliveryDbService deliveryDbService,
			PosterMenu menu
		) {
			_orderService = orderService;
			_basketService = basketService;
			_deliveryDbService = deliveryDbService;
			_menu = menu;
		}

		public string Create(ClientDto client, string nickName = null, OrderTextType type = OrderTextType.ForClient) {
			var isClientText = type == OrderTextType.ForClient;
			var order = _orderService.GetNotCompletedOrder(client.Id);
			var basket = _basketService
				.GetBaksetByOrder(order.Id)
				.Select(i => JsonSerializer.Deserialize<BasketProduct>(i.Data));
			var zones = _deliveryDbService.GetClientZones(client.Id);
			var zone = zones?.FirstOrDefault(z => z.ZoneId == order.DeliveryZoneId);
			var builder = new StringBuilder(isClientText ? "📦 *Ваш заказ *:\n" : $"Заказ №{order.Id}:\n");

			if (!isClientText) {
				builder
					.AppendLine($"Клиент: {client.Name} (@{nickName}, {client.PhoneNumber})")
					.AppendLine();
			}

			foreach (var item in basket) {
				GetProductInfo(builder, item);

				builder
					.AppendLine($"   - Количество: {item.Count} шт.")
					.AppendLine($"   - *Цена*: {item.Price} руб")
					.AppendLine();
			}

			builder
				.AppendLine($"*Комментарий*: {order.Comment ?? "без комментария"}")
				.AppendLine();

			if (isClientText) {
				builder.AppendLine($"*Общая стоимость*: {basket.Sum(b => b.Price)} руб.");
			}

			if (!isClientText) {
				var needDeliveryText = order.NeedDelivery.Value ? "нужна" : "не нужна";

				builder
					.AppendLine()
					.AppendLine($"Доставка: {needDeliveryText}")
					.AppendLine($"Время выдачи(доставки): {order.DateReceiving:hh:mm:ss}");
			}

			if (zone != null) {
				builder.AppendLine($"*Адрес доставки*: {zone.ZoneName} {zone.ZoneDescription}");
			}

			return builder.ToString();
		}

		private void GetProductInfo(StringBuilder builder, BasketProduct data) {
			var product = _menu.Products.FirstOrDefault(p => p.Id == data.ProductId);

			builder.AppendLine($"{product.Name} ({product.Price} руб.)");

			if (data.Mods == null || !data.Mods.Any()) {
				return;
			}

			foreach (var item in data.Mods) {
				var group = product.ModificationGroups.FirstOrDefault(g => g.Id == item.ModGroupId);
				var mod = group.Modifications.FirstOrDefault(m => m.Id == item.ModId);

				builder.AppendLine($"   - {mod.Name} (+{mod.Price} руб.)");
			}
		}
	}
}
