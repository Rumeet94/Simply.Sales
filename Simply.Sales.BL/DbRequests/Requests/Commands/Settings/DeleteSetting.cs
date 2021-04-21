using System.Diagnostics.Contracts;

using MediatR;

namespace Simply.Sales.BLL.DbRequests.Requests.Commands.Settings {
	public class DeleteSetting : IRequest {
		public DeleteSetting(int id) {
			Contract.Requires(id > 0);

			Id = id;
		}

		public int Id { get; }
	}
}
