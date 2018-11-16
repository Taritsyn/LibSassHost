namespace LibSassHost.Constants
{
	/// <summary>
	/// DLL names
	/// </summary>
	internal static class DllName
	{
		public const string Universal = "libsass";
		public const string ForWindows = Universal + ".dll";
		public const string ForLinux = Universal + ".so";
		public const string ForOsx = Universal + ".dylib";
	}
}