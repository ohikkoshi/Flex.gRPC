using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	class Program
	{
		public static void Main(string[] args)
		{
			var boot = "build";
			var ip = "127.0.0.1";
			var port = 30000;

			if (args?.Length > 0) {
				boot = args[0];
			}
			if (args?.Length > 1) {
				ip = args[1];
			}
			if (args?.Length > 2) {
				int.TryParse(args[2], out port);
			}

			if (boot == "server") {
				Server(ip, port);
			} else if (boot == "client") {
				Client(ip, port);
			} else {
				Console.WriteLine("build only.");
			}

			Console.WriteLine("Finish.");
		}

		static void Server(string ip, int port)
		{
			Console.WriteLine($"new server({ip}:{port}).");
			Console.WriteLine("Press any key to shutdown the server...");

			var server = new Flex.RPC.BasicServer(ip, port);
			server.Start();

			Console.ReadKey();
			Console.WriteLine("Shutdown App...");
			server.Shutdown();
		}

		static void Client(string ip, int port)
		{
			Console.WriteLine($"new client({ip}:{port}).");
			Console.WriteLine("Press any key to shutdown the client...");

			var client = new Flex.RPC.BasicClient(ip, port);
			client.Start();

			client.Unary();
			client.ClientStream();
			client.ServerStream();

			Console.ReadKey();
			Console.WriteLine("Shutdown App...");
			client.Shutdown();
		}
	}
}
