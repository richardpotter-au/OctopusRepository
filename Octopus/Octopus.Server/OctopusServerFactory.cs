#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: OctopusServerFactory.cs
#endregion

using System;
using NLog;

namespace Octopus.Server
{
	/// <summary>
	/// Factory class to create Quartz server implementations from.
	/// </summary>
	public class OctopusServerFactory
	{
		private static readonly Logger _log = LogManager.GetLogger("Octopus.Server.OctopusServerFactory");

		/// <summary>
		/// Creates a new instance of an Quartz.NET server core.
		/// </summary>
		/// <returns></returns>
		public static IOctopusServer CreateServer()
		{
			string typeName = Configuration.ServerImplementationType;

			Type t = Type.GetType(typeName, true);

			_log.Debug("Creating new instance of server type '" + typeName + "'");
			IOctopusServer retValue = (IOctopusServer)Activator.CreateInstance(t);
			_log.Debug("Instance successfully created");
			return retValue;
		}
	}
}