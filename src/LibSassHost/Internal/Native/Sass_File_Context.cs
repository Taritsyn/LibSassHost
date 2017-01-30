using System;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Struct for file compilation
	/// </summary>
	internal struct Sass_File_Context
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Sass_File_Context"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Sass_File_Context(IntPtr reference)
		{
			Reference = reference;
		}
	}
}