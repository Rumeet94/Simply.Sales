using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Microsoft.AspNetCore.Mvc;

using Simply.Sales.BLL.DbRequests.Requests.Commands.Db;

namespace Simply.Sales.TelegramBot.Controllers {
	[Route("api/db")]
	[ApiController]
	public class DbController : Controller {
		private readonly IMediator _mediator;

		public DbController(IMediator mediator) {
			Contract.Requires(mediator != null);

			_mediator = mediator;
		}

		[HttpPost("init")]
		public async Task<IActionResult> InitializeDb() {
			try {
				await _mediator.Send(new InitDb());

				return Ok();
			}
			catch {
				return BadRequest();
			}
		}
	}
}
