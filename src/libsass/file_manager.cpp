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
		_to_absolute_path_delegate = NULL;
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

#pragma region to_absolute_path

	void File_Manager::set_to_absolute_path_delegate(Func_String_String delegate)
	{
		_to_absolute_path_delegate = delegate;
	}

	void File_Manager::unset_to_absolute_path_delegate()
	{
		_to_absolute_path_delegate = NULL;
	}

	std::string File_Manager::to_absolute_path(const std::string& path)
	{
		if (_to_absolute_path_delegate == NULL)
		{
			throw std::runtime_error("The delegate for 'to_absolute_path' method is null.");
		}

		std::wstring wpath = UTF_8::convert_to_utf16(path);

		std::string absolute_path = UTF_8::convert_from_utf16(_to_absolute_path_delegate(wpath.c_str()));

		return absolute_path;
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

		return sass_copy_c_string(content.c_str());
	}

#pragma endregion

}