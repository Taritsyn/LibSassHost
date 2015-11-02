#include <string>

namespace Sass
{
	typedef bool(__stdcall *Func_String_Boolean)(const wchar_t* p);
	typedef const wchar_t*(__stdcall *Func_String)();
	typedef const wchar_t*(__stdcall *Func_String_String)(const wchar_t* p);
	typedef const wchar_t*(__stdcall *Func_String_String_String)(const wchar_t* p1, const wchar_t* p2);
	typedef const wchar_t*(__stdcall *Func_String_String_String_String)(const wchar_t* p1, const wchar_t* p2, const wchar_t* p3);

	class File_Manager
	{
		private:
			Func_String _get_current_directory_delegate;
			Func_String_Boolean _file_exists_delegate;
			Func_String_Boolean _is_absolute_path_delegate;
			Func_String_String _get_directory_name_delegate;
			Func_String_String _get_file_name_delegate;
			Func_String_String _get_canonical_path_delegate;
			Func_String_String_String _combine_paths_delegate;
			Func_String_String_String _to_absolute_path_delegate;
			Func_String_String_String_String _make_relative_path_delegate;
			Func_String_String _read_file_delegate;

			File_Manager();
			File_Manager(File_Manager const&);
			void operator=(File_Manager const&);

		public:
			static File_Manager& get_instance();

#pragma region get_current_directory

			void set_get_current_directory_delegate(Func_String delegate);
			void unset_get_current_directory_delegate();
			std::string get_current_directory();

#pragma endregion

#pragma region file_exists

			void set_file_exists_delegate(Func_String_Boolean delegate);
			void unset_file_exists_delegate();
			bool file_exists(const std::string& path);

#pragma endregion

#pragma region is_absolute_path

			void set_is_absolute_path_delegate(Func_String_Boolean delegate);
			void unset_is_absolute_path_delegate();
			bool is_absolute_path(const std::string& path);

#pragma endregion

#pragma region get_directory_name

			void set_get_directory_name_delegate(Func_String_String delegate);
			void unset_get_directory_name_delegate();
			std::string get_directory_name(const std::string& path);

#pragma endregion

#pragma region get_file_name

			void set_get_file_name_delegate(Func_String_String delegate);
			void unset_get_file_name_delegate();
			std::string get_file_name(const std::string& path);

#pragma endregion

#pragma region get_canonical_path

			void set_get_canonical_path_delegate(Func_String_String delegate);
			void unset_get_canonical_path_delegate();
			std::string get_canonical_path(std::string path);

#pragma endregion

#pragma region combine_paths

			void set_combine_paths_delegate(Func_String_String_String delegate);
			void unset_combine_paths_delegate();
			std::string combine_paths(std::string base_path, std::string relative_path);

#pragma endregion

#pragma region to_absolute_path

			void set_to_absolute_path_delegate(Func_String_String_String delegate);
			void unset_to_absolute_path_delegate();
			std::string to_absolute_path(const std::string& relative_path, const std::string& current_directory_path);

#pragma endregion

#pragma region make_relative_path

			void set_make_relative_path_delegate(Func_String_String_String_String delegate);
			void unset_make_relative_path_delegate();
			std::string make_relative_path(const std::string& from_path, const std::string& to_path, const std::string& current_directory_path);

#pragma endregion

#pragma region read_file

			void set_read_file_delegate(Func_String_String delegate);
			void unset_read_file_delegate();
			char* read_file(const std::string& path);

#pragma endregion

	};
}