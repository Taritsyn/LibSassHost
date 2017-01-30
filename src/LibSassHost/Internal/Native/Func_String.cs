using System.Runtime.InteropServices;

namespace LibSassHost.Internal.Native
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.LPWStr)]
	public delegate string Func_String();
}