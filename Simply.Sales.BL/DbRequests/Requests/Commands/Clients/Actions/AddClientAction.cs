﻿using System.Diagnostics.Contracts;

using MediatR;

using Simply.Sales.BLL.Dto.Clients;

namespace Simply.Sales.BLL.DbRequests.Requests.Queries.Clients.Actions {
	public class AddClientAction : IRequest {
		public AddClientAction(ClientActionDto dto) {
			Contract.Requires(dto != null);

			Dto = dto;
		}

		public ClientActionDto Dto { get; }
	}
}
