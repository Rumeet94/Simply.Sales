using System.Text.Json.Serialization;

namespace Simply.Sales.BLL.PosterIntegration.Responces.Items {
	public class Price {
		[JsonPropertyName("1")]
		public string CentPrice { get; set; }
	}
}
