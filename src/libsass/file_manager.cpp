#include "file_manager.h"
#include "utf8_string.hpp"
#include "util.hpp"

namespace Sass
{
	File_Manager::File_Manager()
	{
		_get_current_directory_delegate = NULL;
		_file_exists_delegate = NULL;
		_is_absolute_path_delegate = NULL;
		_get_directory_name_delegate = NULL;
		_get_file_name_delegate = NULL;
		_get_canonical_path_delegate = NULL;
		_combine_paths_delegate = NULL;
		_to_absolute_path_delegate = NULL;
		_make_relative_path_delegate = NULL;
		_read_file_delegate = NULL;
	}


	File_Manager& File_Manager::get_instance()
	{
		static File_Manager _instance;

		return _instance;
	}

#pragma region get_current_directory

	void File_Manager::set_get_current_directory_delegate(Func_String delegate)
	{
		_get_current_directory_delegate = delegate;
	}

	void File_Manager::unset_get_current_directory_delegate()
	{
		_get_current_directory_delegate = NULL;
	}

	std::string File_Manager::get_current_directory()
	{
		if (_get_current_directory_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'get_current_directory' method is null.");
		}

		const wchar_t* wcurrent_directory_name = _get_current_directory_delegate();
		std::string current_directory_name = UTF_8::convert_from_utf16(wcurrent_directory_name);

		return current_directory_name;
	}

#pragma endregion

#pragma region file_exists

	void File_Manager::set_file_exists_delegate(Func_String_Boolean delegate)
	{
		_file_exists_delegate = delegate;
	}

	void File_Manager::unset_file_exists_delegate()
	{
		_file_exists_delegate = NULL;
	}

