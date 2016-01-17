#include "FileManagerDelegates.hpp"

using namespace System::Runtime::InteropServices;

namespace LibSassHost
{
	namespace Native
	{
		public ref class FileManagerMarshaller
		{
			public:
				static void SetDelegates(FileManagerDelegates^ delegates);
				static void UnsetDelegates();
		};
	}
}