namespace Simply.Sales.BLL.PosterIntegration.Items {
	public class ProductModification {
		public ProductModification(int id, string name, decimal price) {
			Id = id;
			Name = name;
			Price = price;
		}

		public int Id { get; set; }

		public string Name { get; set; }

		public decimal Price { get; set; }
	}
}
