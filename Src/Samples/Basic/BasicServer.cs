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

	public class BasicServer : ServerBase
	{
		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public BasicServer(string host = "127.0.0.1", int port = 30000) : base(host, port)
		{
			BindService(Basic.BindService(new BasicService()));
		}
	}

	class BasicService : Basic.BasicBase
	{
		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Reply> Unary(Request req, ServerCallContext context)
		{
			Console.WriteLine($"[{context.Host}][{context.Peer}][{context.Method}]");

			return Task.FromResult(new Reply {
				String = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
			});
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task<Reply> ClientStream(IAsyncStreamReader<Request> req, ServerCallContext context)
		{
			Console.WriteLine($"[{context.Host}][{context.Peer}][{context.Method}]");

			while (await req.MoveNext()) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var stream = req.Current;
			}

			return new Reply();
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="res"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task ServerStream(Request req, IServerStreamWriter<Reply> res, ServerCallContext context)
		{
			Console.WriteLine($"[{context.Host}][{context.Peer}][{context.Method}]");

			for (int i = 0; i < 10; i++) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var reply = new Reply() {
					Result = Reply.Types.Result.Success,
				};

				await res.WriteAsync(reply);
				await Task.Delay(TimeSpan.FromSeconds(1.0));
			}
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="req"></param>
		/// <param name="res"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task BidirectionalStream(IAsyncStreamReader<Request> req, IServerStreamWriter<Reply> res, ServerCallContext context)
		{
			Console.WriteLine($"[{context.Host}][{context.Peer}][{context.Method}]");

			while (await req.MoveNext()) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var stream = req.Current;
			}

			for (int i = 0; i < 10; i++) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var reply = new Reply() {
					Result = Reply.Types.Result.Success,
				};

				await res.WriteAsync(reply);
				await Task.Delay(TimeSpan.FromSeconds(1.0));
			}
		}
	}
}
