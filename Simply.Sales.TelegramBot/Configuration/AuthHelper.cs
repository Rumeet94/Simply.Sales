using System;
using System.Security.Cryptography;
using System.Text;

namespace Simply.Sales.TelegramBot.Configuration {
	internal static class AuthHelper {
		private const string _firstValue = "AidarRAF";
		private const string _secondValue = "million";

		internal static string SearchMethodUrl => @"https://b2b.vianor-tyres.ru/api/b2b/v1/search";

		internal static string GetMd5Token() {
			var yearNumber = DateTime.UtcNow.Year;
			var monthNumber = DateTime.UtcNow.Month;
			var tokenArgument = $"{_firstValue}{monthNumber}{_secondValue}{yearNumber}";

			using var md5 = MD5.Create();

			byte[] inputBytes = Encoding.ASCII.GetBytes(tokenArgument);
			byte[] hashBytes = md5.ComputeHash(inputBytes);

			var builder = new StringBuilder();
			for (int i = 0; i < hashBytes.Length; i++) {
				builder.Append(hashBytes[i].ToString("x2"));
			}

			return builder.ToString();
		}
	}
}
