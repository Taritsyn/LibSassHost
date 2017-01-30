using System.Runtime.InteropServices;

namespace LibSassHost.Internal.Native
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	[return: MarshalAs(UnmanagedType.Bool)]
	internal delegate bool Func_String_Boolean([MarshalAs(UnmanagedType.LPWStr)]string p);
}