using System;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// .NET representation of <code>size_t</code> type
	/// </summary>
	internal struct size_t
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly UIntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="size_t"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public size_t(UIntPtr reference)
		{
			Reference = reference;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="size_t"/> struct
		/// </summary>
		/// <param name="intValue">The <c>int</c> value</param>
		public size_t(int intValue)
		{
			Reference = new UIntPtr((uint)intValue);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="size_t"/> struct
		/// </summary>
		/// <param name="longValue">The <c>long</c> value</param>
		public size_t(long longValue)
		{
			Reference = new UIntPtr((ulong)longValue);
		}


		public static implicit operator int(size_t value)
		{
			return (int)value.Reference.ToUInt32();
		}

		public static implicit operator long(size_t value)
		{
			return (long)value.Reference.ToUInt64();
		}

		public static implicit operator size_t(int intValue)
		{
			return new size_t(intValue);
		}

		public static implicit operator size_t(long longValue)
		{
			return new size_t(longValue);
		}
	}
}