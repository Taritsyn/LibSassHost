using System;

using LibSassHost.Helpers;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Represents text as a series of UTF-8 characters
	/// </summary>
	internal struct Utf8_String
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Utf8_String"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Utf8_String(IntPtr reference)
		{
			Reference = reference;
		}


		public static implicit operator string(Utf8_String value)
		{
			return MarshallingHelpers.PtrToString(value.Reference);
		}

		public static implicit operator Utf8_String(string value)
		{
			return new Utf8_String(MarshallingHelpers.StringToPtr(value));
		}
	}
}