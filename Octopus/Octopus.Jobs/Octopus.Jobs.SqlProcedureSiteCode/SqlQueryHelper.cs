#region Copyright Australian Software Engineering Pty. Ltd. 2012
// All rights are reserved. Reproduction and transmission, in whole or in part, in any form or by any means,
// electronic, mechanical or otherwise, is prohibited without the prior written consent of the copyright owner.'
//
// Filename: SqlCommandExtensions.cs
#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace Octopus.Jobs
{
	public static class SqlCommandExtensions
	{
		/// <summary>
		/// creates a string from the command for logging
		/// works with both text commands and stored procedure commands
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		public static string GetQueryString(this SqlCommand cmd)
		{
			switch (cmd.CommandType)
			{
				case CommandType.StoredProcedure:
					return Environment.NewLine + GetCommandStoredProcedureQueryString(cmd);
				case CommandType.TableDirect:
					throw new NotImplementedException();
				case CommandType.Text:
					return Environment.NewLine + GetCommandTextQueryString(cmd);
				default:
					Debug.Assert(false, "unexpected SqlCommandType");
					return string.Empty;
			}

		}

		#region Helpers
		private static string GetCommandTextQueryString(SqlCommand cmd)
		{
			string query = cmd.CommandText;
			foreach (SqlParameter prm in cmd.Parameters)
			{
				switch (prm.SqlDbType)
				{
					case SqlDbType.Bit:
						int boolToInt = (bool)prm.Value ? 1 : 0;
						query = query.Replace(prm.ParameterName, string.Format("{0}", (bool)prm.Value ? 1 : 0));
						break;
					case SqlDbType.Int:
						query = query.Replace(prm.ParameterName, string.Format("{0}", prm.Value));
						break;
					case SqlDbType.VarChar:
						query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
						break;
					default:
						query = query.Replace(prm.ParameterName, string.Format("'{0}'", prm.Value));
						break;
				}
			}
			return query;
		}

		private static string GetCommandStoredProcedureQueryString(SqlCommand cmd)
		{
			var queryParameters = new List<string>();
			foreach (SqlParameter prm in cmd.Parameters)
			{
				string parameterText = string.Format("{0}='{1}'", prm.ParameterName, GetParameterValueString(prm.Value));
				queryParameters.Add(parameterText);
			}
			return cmd.CommandText + " " + string.Join(", ", queryParameters.ToArray());
		}

		private static string GetParameterValueString(object parameterValue)
		{
			if (parameterValue == null) return "null";

			// note if the parameter is a table
			// this code only outputs the first column data
			// there is usually only one column
			DataTable table = parameterValue as DataTable;
			if (table != null)
			{
				var rowData = new List<string>();
				foreach (DataRow row in table.Rows)
				{
					rowData.Add(row[0].ToString());
				}
				return string.Join(", ", rowData.ToArray());
			}

			return parameterValue.ToString();

		}
		#endregion
	}
}
