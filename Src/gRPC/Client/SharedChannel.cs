#pragma warning disable 8632
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf;
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;

	public partial class SharedChannel
	{
		//
		static ConcurrentDictionary<int, SharedChannel> channels = new ConcurrentDictionary<int, SharedChannel>();


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public static SharedChannel Instantiate(string host, int port)
		{
			int hash = $"{host}:{port}".GetHashCode();

			if (channels.TryGetValue(hash, out var value)) {
				return value;
			}

			var container = new SharedChannel(host, port, hash);
			channels.TryAdd(hash, container);

			return container;
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="channel"></param>
		public static void Release(SharedChannel channel)
		{
			if (channel == null) {
				return;
			}

			int hash = channel.hash;

			if (channels.TryGetValue(hash, out _)) {
				channel.ShutdownAsync();
			}
		}
	}

	/// <summary>
	/// <see href="https://grpc.github.io/grpc/csharp/api/Grpc.Core.Channel.html" />
	/// </summary>
	public partial class SharedChannel
	{
		//
		public Channel Channel => channel;

		//
		Channel channel = null!;
		int hash;
		int connection;

		//
		public event EventHandler<ChannelState> onStateChanged = null!;


		/// <summary>
		/// .
		/// </summary>
		private SharedChannel() { }

		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="hash"></param>
		public SharedChannel(string host, int port, int hash)
		{
			this.channel = new Channel(host, port, ChannelCredentials.Insecure);
			this.hash = hash;
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
