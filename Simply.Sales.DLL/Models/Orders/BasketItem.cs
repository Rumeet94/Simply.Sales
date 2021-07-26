namespace Simply.Sales.DLL.Models.Orders {
	public class BasketItem : BaseDbModel {
		public int OrderId { get; set; }

		public string Data { get; set; }

		public Order Order { get; set; }
	}
}
