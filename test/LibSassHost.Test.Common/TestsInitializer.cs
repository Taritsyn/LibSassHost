#if NETCOREAPP
using System.Text;

namespace LibSassHost.Test.Common
{
	internal static class TestsInitializer
	{
		private static readonly object _synchronizer = new object();
		private static bool _initialized;

		public static void Initialize()
		{
			if (!_initialized)
			{
				lock (_synchronizer)
				{
					if (!_initialized)
					{
						Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
						_initialized = true;
					}
				}
			}
		}
	}
}
#endif