#include <string>

namespace Sass
{
	typedef bool(__stdcall *Func_String_Boolean)(const wchar_t* p);
	typedef const wchar_t*(__stdcall *Func_String)();
	typedef const wchar_t*(__stdcall *Func_String_String)(const wchar_t* p);

	class File_Manager
	{
		private:
			Func_String _get_current_directory_delegate;
			Func_String_Boolean _file_exists_delegate;
			Func_String_Boolean _is_absolute_path_delegate;
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

#pragma region read_file

			void set_read_file_delegate(Func_String_String delegate);
			void unset_read_file_delegate();
			char* read_file(const std::string& path);

#pragma endregion

	};
}