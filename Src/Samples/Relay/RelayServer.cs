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

	public class RelayServer : ServerBase
	{
		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public RelayServer(string host = "127.0.0.1", int port = 30000) : base(host, port)
		{
			BindService(Relay.BindService(new RelayService()));
		}
	}

	class RelayService : Relay.RelayBase
	{
		//
		ConcurrentDictionary<int, IServerStreamWriter<RelayReply>> users = new ConcurrentDictionary<int, IServerStreamWriter<RelayReply>>();


		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="res"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task Join(RelayRequest req, IServerStreamWriter<RelayReply> res, ServerCallContext context)
		{
			int key = context.Peer.GetHashCode();

			if (!users.ContainsKey(key)) {
				if (users.TryAdd(key, res)) {
					Console.WriteLine($"Join {context.Peer.ToString()}");
				}

				context.CancellationToken.Register(() => {
					users.TryRemove(key, out var _);
				});
			}

			await Task.CompletedTask;
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Empty> Leave(Empty _, ServerCallContext context)
		{
			int key = context.Peer.GetHashCode();

			if (users.TryRemove(key, out var _)) {
				Console.WriteLine($"Leave {context.Peer.ToString()}");
			}

			return Task.FromResult(new Empty());
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Empty> UniCast(CastRequest req, ServerCallContext context)
		{
#if false
			int key = context.Peer.GetHashCode();
			int id = req.Id;

			var reply = new RelayReply() {
				Data = req.Data,
			};

			if (users.ContainsKey(id)) {
				var user = users[id];

				if (!user.WriteAsync(reply)) {
					// TODO::
				}
			}
#endif
			return Task.FromResult(new Empty());
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Empty> MultiCast(CastRequest req, ServerCallContext context)
		{
#if false
			int key = context.Peer.GetHashCode();
			int id = req.Id;

			var reply = new RelayReply() {
				Data = req.Data,
			};

			var array = users.Where(x => x.Key != id && x.Value.RoomId == id).Select(x => x.Value);

			foreach (var user in array) {
				if (!user.WriteAsync(reply)) {
					// TODO::
				}
			}
#endif
			return Task.FromResult(new Empty());
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Empty> BroadCast(CastRequest req, ServerCallContext context)
		{
#if false
			int key = context.Peer.GetHashCode();
			int id = req.Id;

			var reply = new RelayReply() {
				Data = req.Data,
			};

			var array = users.Where(x => x.Key != id).Select(x => x.Value);

			foreach (var user in array) {
				if (!user.WriteAsync(reply)) {
					// TODO::
				}
			}
#endif
			return Task.FromResult(new Empty());
		}
	}
}
