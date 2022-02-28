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

	public class Client : ClientBase
	{
		//
		Basic.BasicClient client;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public Client(string host = "127.0.0.1", int port = 30000) : base(host, port)
		{
			client = new Basic.BasicClient(Channel);
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public override void OnStateChanged(object? sender, ChannelState e)
		{
#if true
			Console.WriteLine($"{Host}:{Port} - {e.ToString()}");
#endif
		}

		/// <summary>
		/// .
		/// </summary>
		public void Unary()
		{
#if true
			try {
				var req = new Request();
				var res = client.Unary(req);
			} catch (Exception e) {
				Console.WriteLine($"{e.TargetSite}\n{e.Message}\n{e.StackTrace}");
			}
#endif
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public async void ClientStream()
		{
#if true
			try {
				var req = client.ClientStream();

				for (int i = 0; i < 10; i++) {
					Console.WriteLine($"{i}");



					var obj = new Request();
					await req.RequestStream.WriteAsync(obj);
					await Task.Delay(TimeSpan.FromSeconds(0.5));
				}

				await req.RequestStream.CompleteAsync();
			} catch (Exception e) {
				Console.WriteLine($"{e.TargetSite}\n{e.Message}\n{e.StackTrace}");
			}
#else
			await Task.CompletedTask;
#endif
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public async void ServerStream()
		{
#if true
			try {
				var req = new Request();
				var res = client.ServerStream(req);

				while (await res.ResponseStream.MoveNext()) {
					var obj = res.ResponseStream.Current;
				}
			} catch (Exception e) {
				Console.WriteLine($"{e.TargetSite}\n{e.Message}\n{e.StackTrace}");
			}
#else
			await Task.CompletedTask;
#endif
		}
	}
}
