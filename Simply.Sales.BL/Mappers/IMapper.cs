using System.Collections.Generic;
using System.Linq;

namespace Simply.Sales.BLL.Mappers {
	public interface IMapper<Tin, Tout> {
		Tout Map(Tin source);

		Tin BackMap(Tout source);

		IEnumerable<Tout> MapList(IEnumerable<Tin> sourse) {
			if (sourse == null || !sourse.Any()) {
				return null;
			}

			return sourse.Select(s => Map(s));
		}

		IEnumerable<Tin> BackMapList(IEnumerable<Tout> sourse) {
			if (sourse == null || !sourse.Any()) {
				return null;
			}

			return sourse.Select(s => BackMap(s));
		}
	}
}
