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
				IntPtr^ GetDirectoryNameDelegate;
				IntPtr^ GetFileNameDelegate;
				IntPtr^ GetCanonicalPathDelegate;
				IntPtr^ CombinePathsDelegate;
				IntPtr^ ToAbsolutePathDelegate;
				IntPtr^ MakeRelativePathDelegate;
				IntPtr^ ReadFileDelegate;
		};
	}
}