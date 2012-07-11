#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: OctopusServer.cs
#endregion

using System;
using System.Threading;
using NLog;
using Quartz;
using Quartz.Impl;

namespace Octopus.Server
{
	/// <summary>
	/// The main server logic.
	/// </summary>
	public class OctopusServer : IOctopusServer
	{
		private static readonly Logger _log = LogManager.GetLogger("Octopus.Server.OctopusServer");
		private ISchedulerFactory schedulerFactory;
		private IScheduler scheduler;

		/// <summary>
		/// Initializes a new instance of the <see cref="OctopusServer"/> class.
		/// </summary>
		public OctopusServer()
		{
		}

		/// <summary>
		/// Initializes the instance of the <see cref="OctopusServer"/> class.
		/// </summary>
		public virtual void Initialize()
		{
			try
			{
				schedulerFactory = CreateSchedulerFactory();
				scheduler = GetScheduler();
			}
			catch (Exception e)
			{
				_log.Error("Server initialization failed:" + e.Message, e);
				throw;
			}
		}

		/// <summary>
		/// Gets the scheduler with which this server should operate with.
		/// </summary>
		/// <returns></returns>
		protected virtual IScheduler GetScheduler()
		{
			return schedulerFactory.GetScheduler();
		}

		/// <summary>
		/// Returns the current scheduler instance (usually created in <see cref="Initialize" />
		/// using the <see cref="GetScheduler" /> method).
		/// </summary>
		protected virtual IScheduler Scheduler
		{
			get { return scheduler; }
		}

		/// <summary>
		/// Creates the scheduler factory that will be the factory
		/// for all schedulers on this instance.
		/// </summary>
		/// <returns></returns>
		protected virtual ISchedulerFactory CreateSchedulerFactory()
		{
			return new StdSchedulerFactory();
		}

		/// <summary>
		/// Starts this instance, delegates to scheduler.
		/// </summary>
		public virtual void Start()
		{
			scheduler.Start();

			try
			{
				Thread.Sleep(3000);
			}
			catch (ThreadInterruptedException)
			{
			}

			_log.Info("Scheduler started successfully");
		}

		/// <summary>
		/// Stops this instance, delegates to scheduler.
		/// </summary>
		public virtual void Stop()
		{
			scheduler.Shutdown(true);
			_log.Info("Scheduler shutdown complete");
		}

		/// <summary>
		/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		/// </summary>
		public virtual void Dispose()
		{
			// no-op for now
		}

		/// <summary>
		/// Pauses all activity in scheudler.
		/// </summary>
		public virtual void Pause()
		{
			scheduler.PauseAll();
		}

		/// <summary>
		/// Resumes all acitivity in server.
		/// </summary>
		public void Resume()
		{
			scheduler.ResumeAll();
		}
	}
}
