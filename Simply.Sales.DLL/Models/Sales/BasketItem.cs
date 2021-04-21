namespace Simply.Sales.DLL.Models.Sales {
	public class BasketItem : BaseDbModel {
		public int OrderId { get; set; }

		public int ProductId { get; set; }

		public Product Product { get; set; }

		public Order Order { get; set; }
	}
}
