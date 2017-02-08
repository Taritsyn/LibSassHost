using System.Runtime.InteropServices;

namespace LibSassHost.Internal.Native
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPStr)]
	public delegate string Func_StringAnsi();
}