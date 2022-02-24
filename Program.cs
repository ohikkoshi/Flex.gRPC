using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.gRPC
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args?.Length > 0) {
				var boot = args[0];
				var ip = args[1];
				var port = int.Parse(args[2]);

				if (boot == "server") {
					var host = new Flex.RPC.Host(ip, port);
					host.Start();

					Console.WriteLine("Press any key to shutdown the server...");
					Console.ReadKey();

					host.Shutdown();
				} else if (boot == "client") {
					var client = new Flex.RPC.Client(ip, port);
					client.Connect();

					client.Ping();
					client.Hello2();

					Console.WriteLine("Press any key to shutdown the client...");
					Console.ReadKey();

					client.Shutdown();
				}
			} else {
				Console.WriteLine("build only");
			}
		}
	}
}
