using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Flex.RPC
{
	using Google.Protobuf.WellKnownTypes;
	using Grpc.Core;
	using gRPC.Proto.Services;

	public class Host : ServerBase
	{
		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public Host(string host = "127.0.0.1", int port = 30000) : base(host, port)
		{
			BindService(Basic.BindService(new BasicService()));
		}
	}

	class BasicService : Basic.BasicBase
	{
		/// <summary>
		/// .
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override Task<Reply> Unary(Request request, ServerCallContext context)
		{
			Console.WriteLine($"{context.Host}:{context.Method}:{context.Peer}");

			return Task.FromResult(new Reply {
				String = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
			});
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="request"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task<Reply> ClientStream(IAsyncStreamReader<Request> request, ServerCallContext context)
		{
			Console.WriteLine($"{context.Host}:{context.Method}:{context.Peer}");

			while (await request.MoveNext()) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var stream = request.Current;
			}

			return new Reply();
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="request"></param>
		/// <param name="response"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task ServerStream(Request request, IServerStreamWriter<Reply> response, ServerCallContext context)
		{
			Console.WriteLine($"{context.Host}:{context.Method}:{context.Peer}");

			for (int i = 0; i < 10; i++) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var reply = new Reply() {
					Result = Reply.Types.Result.Success,
				};

				await response.WriteAsync(reply);
				await Task.Delay(TimeSpan.FromSeconds(1.0));
			}
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="request"></param>
		/// <param name="response"></param>
		/// <param name="context"></param>
		/// <returns></returns>
		public override async Task BidirectionalStream(IAsyncStreamReader<Request> request, IServerStreamWriter<Reply> response, ServerCallContext context)
		{
			Console.WriteLine($"{context.Host}:{context.Method}:{context.Peer}");

			while (await request.MoveNext()) {
				if (context.CancellationToken.IsCancellationRequested) {
					break;
				}

				var stream = request.Current;
			}

			for (int i = 0; i < 10; i++) {
				if (context.CancellationToken.IsCancellationRequested) {
					Console.WriteLine($"{context.Status.StatusCode.ToString()}");
					break;
				}

				var reply = new Reply() {
					Result = Reply.Types.Result.Success,
				};

				await response.WriteAsync(reply);
				await Task.Delay(TimeSpan.FromSeconds(1.0));
			}
		}
	}
}
