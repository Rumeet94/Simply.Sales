using System.Diagnostics.Contracts;

namespace Simply.Sales.BLL.PosterIntegration.Items {
	public class PosterProductCategory {
		public PosterProductCategory(int id, int parentId, string name, string photoUrl) {
			Contract.Requires(id > 0);
			Contract.Requires(string.IsNullOrWhiteSpace(name));
			Contract.Requires(string.IsNullOrWhiteSpace(photoUrl));

			Id = id;
			ParentId = parentId;
			Name = name;
			PhotoUrl = photoUrl;
		}

		public int Id { get; set; }

		public int ParentId { get; set; }

		public string Name { get; set; }

		public string PhotoUrl { get; set; }
	}
}
