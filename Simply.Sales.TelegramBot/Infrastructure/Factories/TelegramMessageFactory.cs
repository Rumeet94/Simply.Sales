using Simply.Sales.TelegramBot.Infrastructure.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace Simply.Sales.TelegramBot.Infrastructure.Factories {
	public class TelegramMessageFactory : ITelegramMessageFactory {
		public KeyboardItem CreateKeyboard(string userMessageText) {
			if (userMessageText.Equals("/startars")) {
				var markup = new InlineKeyboardMarkup(
					new[] {
						new[] { InlineKeyboardButton.WithCallbackData("Сделать заказ", "/request_menu") },
						new[] { InlineKeyboardButton.WithCallbackData("Узнать aдрес", "/company_address") },
						new[] { InlineKeyboardButton.WithCallbackData("Тех. поддержка", "/company_support") },
						new[] { InlineKeyboardButton.WithCallbackData("Изменить свои данные", "/edit_profile") }
					}
				);

				return new KeyboardItem(markup, "Выберите действие");
			}

			if (userMessageText.Equals("/request_menu")) {
				var markup = new InlineKeyboardMarkup(
					new[] {
						new[] {
							InlineKeyboardButton.WithCallbackData("Кофе", "/menu_coffee"),
							InlineKeyboardButton.WithCallbackData("Выпечка", "/menu_cakes")
						},
						new[] { InlineKeyboardButton.WithCallbackData("Напитки", "/menu_beverage") },
					}
				);

				return new KeyboardItem(markup, "Выберите категорию");
			}

			if (userMessageText.Equals("/menu_coffee")) {
				var markup = new InlineKeyboardMarkup(
					new[] {
						new[] {
							InlineKeyboardButton.WithCallbackData("Раф", "/coffee_raf"),
							InlineKeyboardButton.WithCallbackData("Латте", "/coffee_latte")
						},
						new[] { InlineKeyboardButton.WithCallbackData("Американо", "/coffee_americano") },
					}
				);

				return new KeyboardItem(markup, "Выберите кофе");
			}

			if ((new[] { "/coffee_raf", "/coffee_latte", "/coffee_americano" }).Contains(userMessageText)) {
				var markup = new InlineKeyboardMarkup(
					new[] {
						new[] {
							InlineKeyboardButton.WithCallbackData("0.2 л", "/cost_60" ),
							InlineKeyboardButton.WithCallbackData("0.4 л", "/cost_120"),
							InlineKeyboardButton.WithCallbackData("0.6 л", "/cost_180")
						}
					}
				);

				return new KeyboardItem(markup, "Выберите объем");
			}

			if ((new[] { "/cost_60", "/cost_120", "/cost_180" }).Contains(userMessageText)) {
				var markup = new InlineKeyboardMarkup(
					new[] {
						new[] { InlineKeyboardButton.WithUrl("Оплатить", @"https://money.alfabank.ru/p2p/web/transfer/arafikov3739") },
						new[] {
							InlineKeyboardButton.WithCallbackData("Я оплатил(а)", "/payment_success"),
							InlineKeyboardButton.WithCallbackData("Я передумал", "/payment_cansel")
						}
					}
				);

				return new KeyboardItem(markup, "Перейдите по ссылке, оплатите заказ и подтвердите платёж");
			}

			return null;
		}
	}
}
