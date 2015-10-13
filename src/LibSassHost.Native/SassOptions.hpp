using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		public ref class SassOptions
		{
			public:
				property String^ IncludePath;
				property String^ Indent;
				property bool IsIndentedSyntaxSource;
				property String^ LineFeed;
				property bool OmitSourceMapUrl;
				property int OutputStyle;
				property int Precision;
				property bool SourceComments;
				property bool SourceMapContents;
				property bool SourceMapEmbed;
				property String^ SourceMapFile;
				property String^ SourceMapRoot;
		};
	}
}