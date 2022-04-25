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
	using gRPC.Proto.Services;

	public class RelaySession
	{
		//
		public int Id { get; private set; } = 0;
		public string Name { get; private set; } = null!;
		public int RoomId { get; private set; } = 0;

		//
		IServerStreamWriter<RelayReply> stream = null!;
		ServerCallContext context = null!;


		/// <summary>
		/// .
		/// </summary>
		private RelaySession() { }

		/// <summary>
		/// .
		/// </summary>
		/// <param name="id"></param>
		/// <param name="name"></param>
		/// <param name="roomId"></param>
		/// <param name="stream"></param>
		/// <param name="context"></param>
		public RelaySession(int id, string name, int roomId, IServerStreamWriter<RelayReply> stream, ServerCallContext context)
		{
			this.Id = id;
			this.Name = name;
			this.RoomId = roomId;
			this.stream = stream;
			this.context = context;
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="reply"></param>
		/// <returns></returns>
		public bool WriteAsync(RelayReply reply)
		{
			if (context.CancellationToken.IsCancellationRequested) {
				return false;
			}

			try {
				stream.WriteAsync(reply);
			} catch (Exception) {
				return false;
			}

			return true;
		}
	}
}
