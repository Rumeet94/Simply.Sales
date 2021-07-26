using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simply.Sales.BLL.PosterIntegration.Servicies {
	public interface IPosterService {
		Task GetMenu();

		Task CreateOrder();
	}
}
