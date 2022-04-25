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

	public class BasicClient : ClientBase
	{
		//
		Basic.BasicClient client;


		/// <summary>
		/// .
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <returns></returns>
		public BasicClient(string host = "127.0.0.1", int port = 30000) : base(host, port)
		{
			client = new Basic.BasicClient(Channel);
			SharedChannel.onStateChanged += OnStateChanged;
		}

		/// <summary>
		/// .
		/// </summary>
		~BasicClient()
		{
			SharedChannel.onStateChanged -= OnStateChanged;
		}

		/// <summary>
		/// .
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void OnStateChanged(object? sender, ChannelState e)
		{
			var state = e.ToString();
			Console.WriteLine(state);
		}

		/// <summary>
		/// .
		/// </summary>
		public void Unary()
		{
			try {
				var req = new Request();
				var res = client.Unary(req);
			} catch (Exception e) {
				Console.WriteLine($"{e.TargetSite}\n{e.Message}\n{e.StackTrace}");
			}
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public async void ClientStream()
		{
			using (var req = client.ClientStream()) {
				try {
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
			}
		}

		/// <summary>
		/// .
		/// </summary>
		/// <returns></returns>
		public async void ServerStream()
		{
			var req = new Request();

			using (var res = client.ServerStream(req)) {
				try {
					while (await res.ResponseStream.MoveNext()) {
						var obj = res.ResponseStream.Current;
					}
				} catch (Exception e) {
					Console.WriteLine($"{e.TargetSite}\n{e.Message}\n{e.StackTrace}");
				}
			}
		}
	}
}
