using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.Mappers.Dto.Settings {
	public class SettingDtoMapper : IMapper<Setting, SettingDto> {
		public Setting BackMap(SettingDto source) {
			if (source == null) {
				return null;
			}

			return new Setting {
				Id = source.Id,
				Name = source.Name,
				Value = source.Value
			};
		}

		public SettingDto Map(Setting source) {
			if (source == null) {
				return null;
			}

			return new SettingDto {
				Id = source.Id,
				Name = source.Name,
				Value = source.Value
			};
		}
	}
}
