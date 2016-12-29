using System.Linq;
using System.Net;
using System.Net.NetworkInformation;

namespace BarBot.iOS.Util
{
	public class NetworkUtil
	{
		public NetworkUtil()
		{
		}

		// Validates an IP Address string
		public static bool ValidateIPv4(string ipString)
		{
			if (string.IsNullOrWhiteSpace(ipString))
			{
				return false;
			}

			string[] splitValues = ipString.Split('.');
			if (splitValues.Length != 4)
			{
				return false;
			}

			byte tempForParsing;

			return splitValues.All(r => byte.TryParse(r, out tempForParsing));
		}

		public static string MapToIPv6(string input)
		{
			string ips = "";
			IPAddress address;
			if (IPAddress.TryParse(input, out address))
			{
				switch (address.AddressFamily)
				{
					case System.Net.Sockets.AddressFamily.InterNetwork:
						// we have IPv4, map it to IPv6
						IPAddress ip = IPAddress.Parse(input).MapToIPv6();
						ips = ip.ToString();
						break;
					case System.Net.Sockets.AddressFamily.InterNetworkV6:
						// we have IPv6, leave it as is
						ips = input;
						break;
				}
			}
			return ips;
		}
	}
}
