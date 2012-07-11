#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: SqlProcedureSiteCodeJob.cs
#endregion

using System;
using System.Data;
using System.Data.SqlClient;
using NLog;
using Quartz;

namespace Octopus.Jobs
{
	public class SqlProcedureSiteCodeJob : IJob
	{
		private static readonly Logger _log = LogManager.GetLogger("Octopus.Jobs.SqlProcedureSiteCodeJob");

		public void Execute(IJobExecutionContext context)
		{
			string description = context.JobDetail.Description;
			_log.Debug("job with description '{0}' launched", description);

			JobDataMap dataMap = context.JobDetail.JobDataMap;
			string connectionString = dataMap.GetString("connectionString");
			string storedProcedureName = dataMap.GetString("storedProcedureName");
			string siteCode = dataMap.GetString("siteCode");

			try
			{
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					using (var cm = connection.CreateCommand())
					{
						cm.CommandType = CommandType.StoredProcedure;
						cm.CommandText = storedProcedureName;
						cm.Parameters.AddWithValue("@varSiteInternalCode", siteCode);
						_log.Debug(cm.GetQueryString());

						int result = cm.ExecuteNonQuery();
						_log.Debug("result '{0}'", result);
					}
				}
			}
			catch (SqlException sqlEx)
			{
				_log.ErrorException("SQL Exception", sqlEx);
			}
			catch (Exception ex)
			{
				_log.ErrorException("Exception", ex);
			}
		}
	}
}