	bool File_Manager::file_exists(const std::string& path)
	{
		if (_file_exists_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'file_exists' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		bool result = _file_exists_delegate(wpath.c_str());

		return result;
	}

#pragma endregion

#pragma region is_absolute_path

	void File_Manager::set_is_absolute_path_delegate(Func_String_Boolean delegate)
	{
		_is_absolute_path_delegate = delegate;
	}

	void File_Manager::unset_is_absolute_path_delegate()
	{
		_is_absolute_path_delegate = NULL;
	}

	bool File_Manager::is_absolute_path(const std::string& path)
	{
		if (_is_absolute_path_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'is_absolute_path' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		bool result = _is_absolute_path_delegate(wpath.c_str());

		return result;
	}

#pragma endregion

#pragma region get_directory_name

	void File_Manager::set_get_directory_name_delegate(Func_String_String delegate)
	{
		_get_directory_name_delegate = delegate;
	}

	void File_Manager::unset_get_directory_name_delegate()
	{
		_get_directory_name_delegate = NULL;
	}

	std::string File_Manager::get_directory_name(const std::string& path)
	{
		if (_get_directory_name_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'get_directory_name' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		const wchar_t* wdirectory_name = _get_directory_name_delegate(wpath.c_str());
		std::string directory_name = UTF_8::convert_from_utf16(std::wstring(wdirectory_name));

		return directory_name;
	}

#pragma endregion

#pragma region get_file_name

	void File_Manager::set_get_file_name_delegate(Func_String_String delegate)
	{
		_get_file_name_delegate = delegate;
	}

	void File_Manager::unset_get_file_name_delegate()
	{
		_get_file_name_delegate = NULL;
	}

	std::string File_Manager::get_file_name(const std::string& path)
	{
		if (_get_file_name_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'get_file_name' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		const wchar_t* wfile_name = _get_file_name_delegate(wpath.c_str());
		std::string file_name = UTF_8::convert_from_utf16(std::wstring(wfile_name));

		return file_name;
	}

#pragma endregion

#pragma region get_canonical_path

	void File_Manager::set_get_canonical_path_delegate(Func_String_String delegate)
	{
		_get_canonical_path_delegate = delegate;
	}

	void File_Manager::unset_get_canonical_path_delegate()
	{
		_get_canonical_path_delegate = NULL;
	}

	std::string File_Manager::get_canonical_path(std::string path)
	{
		if (_get_canonical_path_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'get_canonical_path' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		const wchar_t* wcanonical_path = _get_canonical_path_delegate(wpath.c_str());
		std::string canonical_path = UTF_8::convert_from_utf16(std::wstring(wcanonical_path));

		return canonical_path;
	}

#pragma endregion

#pragma region combine_paths

	void File_Manager::set_combine_paths_delegate(Func_String_String_String delegate)
	{
		_combine_paths_delegate = delegate;
	}

	void File_Manager::unset_combine_paths_delegate()
	{
		_combine_paths_delegate = NULL;
	}

	std::string File_Manager::combine_paths(std::string base_path, std::string relative_path)
	{
		if (_combine_paths_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'combine_paths' method is null.");
		}

		std::wstring wbase_path = UTF_8::convert_to_utf16(base_path);
		std::wstring wrelative_path = UTF_8::convert_to_utf16(relative_path);

		const wchar_t* wcombined_path = _combine_paths_delegate(wbase_path.c_str(), wrelative_path.c_str());
		std::string combined_path = UTF_8::convert_from_utf16(std::wstring(wcombined_path));

		return combined_path;
	}

#pragma endregion

#pragma region to_absolute_path

	void File_Manager::set_to_absolute_path_delegate(Func_String_String_String delegate)
	{
		_to_absolute_path_delegate = delegate;
	}

	void File_Manager::unset_to_absolute_path_delegate()
	{
		_to_absolute_path_delegate = NULL;
	}

	std::string File_Manager::to_absolute_path(const std::string& relative_path, const std::string& current_directory_path)
	{
		if (_to_absolute_path_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'to_absolute_path' method is null.");
		}

		std::wstring wrelative_path = UTF_8::convert_to_utf16(relative_path);
		std::wstring wcurrent_directory_path = UTF_8::convert_to_utf16(current_directory_path);

		const wchar_t* wabsolute_path = _to_absolute_path_delegate(wrelative_path.c_str(), wcurrent_directory_path.c_str());
		std::string absolute_path = UTF_8::convert_from_utf16(std::wstring(wabsolute_path));

		return absolute_path;
	}

#pragma endregion

#pragma region make_relative_path

	void File_Manager::set_make_relative_path_delegate(Func_String_String_String_String delegate)
	{
		_make_relative_path_delegate = delegate;
	}

	void File_Manager::unset_make_relative_path_delegate()
	{
		_make_relative_path_delegate = NULL;
	}

	std::string File_Manager::make_relative_path(const std::string& from_path, const std::string& to_path, const std::string& current_directory_path)
	{
		if (_make_relative_path_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'make_relative_path' method is null.");
		}

		std::wstring wfrom_path = UTF_8::convert_to_utf16(from_path);
		std::wstring wto_path = UTF_8::convert_to_utf16(to_path);
		std::wstring wcurrent_directory_path = UTF_8::convert_to_utf16(current_directory_path);

		const wchar_t* wrelative_path = _make_relative_path_delegate(wfrom_path.c_str(), wto_path.c_str(), wcurrent_directory_path.c_str());
		std::string relative_path = UTF_8::convert_from_utf16(std::wstring(wrelative_path));

		return relative_path;
	}

#pragma endregion

#pragma region read_file

	void File_Manager::set_read_file_delegate(Func_String_String delegate)
	{
		_read_file_delegate = delegate;
	}

	void File_Manager::unset_read_file_delegate()
	{
		_read_file_delegate = NULL;
	}

	char* File_Manager::read_file(const std::string& path)
	{
		if (_read_file_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'read_file' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		const wchar_t* wcontent = _read_file_delegate(wpath.c_str());
		std::string content = UTF_8::convert_from_utf16(std::wstring(wcontent));

		return sass_strdup(content.c_str());
	}

#pragma endregion

}