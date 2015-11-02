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

			IntPtr^ get_directory_name_delegate = delegates->GetDirectoryNameDelegate;
			_get_directory_name_handle = GCHandle::Alloc(get_directory_name_delegate, GCHandleType::Pinned);
			file_manager.set_get_directory_name_delegate(
				(Func_String_String)get_directory_name_delegate->ToPointer());

			IntPtr^ get_file_name_delegate = delegates->GetFileNameDelegate;
			_get_file_name_handle = GCHandle::Alloc(get_file_name_delegate, GCHandleType::Pinned);
			file_manager.set_get_file_name_delegate(
				(Func_String_String)get_file_name_delegate->ToPointer());

			IntPtr^ get_canonical_path_delegate = delegates->GetCanonicalPathDelegate;
			_get_canonical_path_handle = GCHandle::Alloc(get_canonical_path_delegate, GCHandleType::Pinned);
			file_manager.set_get_canonical_path_delegate(
				(Func_String_String)get_canonical_path_delegate->ToPointer());

			IntPtr^ combine_paths_delegate = delegates->CombinePathsDelegate;
			_combine_paths_handle = GCHandle::Alloc(combine_paths_delegate, GCHandleType::Pinned);
			file_manager.set_combine_paths_delegate(
				(Func_String_String_String)combine_paths_delegate->ToPointer());

			IntPtr^ to_absolute_path_delegate = delegates->ToAbsolutePathDelegate;
			_to_absolute_path_handle = GCHandle::Alloc(to_absolute_path_delegate, GCHandleType::Pinned);
			file_manager.set_to_absolute_path_delegate(
				(Func_String_String_String)to_absolute_path_delegate->ToPointer());

			IntPtr^ make_relative_path_delegate = delegates->MakeRelativePathDelegate;
			_make_relative_path_handle = GCHandle::Alloc(make_relative_path_delegate, GCHandleType::Pinned);
			file_manager.set_make_relative_path_delegate(
				(Func_String_String_String_String)make_relative_path_delegate->ToPointer());

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

			file_manager.unset_get_directory_name_delegate();
			_get_directory_name_handle.Free();

			file_manager.unset_get_file_name_delegate();
			_get_file_name_handle.Free();

			file_manager.unset_get_canonical_path_delegate();
			_get_canonical_path_handle.Free();

			file_manager.unset_combine_paths_delegate();
			_combine_paths_handle.Free();

			file_manager.unset_to_absolute_path_delegate();
			_to_absolute_path_handle.Free();

			file_manager.unset_make_relative_path_delegate();
			_make_relative_path_handle.Free();

			file_manager.unset_read_file_delegate();
			_read_file_handle.Free();
		}
	}
}