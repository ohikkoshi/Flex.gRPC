using System;
using System.Linq;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;
	using Grpc.Proto.Ping;
	using Grpc.Proto.Rpc;

	public class Client : ClientBase
	{
		Ping.PingClient ping;
		Rpc.RpcClient rpc;


		public Client(string ip = "127.0.0.1", int port = 30000) : base(ip, port)
		{
			Console.WriteLine($"new client({ip}:{port}).");

			onStateChanged = (state) => {
				Console.WriteLine($"{Host}:{Port} - {state.ToString()}");
			};

			ping = new Ping.PingClient(Channel);
			rpc = new Rpc.RpcClient(Channel);
		}

		public void Ping()
		{
			var req = new PingRequest();
			var res = ping.Call(req);
			Console.WriteLine($"Ping.Call({res.Utc})");
		}

		public void Hello2()
		{
			var req = new RpcRequest();
			var res = rpc.Method(req);
			Console.WriteLine($"Rpc({res.Code.ToString()}, {res.Message})");
		}
	}

	public class ClientBase
	{
		public string Host { get; }
		public int Port { get; }
		public Channel Channel => channel;
		Channel channel;
		protected System.Action<ChannelState>? onStateChanged;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public ClientBase(string ip, int port)
		{
			this.Host = ip;
			this.Port = port;
			this.channel = new Channel(ip, port, ChannelCredentials.Insecure);
			Task.Run(() => OnStateChanged(channel));
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		~ClientBase() => Shutdown();

		/// <summary>
		/// .
		/// </summary>
		public void Connect() => channel.ConnectAsync().Wait();
		public Task ConnectAsync() => channel.ConnectAsync();

		/// <summary>
		/// .
		/// </summary>
		public void Shutdown() => channel.ShutdownAsync().Wait();
		public Task ShutdownAsync() => channel.ShutdownAsync();

		/// <summary>
		/// .
		/// </summary>
		/// <param name="ch"></param>
		/// <returns></returns>
		async Task OnStateChanged(Channel ch)
		{
			while (ch.State != ChannelState.Shutdown) {
				var state = ch.State;

				if (await ch.TryWaitForStateChangedAsync(state).ConfigureAwait(false)) {
					onStateChanged?.Invoke(ch.State);
				}
			}
		}
	}
}
