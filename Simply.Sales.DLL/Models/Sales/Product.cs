namespace Simply.Sales.DLL.Models.Sales {
	public class Product {
		public int Id { get; set; }

		public int CategoryId { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }

		public bool IsVisible { get; set; }

		public Category Category { get; set; }
	}
}
