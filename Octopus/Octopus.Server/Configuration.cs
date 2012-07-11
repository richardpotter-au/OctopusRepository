#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: Configuration.cs
#endregion

using System.Collections.Specialized;
using System.Configuration;

namespace Octopus.Server
{
	/// <summary>
	/// Configuration for the Quartz server.
	/// </summary>
	public class Configuration
	{
		private const string PrefixServerConfiguration = "octopus.server";
		private const string KeyServiceName = PrefixServerConfiguration + ".serviceName";
		private const string KeyServiceDisplayName = PrefixServerConfiguration + ".serviceDisplayName";
		private const string KeyServiceDescription = PrefixServerConfiguration + ".serviceDescription";
		private const string KeyServerImplementationType = PrefixServerConfiguration + ".type";

		private const string DefaultServiceName = "OctopusServer";
		private const string DefaultServiceDisplayName = "Octopus Server";
		private const string DefaultServiceDescription = "Octopus Job Scheduling Server";
		private static readonly string DefaultServerImplementationType = typeof(OctopusServer).AssemblyQualifiedName;

		private static readonly NameValueCollection configuration;

		/// <summary>
		/// Initializes the <see cref="Configuration"/> class.
		/// </summary>
		static Configuration()
		{
			configuration = (NameValueCollection)ConfigurationManager.GetSection("quartz");
		}

		/// <summary>
		/// Gets the name of the service.
		/// </summary>
		/// <value>The name of the service.</value>
		public static string ServiceName
		{
			get { return GetConfigurationOrDefault(KeyServiceName, DefaultServiceName); }
		}

		/// <summary>
		/// Gets the display name of the service.
		/// </summary>
		/// <value>The display name of the service.</value>
		public static string ServiceDisplayName
		{
			get { return GetConfigurationOrDefault(KeyServiceDisplayName, DefaultServiceDisplayName); }
		}

		/// <summary>
		/// Gets the service description.
		/// </summary>
		/// <value>The service description.</value>
		public static string ServiceDescription
		{
			get { return GetConfigurationOrDefault(KeyServiceDescription, DefaultServiceDescription); }
		}

		/// <summary>
		/// Gets the type name of the server implementation.
		/// </summary>
		/// <value>The type of the server implementation.</value>
		public static string ServerImplementationType
		{
			get { return GetConfigurationOrDefault(KeyServerImplementationType, DefaultServerImplementationType); }
		}

		/// <summary>
		/// Returns configuration value with given key. If configuration
		/// for the does not exists, return the default value.
		/// </summary>
		/// <param name="configurationKey">Key to read configuration with.</param>
		/// <param name="defaultValue">Default value to return if configuration is not found</param>
		/// <returns>The configuration value.</returns>
		private static string GetConfigurationOrDefault(string configurationKey, string defaultValue)
		{
			string retValue = null;
			if (configuration != null)
			{
				retValue = configuration[configurationKey];
			}

			if (retValue == null || retValue.Trim().Length == 0)
			{
				retValue = defaultValue;
			}
			return retValue;
		}
	}
}
