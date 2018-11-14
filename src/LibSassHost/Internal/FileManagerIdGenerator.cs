#if SUPPORT_FILE_MANAGER_ID_GENERATION
using System;
using System.Runtime.Serialization;

namespace LibSassHost.Internal
{
	/// <summary>
	/// ID generator for instances of the file managers
	/// </summary>
	internal static class FileManagerIDGenerator
	{
		/// <summary>
		/// Instance of the <see cref="ObjectIDGenerator"/> class
		/// </summary>
		private static ObjectIDGenerator _objIdGenerator = new ObjectIDGenerator();


		/// <summary>
		/// Generates an ID for instance of the file manager
		/// </summary>
		/// <param name="fileManager">Instance of the file manager</param>
		/// <returns>ID for instance of the file manager</returns>
		public static int GenerateID(IFileManager fileManager)
		{
			if (fileManager == null)
			{
				return 0;
			}

			int appDomainId = AppDomain.CurrentDomain.Id;
			bool firstTime;
			long longObjId = _objIdGenerator.GetId(fileManager, out firstTime);
			short objId = Convert.ToInt16(longObjId);

			int fileManagerId = (appDomainId << 15) + objId;

			return fileManagerId;
		}
	}
}
#endif