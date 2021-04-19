using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using Simply.Sales.TelegramBot.Infrastructure.Servicies.Bot;

namespace Simply.Sales.TelegramBot.Controllers {
	[Route("api/telegram-bot")]
	[ApiController]
	public class TelegramBotApiController : ControllerBase {
		private readonly ITelegramBotService _botService;

		public TelegramBotApiController(ITelegramBotService botService) {
			Contract.Requires(botService != null);

			_botService = botService;
		}

		[HttpGet("start")]
		public async Task<IActionResult> Start() {
			await _botService.Watch();

			return Ok();
		}

		[HttpGet("stop")]
		public IActionResult Stop() {
			_botService.StopWatch();

			return Ok();
		}

	}
}
