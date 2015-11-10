#include "FileManagerDelegates.hpp"

using namespace System::Runtime::InteropServices;

namespace LibSassHost
{
	namespace Native
	{
		public ref class FileManagerMarshaller
		{
			private:
				static GCHandle _get_current_directory_handle;
				static GCHandle _file_exists_handle;
				static GCHandle _is_absolute_path_handle;
				static GCHandle _read_file_handle;

			public:
				static void SetDelegates(FileManagerDelegates^ delegates);
				static void UnsetDelegates();
		};
	}
}