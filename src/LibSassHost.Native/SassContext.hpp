#include "SassErrorInfo.hpp"
#include "SassOptions.hpp"

using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		public ref class SassContext
		{
			public:
				property SassErrorInfo^ Error;
				property array<String^>^ IncludedFiles;
				property String^ InputPath;
				property SassOptions^ Options;
				property String^ OutputPath;
				property String^ OutputString;
				property String^ SourceMapString;
		};

		public ref class SassDataContext : SassContext
		{
			public:
				property String^ SourceString;
		};

		public ref class SassFileContext : SassContext
		{ };
	}
}