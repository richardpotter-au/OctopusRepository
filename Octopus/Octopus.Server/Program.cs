#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: Program.cs
#endregion

using Topshelf;

namespace Octopus.Server
{
	/// <summary>
	/// The server's main entry point.
	/// </summary>
	public static class Program
	{
		/// <summary>
		/// Main.
		/// </summary>
		/// <param name="args"></param>
		public static void Main(string[] args)
		{
			Host host = HostFactory.New(x =>
			{
				x.Service<IOctopusServer>(s =>
				{
					s.SetServiceName("octopus.server");
					s.ConstructUsing(builder =>
											{
												OctopusServer server = new OctopusServer();
												server.Initialize();
												return server;
											});
					s.WhenStarted(server => server.Start());
					s.WhenPaused(server => server.Pause());
					s.WhenContinued(server => server.Resume());
					s.WhenStopped(server => server.Stop());
				});

				x.RunAsLocalSystem();

				x.SetDescription(Configuration.ServiceDescription);
				x.SetDisplayName(Configuration.ServiceDisplayName);
				x.SetServiceName(Configuration.ServiceName);
			});

			host.Run();
		}

	}
}
