using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
#if NET40

using PolyfillsForOldDotNet.System.Runtime.InteropServices;
#endif

namespace LibSassHost.Helpers
{
	/// <summary>
	/// Marshalling helpers
	/// </summary>
	internal static class MarshallingHelpers
	{
		public static IntPtr StringToPtr(string value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}

			string processedValue;
			if (value.Length > 0)
			{
				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
				{
					byte[] bytes = Encoding.UTF8.GetBytes(value);
					processedValue = Encoding.GetEncoding(0).GetString(bytes);
				}
				else
				{
					processedValue = value;
				}
			}
			else
			{
				processedValue = string.Empty;
			}

			IntPtr ptr = Marshal.StringToHGlobalAnsi(processedValue);

			return ptr;
		}

		public static string PtrToString(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}

			string result;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			{
				var bytes = new List<byte>();
				int byteIndex = 0;
				byte byteValue;

				while ((byteValue = Marshal.ReadByte(ptr, byteIndex)) != 0)
				{
					bytes.Add(byteValue);
					byteIndex++;
				}

				result = Encoding.UTF8.GetString(bytes.ToArray());
			}
			else
			{
				result = Marshal.PtrToStringAnsi(ptr);
			}

			return result;
		}

		public static string[] PtrToStringArray(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return new string[0];
			}

			var items = new List<string>();
			int itemCount = 0;
			int offset = 0;
			IntPtr itemPtr;

			while ((itemPtr = Marshal.ReadIntPtr(ptr, offset)) != IntPtr.Zero)
			{
				string itemValue = PtrToString(itemPtr);
				items.Add(itemValue);

				itemCount++;
				offset = itemCount * IntPtr.Size;
			}

			return items.ToArray();
		}
	}
}