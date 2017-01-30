using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

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

			string encodedValue;
			if (value.Length > 0)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(value);
				encodedValue = Encoding.GetEncoding(0).GetString(bytes);
			}
			else
			{
				encodedValue = string.Empty;
			}

			IntPtr ptr = Marshal.StringToHGlobalAnsi(encodedValue);

			return ptr;
		}

		public static string PtrToString(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}

			var bytes = new List<byte>();
			int byteIndex = 0;
			byte byteValue;

			while ((byteValue = Marshal.ReadByte(ptr, byteIndex)) != 0)
			{
				bytes.Add(byteValue);
				byteIndex++;
			}

			return Encoding.UTF8.GetString(bytes.ToArray());
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