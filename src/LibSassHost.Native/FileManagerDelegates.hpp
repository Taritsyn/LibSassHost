using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		public ref class FileManagerDelegates
		{
			public:
				IntPtr^ GetCurrentDirectoryDelegate;
				IntPtr^ FileExistsDelegate;
				IntPtr^ IsAbsolutePathDelegate;
				IntPtr^ ToAbsolutePathDelegate;
				IntPtr^ ReadFileDelegate;
		};
	}
}