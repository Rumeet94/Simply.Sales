using System.Collections.Generic;

using Simply.Sales.TelegramBot.Infrastructure.Items.Form.Data;

namespace Simply.Sales.TelegramBot.Infrastructure.Items.Form {
	public class ProductFormValue {
		public ProductFormCategoryItem Category { get; set; }

		public ProductFormItem Product { get; set; }

		public IEnumerable<ProductFormModificationItem> Modifications { get; set; }

		public bool NeedCount { get; set; }
	}
}
