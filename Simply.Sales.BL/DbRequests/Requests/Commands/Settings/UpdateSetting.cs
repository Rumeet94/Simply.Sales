using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Settings;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Settings {
	public class UpdateSetting : IRequest {
		public UpdateSetting(SettingDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public SettingDto Dto { get; }
	}
}
