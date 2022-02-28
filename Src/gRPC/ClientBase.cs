using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;

	public class ClientBase
	{
		//
		public Channel Channel => channel.Channel;
		public string Host => channel.Host;
		public int Port => channel.Port;
		public int Hash => channel.Hash;

		//
		ChannelContainer channel;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		public ClientBase(string host, int port)
		{
			channel = ChannnelHelper.Instantiate(host, port);
			channel.onStateChanged += OnStateChanged;
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		~ClientBase()
		{
			channel.onStateChanged -= OnStateChanged;
			Shutdown();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="e"></param>
		public virtual void OnStateChanged(object? sender, ChannelState e)
		{
		}

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
