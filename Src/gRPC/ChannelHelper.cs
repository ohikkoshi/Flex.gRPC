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

	public class ChannnelHelper
	{
		//
		static Dictionary<int, ChannelContainer> channels = new Dictionary<int, ChannelContainer>();


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public static ChannelContainer Instantiate(string host, int port)
		{
			int hash = $"{host}:{port}".GetHashCode();

			if (!channels.ContainsKey(hash)) {
				var container = new ChannelContainer(host, port, hash);
				channels.Add(hash, container);
			}

			return channels[hash];
		}
	}
}
#pragma warning restore 8632
