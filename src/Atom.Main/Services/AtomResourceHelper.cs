using System;
using System.Reflection;
using System.Resources;
using log4net;

namespace Atom.Main.Services
{
	public static class AtomResourceHelper
	{
		private static ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		public static string ReadResourceValue(Type resourceType, string key)
		{
			//value for our return value
			var resourceValue = string.Empty;
			try
			{
				// get the path of your file
				string filePath = AppDomain.CurrentDomain.BaseDirectory;
				// create a resource manager for reading from
				//the resx file
				ResourceManager resourceManager = new ResourceManager(resourceType);
				// retrieve the value of the specified key
				resourceValue = resourceManager.GetString(key);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				throw;
			}
			return resourceValue;
		}
	}
}
