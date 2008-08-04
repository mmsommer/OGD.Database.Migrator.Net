#region License
//The contents of this file are subject to the Mozilla Public License
//Version 1.1 (the "License"); you may not use this file except in
//compliance with the License. You may obtain a copy of the License at
//http://www.mozilla.org/MPL/
//Software distributed under the License is distributed on an "AS IS"
//basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
//License for the specific language governing rights and limitations
//under the License.
#endregion

using System;
using System.Collections.Generic;
using NAnt.Core;
using Migrator.Framework;

namespace Migrator.NAnt.Loggers
{
	/// <summary>
	/// NAnt task logger for the migration mediator
	/// </summary>
	public class TaskLogger : ILogger
	{
		private int _widthFirstColumn = 5;
		private Task _task;
		
		public TaskLogger(Task task)
		{
			_task = task;
		}
		
		protected void LogInfo(string format, params object[] args)
		{
			_task.Log(Level.Info, format, args);
		}
		
		protected void LogError(string format, params object[] args)
		{
			_task.Log(Level.Error, format, args);
		}
		
		public void Started(long currentVersion, long finalVersion)
		{
			LogInfo("Current version : {0}", currentVersion);
		}
		
		public void Started(List<long> currentVersions, long finalVersion)
		{
			LogInfo("Latest version applied : {0}.  Target version : {1}", LatestVersion(currentVersions), finalVersion);
		}

		public void MigrateUp(long version, string migrationName)
		{
			LogInfo("Applying {0}: {1}", version.ToString().PadLeft(_widthFirstColumn), migrationName);
		}

		public void MigrateDown(long version, string migrationName)
		{
			LogInfo("Removing {0}: {1}", version.ToString().PadLeft(_widthFirstColumn), migrationName);
		}
		
		public void Skipping(long version)
		{
			MigrateUp(version, "<Migration not found>");
		}
		
		public void RollingBack(long originalVersion)
		{
			LogInfo("Rolling back to migration {0}", originalVersion);
		}
		
		public void Exception(long version, string migrationName, Exception ex)
		{
			LogInfo("{0} Error in migration {1} : {2}", "".PadLeft(_widthFirstColumn), version, ex.Message);
			
			LogError(ex.Message);
			LogError(ex.StackTrace);
			Exception iex = ex.InnerException;
			while (ex.InnerException != null)
			{
				LogError("Caused by: {0}", ex.InnerException);
				LogError(ex.InnerException.StackTrace);
				iex = iex.InnerException;
			}
		}
		
		public void Finished(long originalVersion, long currentVersion)
		{
			LogInfo("Migrated to version {0}", currentVersion);
		}
		
		public void Finished(List<long> originalVersion, long currentVersion)
		{
			LogInfo("Migrated to version {0}", currentVersion);
		}
		
		public void Log(string format, params object[] args)
		{
			LogInfo("{0} {1}", "".PadLeft(_widthFirstColumn), String.Format(format, args));
		}
		
		public void Warn(string format, params object[] args)
		{
			LogInfo("{0} [Warning] {1}", "".PadLeft(_widthFirstColumn), String.Format(format, args));
		}		
		
		public void Trace(string format, params object[] args)
		{
			_task.Log(Level.Debug, "{0} {1}", "".PadLeft(_widthFirstColumn), String.Format(format, args));
		}	
		
		private string LatestVersion(List<long> versions){
			if(versions.Count > 0)
			{
				return versions[versions.Count - 1].ToString();
			}
			return "No migrations applied yet!";
		}
	}
}
