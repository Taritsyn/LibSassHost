using System;
#if NET45 || NET471 || NETSTANDARD || NETCOREAPP2_1
using System.Buffers;
#endif
using System.Runtime.InteropServices;
using System.Text;
#if NET40

using PolyfillsForOldDotNet.System.Buffers;
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

			IntPtr ptr;
			int valueLength = value.Length;

			if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && valueLength > 0)
			{
				// Convert Unicode to ANSI
				Encoding srcEncoding = Encoding.UTF8;
#if NETFULL
				Encoding dstEncoding = Encoding.Default;
#else
				Encoding dstEncoding = Encoding.GetEncoding(0);
#endif

				var byteArrayPool = ArrayPool<byte>.Shared;
				int bufferLength = srcEncoding.GetByteCount(value);
				byte[] buffer = byteArrayPool.Rent(bufferLength + 1);
				buffer[bufferLength] = 0;

				try
				{
					fixed (char* pValue = value)
					fixed (byte* pBuffer = buffer)
					{
						srcEncoding.GetBytes(pValue, valueLength, pBuffer, bufferLength);
					}

					int resultCharCount = dstEncoding.GetCharCount(buffer, 0, bufferLength);
					var charArrayPool = ArrayPool<char>.Shared;
					char[] resultChars = charArrayPool.Rent(resultCharCount + 1);
					resultChars[resultCharCount] = '\0';

					try
					{
						fixed (byte* pBuffer = buffer)
						fixed (char* pResultChars = resultChars)
						{
							dstEncoding.GetChars(pBuffer, bufferLength, pResultChars, resultCharCount);
							ptr = CharsToPtr(dstEncoding, pResultChars, resultCharCount);
						}
					}
					finally
					{
						charArrayPool.Return(resultChars);
					}
				}
				finally
				{
					byteArrayPool.Return(buffer);
				}
			}
			else
			{
				fixed (char* pValue = value)
				{
					ptr = CharsToPtr(Encoding.UTF8, pValue, valueLength);
				}
			}

			return ptr;
		}

		private static unsafe IntPtr CharsToPtr(Encoding encoding, char* chars, int count)
		{
			int bufferLength = encoding.GetByteCount(chars, count);
			IntPtr ptr = Sass_Api.sass_alloc_memory(bufferLength + 1);
			var pBuffer = (byte*)ptr;

			encoding.GetBytes(chars, count, pBuffer, bufferLength);
			pBuffer[bufferLength] = 0;

			return ptr;
		}

		public static unsafe string PtrToString(IntPtr ptr)
		{
			if (ptr == IntPtr.Zero)
			{
				return null;
			}

			string result;

			var pTempBuffer = (byte*)ptr;
			while (*pTempBuffer != 0)
			{
				pTempBuffer++;
			}

			var pBuffer = (byte*)ptr;
			int bufferLength = (int)(pTempBuffer - pBuffer);

#if NET471 || NETSTANDARD || NETCOREAPP2_1
			result = Encoding.UTF8.GetString(pBuffer, bufferLength);
#else
			var byteArrayPool = ArrayPool<byte>.Shared;
			byte[] buffer = byteArrayPool.Rent(bufferLength + 1);
			buffer[bufferLength] = 0;

			try
			{
				Marshal.Copy(ptr, buffer, 0, bufferLength);
				result = Encoding.UTF8.GetString(buffer, 0, bufferLength);
			}
			finally
			{
				byteArrayPool.Return(buffer);
			}
#endif

			return result;
		}

		public static string[] PtrToStringArray(IntPtr ptr, int len)
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