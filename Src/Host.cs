using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;
	using Grpc.Proto.Ping;
	using Grpc.Proto.Rpc;


	public class Host : HostBase
	{
		public Host(string ip = "127.0.0.1", int port = 30000) : base(ip, port)
		{
			Console.WriteLine($"new server({ip}:{port}).");

			BindService(Ping.BindService(new PingService()));
			BindService(Rpc.BindService(new RpcService()));
		}

		class PingService : Ping.PingBase
		{
			public override Task<PingReply> Call(PingRequest request, ServerCallContext context)
			{
				Console.WriteLine($"{context.Host}:{context.Method}:{context.Peer}");

				return Task.FromResult(new PingReply {
					Utc = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
				});
			}
		}

		class RpcService : Rpc.RpcBase
		{
			public override Task<RpcReply> Method(RpcRequest request, ServerCallContext context)
			{
				return Task.FromResult(new RpcReply {
					Code = RpcReply.Types.Code.Success,
					Message = "Hello,gRPC!"
				});
			}
		}
	}

	public class HostBase
	{
		public string Host { get; }
		public int Port { get; }
		Server server;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public HostBase(string ip = "127.0.0.1", int port = 30000)
		{
			this.Host = ip;
			this.Port = port;
			this.server = new Server();
			this.server.Ports.Add(new ServerPort(ip, port, ServerCredentials.Insecure));
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		~HostBase() => Shutdown();

		/// <summary>
		/// .
		/// </summary>
		/// <param name="service"></param>
		public void BindService(ServerServiceDefinition service) => server.Services.Add(service);

		/// <summary>
		/// .
		/// </summary>
		public void Start() => server.Start();

		/// <summary>
		/// .
		/// </summary>
		public void Shutdown() => server.ShutdownAsync().Wait();
		public Task ShutdownAsync() => server.ShutdownAsync();
	}
}
