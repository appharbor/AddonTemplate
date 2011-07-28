using System;
using System.Security.Cryptography;
using System.Text;

namespace AddonTemplate.Web
{
	internal static class StringExtensions
	{
		public static string ToHash<T>(this string value) where T : HashAlgorithm, new()
		{
			using (HashAlgorithm hashAlgorithm = new T())
			{
				var valueBytes = Encoding.UTF8.GetBytes(value);

				var hashBytes = hashAlgorithm.ComputeHash(valueBytes);

				return BitConverter
					.ToString(hashBytes)
					.ToLower()
					.Replace("-", "");
			}
		}
	}
}
