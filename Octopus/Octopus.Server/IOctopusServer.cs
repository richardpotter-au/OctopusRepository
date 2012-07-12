#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'change made to branch 12
// branch 1
// branch 2
// Filename: IOctopusServer.cs
#endregion

namespace Octopus.Server
{
	/// <summary>
	/// Service interface for core Quartz.NET server.
	/// </summary>
	public interface IOctopusServer
	{
		/// <summary>
		/// Initializes the instance of <see cref="IOctopusServer"/>.
		/// Initialization will only be called once in server's lifetime.
		/// </summary>
		void Initialize();

		/// <summary>
		/// Starts this instance.
		/// </summary>
		void Start();

		/// <summary>
		/// Stops this instance.
		/// </summary>
		void Stop();

		/// <summary>
		/// Pauses all activity in scheudler.
		/// </summary>
		void Pause();

		/// <summary>
		/// Resumes all acitivity in server.
		/// </summary>
		void Resume();
	}
}