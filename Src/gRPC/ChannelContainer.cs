#pragma warning disable 8632
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;

	/// <summary>
	/// <see href="https://grpc.github.io/grpc/csharp/api/Grpc.Core.Channel.html" />
	/// </summary>
	public class ChannelContainer
	{
		//
		public Channel Channel => channel;
		public string Host => host;
		public int Port => port;
		public int Hash => hash;

		//
		Channel channel = null!;
		string host = null!;
		int port;
		int hash;
		int connection;

		//
		public event EventHandler<ChannelState> onStateChanged = null!;


		/// <summary>
		/// .
		/// </summary>
		private ChannelContainer() { }

		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="hash"></param>
		public ChannelContainer(string host, int port, int hash)
		{
			this.host = host;
			this.port = port;
			this.hash = hash;
			this.channel = new Channel(host, port, ChannelCredentials.Insecure);
			this.connection = 0;
			Task.Run(() => OnStateChanged(channel));
		}

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
					onStateChanged?.Invoke(this, ch.State);
				}
			}
		}

		/// <summary>
		/// .
		/// </summary>
		public void Start() => StartAsync().ConfigureAwait(false);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public Task StartAsync()
		{
			if (connection > 0) {
				return Task.CompletedTask;
			}

			connection++;
			return channel?.ConnectAsync() ?? Task.CompletedTask;
		}

		/// <summary>
		/// .
		/// </summary>
		public void Shutdown() => ShutdownAsync().ConfigureAwait(false);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public Task ShutdownAsync()
		{
			if (--connection > 0) {
				return Task.CompletedTask;
			}

			connection = 0;
			return channel?.ShutdownAsync() ?? Task.CompletedTask;
		}
	}
}
#pragma warning restore 8632
