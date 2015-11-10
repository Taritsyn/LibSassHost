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

			IntPtr^ get_current_directory_delegate = delegates->GetCurrentDirectoryDelegate;
			_get_current_directory_handle = GCHandle::Alloc(get_current_directory_delegate, GCHandleType::Pinned);
			file_manager.set_get_current_directory_delegate(
				(Func_String)get_current_directory_delegate->ToPointer());

			IntPtr^ file_exists_delegate = delegates->FileExistsDelegate;
			_file_exists_handle = GCHandle::Alloc(file_exists_delegate, GCHandleType::Pinned);
			file_manager.set_file_exists_delegate(
				(Func_String_Boolean)file_exists_delegate->ToPointer());

			IntPtr^ is_absolute_path_delegate = delegates->IsAbsolutePathDelegate;
			_is_absolute_path_handle = GCHandle::Alloc(is_absolute_path_delegate, GCHandleType::Pinned);
			file_manager.set_is_absolute_path_delegate(
				(Func_String_Boolean)is_absolute_path_delegate->ToPointer());

			IntPtr^ read_file_delegate = delegates->ReadFileDelegate;
			_read_file_handle = GCHandle::Alloc(read_file_delegate, GCHandleType::Pinned);
			file_manager.set_read_file_delegate(
				(Func_String_String)read_file_delegate->ToPointer());
		}

		void FileManagerMarshaller::UnsetDelegates()
		{
			File_Manager& file_manager = File_Manager::get_instance();

			file_manager.unset_get_current_directory_delegate();
			_get_current_directory_handle.Free();

			file_manager.unset_file_exists_delegate();
			_file_exists_handle.Free();

			file_manager.unset_is_absolute_path_delegate();
			_is_absolute_path_handle.Free();

			file_manager.unset_read_file_delegate();
			_read_file_handle.Free();
		}
	}
}