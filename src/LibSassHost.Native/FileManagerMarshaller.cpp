#include <file_manager.h>

#include "FileManagerMarshaller.hpp"

using namespace System::Runtime::InteropServices;

using namespace Sass;

namespace LibSassHost
{
	namespace Native
	{
		void FileManagerMarshaller::SetDelegates(FileManagerDelegates^ delegates)
		{
			File_Manager& file_manager = File_Manager::get_instance();

			file_manager.set_get_current_directory_delegate(
				(Func_String)delegates->GetCurrentDirectoryDelegate->ToPointer());
			file_manager.set_file_exists_delegate(
				(Func_String_Boolean)delegates->FileExistsDelegate->ToPointer());
			file_manager.set_is_absolute_path_delegate(
				(Func_String_Boolean)delegates->IsAbsolutePathDelegate->ToPointer());
			file_manager.set_read_file_delegate(
				(Func_String_String)delegates->ReadFileDelegate->ToPointer());
		}

		void FileManagerMarshaller::UnsetDelegates()
		{
			File_Manager& file_manager = File_Manager::get_instance();

			file_manager.unset_get_current_directory_delegate();
			file_manager.unset_file_exists_delegate();
			file_manager.unset_is_absolute_path_delegate();
			file_manager.unset_read_file_delegate();
		}
	}
}