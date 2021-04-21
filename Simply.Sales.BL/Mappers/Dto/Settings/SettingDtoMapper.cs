using Simply.Sales.BLL.Dto.Settings;
using Simply.Sales.DLL.Models.Settings;

namespace Simply.Sales.BLL.Mappers.Dto.Settings {
	public class SettingDtoMapper : IMapper<Setting, SettingDto> {
		public Setting BackMap(SettingDto source) =>
			new Setting {
				Id = source.Id,
				Name = source.Name,
				Value = source.Value
			};

		public SettingDto Map(Setting source) =>
			new SettingDto {
				Id = source.Id,
				Name = source.Name,
				Value = source.Value
			};
	}
}
