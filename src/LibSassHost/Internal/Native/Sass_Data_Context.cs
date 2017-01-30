using System;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Struct for data compilation
	/// </summary>
	internal struct Sass_Data_Context
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Sass_Data_Context"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Sass_Data_Context(IntPtr reference)
		{
			Reference = reference;
		}
	}
}