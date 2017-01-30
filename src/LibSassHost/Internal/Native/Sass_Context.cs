using System;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Base for all contexts
	/// </summary>
	internal struct Sass_Context
	{
		/// <summary>
		/// The reference
		/// </summary>
		public readonly IntPtr Reference;


		/// <summary>
		/// Initializes a new instance of the <see cref="Sass_Context"/> struct
		/// </summary>
		/// <param name="reference">The reference</param>
		public Sass_Context(IntPtr reference)
		{
			Reference = reference;
		}
	}
}