using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Settings;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Settings {
	public class AddSetting : IRequest {
		public AddSetting(SettingDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public SettingDto Dto { get; }
	}
}
