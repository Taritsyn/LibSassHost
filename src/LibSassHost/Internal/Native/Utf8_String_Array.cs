using System;

using LibSassHost.Helpers;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Represents array of a UTF-8 strings
	/// </summary>
	internal struct Utf8_String_Array
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Utf8_String_Array"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Utf8_String_Array(IntPtr reference)
		{
			Reference = reference;
		}


		public static implicit operator string[](Utf8_String_Array value)
		{
			return MarshallingHelpers.PtrToStringArray(value.Reference);
		}
	}
}