using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
#if NET40

using PolyfillsForOldDotNet.System.Runtime.InteropServices;
#endif

using LibSassHost.Internal.Native;

namespace LibSassHost.Helpers
{
	/// <summary>
	/// Marshalling helpers
	/// </summary>
	internal static class MarshallingHelpers
	{
		public static unsafe IntPtr StringToPtr(string value)
		{
			if (value == null)
			{
				return IntPtr.Zero;
			}

			Encoding srcEncoding = Encoding.UTF8;
			Encoding dstEncoding = srcEncoding;
			string processedValue = value;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && value.Length > 0)
			{
				byte[] bytes = srcEncoding.GetBytes(value);
#if NETFULL
				dstEncoding = Encoding.Default;
#else
				dstEncoding = Encoding.GetEncoding(0);
#endif
				processedValue = dstEncoding.GetString(bytes);
			}

#if NET45 || NET471 || NETSTANDARD
			int bufferLength = dstEncoding.GetByteCount(processedValue);
			IntPtr ptr = Sass_Api.sass_alloc_memory(bufferLength + 1);

			fixed (char* pValue = processedValue)
			{
				dstEncoding.GetBytes(pValue, processedValue.Length, (byte*)ptr, bufferLength);
			}
#else
			byte[] buffer = dstEncoding.GetBytes(processedValue);
			int bufferLength = buffer.Length;
			IntPtr ptr = Sass_Api.sass_alloc_memory(bufferLength + 1);
			Marshal.Copy(buffer, 0, ptr, bufferLength);
#endif
			((byte*)ptr)[bufferLength] = 0;

			return ptr;
		}

		public static unsafe string PtrToString(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}

			var pTempBuffer = (byte*)ptr;
			while (*pTempBuffer != 0)
			{
				pTempBuffer++;
			}

			var pBuffer = (byte*)ptr;
			int bufferLength = (int)(pTempBuffer - pBuffer);

#if NET471 || NETSTANDARD
			string result = Encoding.UTF8.GetString(pBuffer, bufferLength);
#else
			byte[] buffer = new byte[bufferLength];
			Marshal.Copy(ptr, buffer, 0, bufferLength);
			string result = Encoding.UTF8.GetString(buffer, 0, bufferLength);
#endif

			return result;
		}

		public static unsafe string[] PtrToStringArray(IntPtr ptr, int len)
		{
			if (ptr == IntPtr.Zero)
			{
				return new string[0];
			}

			var items = new string[len];

			for (int itemIndex = 0; itemIndex < len; itemIndex++)
			{
				int offset = itemIndex * IntPtr.Size;
				IntPtr itemPtr = Marshal.ReadIntPtr(ptr, offset);
				string itemValue = PtrToString(itemPtr);

				items[itemIndex] = itemValue;
			}

			return items;
		}
	}
}