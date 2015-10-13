using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		public ref class SassErrorInfo
		{
			public:
				property int Status;
				property String^ Text;
				property String^ Message;
				property String^ File;
				property int Line;
				property int Column;
				property String^ Source;
		};
	}
}