using System.Runtime.InteropServices;

namespace LibSassHost.Internal.Native
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal delegate bool Func_StringAnsi_Boolean([MarshalAs(UnmanagedType.LPStr)]string p);
}