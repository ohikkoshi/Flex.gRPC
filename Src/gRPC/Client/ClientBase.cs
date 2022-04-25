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

	/// <summary>
	/// <see href="https://grpc.github.io/grpc/csharp/api/Grpc.Core.Channel.html" />
	/// </summary>
	public class ClientBase
	{
		//
		public Channel Channel => channel.Channel;
		public SharedChannel SharedChannel => channel;

		//
		SharedChannel channel;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		public ClientBase(string host, int port) => channel = SharedChannel.Instantiate(host, port);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		~ClientBase() => SharedChannel.Release(channel);

		/// <summary>
		/// .
		/// </summary>
		public void Start() => StartAsync().ConfigureAwait(false);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public Task StartAsync() => channel?.StartAsync() ?? Task.CompletedTask;

		/// <summary>
		/// .
		/// </summary>
		public void Shutdown() => ShutdownAsync().ConfigureAwait(false);

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public Task ShutdownAsync() => channel?.ShutdownAsync() ?? Task.CompletedTask;
	}
}
#pragma warning restore 8632
