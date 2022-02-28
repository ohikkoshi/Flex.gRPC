using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;

	public class ServerBase
	{
		//
		Server server = null!;


		/// <summary>
		/// .
		/// </summary>
		private ServerBase() { }

		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		public ServerBase(string host = "127.0.0.1", int port = 30000)
		{
			this.server = new Server();
			this.server.Ports.Add(new ServerPort(host, port, ServerCredentials.Insecure));
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		~ServerBase() => Shutdown();

		/// <summary>
		/// .
		/// </summary>
		/// <param name="service"></param>
		public void BindService(ServerServiceDefinition service) => server?.Services.Add(service);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public bool Start()
		{
			try {
				server?.Start();
			} catch (System.IO.IOException) {
				server?.KillAsync();
				return false;
			}

			return true;
		}

		/// <summary>
		/// .
		/// </summary>
		public void Shutdown() => ShutdownAsync().ConfigureAwait(false);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public Task ShutdownAsync() => server?.ShutdownAsync() ?? Task.CompletedTask;
	}
}
