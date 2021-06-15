using Simply.Sales.TelegramBot.Infrastructure.Enums;

namespace Simply.Sales.TelegramBot.Infrastructure.Items {
	/// <summary>
	/// Query кнопки. Названия свойств сокращены из-за ограничения размера query у кнопок телеграмма
	/// </summary>
	public class SelectItem {
		/// <summary>
		/// Тип будущего действия
		/// </summary>
		public IncomeMessageType T { get; set; }

		/// <summary>
		/// Категория продуктов
		/// </summary>
		public int? CId { get; set; }

		/// <summary>
		/// Id продукта
		/// </summary>
		public int? PId { get; set; }

		/// <summary>
		/// Id параметра продукта
		/// </summary>
		public int? PPId { get; set; }

		/// <summary>
		/// Id корзины
		/// </summary>
		public int? BId { get; set; }

		/// <summary>
		/// Id заказа
		/// </summary>
		public int? OId { get; set; }

		/// <summary>
		/// Id пользователя в телеграмме (ChatId)
		/// </summary>
		public long CI { get; set; }

		/// <summary>
		/// Скидка
		/// </summary>
		public decimal? D { get; set; }

		/// <summary>
		/// Флаг доставки
		/// </summary>
		public bool? ND { get; set; }

		/// <summary>
		/// Цена
		/// </summary>
		public OrderPrice P { get; set; }
	}
}
