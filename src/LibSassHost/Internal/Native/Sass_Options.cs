using System;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Sass config options structure
	/// </summary>
	internal struct Sass_Options
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Sass_Options"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Sass_Options(IntPtr reference)
		{
			Reference = reference;
		}
	}
}