#include "SassContext.hpp"

namespace LibSassHost
{
	namespace Native
	{
		public ref class SassNativeCompiler
		{
			public:
				void Compile(SassDataContext^ dataContext);
				void CompileFile(SassFileContext^ fileContext);

			private:
				static Sass_Output_Style ConvertOutputStyleEnumValueToConstant(int outputStyle);
				static void FillUnmanagedContextOptions(Sass_Options* ctx_options, SassContext^ context);
				static void FillManagedContextOutput(SassContext^ context, Sass_Context* ctx);
				static void FillManagedContextError(SassContext^ context, Sass_Context* ctx);
		};
	}
}