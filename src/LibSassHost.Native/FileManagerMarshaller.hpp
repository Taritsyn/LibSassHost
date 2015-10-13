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
				static GCHandle _get_directory_name_handle;
				static GCHandle _get_file_name_handle;
				static GCHandle _get_canonical_path_handle;
				static GCHandle _combine_paths_handle;
				static GCHandle _to_absolute_path_handle;
				static GCHandle _make_relative_path_handle;
				static GCHandle _read_file_handle;

			public:
				static void SetDelegates(FileManagerDelegates^ delegates);
				static void UnsetDelegates();
		};
	}
}