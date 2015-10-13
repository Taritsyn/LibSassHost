#include "file_manager.hpp"

using namespace std;

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

	string File_Manager::get_current_directory()
	{
		if (_get_current_directory_delegate == NULL)
		{
			throw runtime_error("The delegate for 'get_current_directory' method is null.");
		}

		return strdup(_get_current_directory_delegate());
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

	bool File_Manager::file_exists(const string& path)
	{
		if (_file_exists_delegate == NULL)
		{
			throw runtime_error("The delegate for 'file_exists' method is null.");
		}

		return _file_exists_delegate(path.c_str());
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

	bool File_Manager::is_absolute_path(const string& path)
	{
		if (_is_absolute_path_delegate == NULL)
		{
			throw runtime_error("The delegate for 'is_absolute_path' method is null.");
		}

		return _is_absolute_path_delegate(path.c_str());
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

	string File_Manager::get_directory_name(const string& path)
	{
		if (_get_directory_name_delegate == NULL)
		{
			throw runtime_error("The delegate for 'get_directory_name' method is null.");
		}

		return strdup(_get_directory_name_delegate(path.c_str()));
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

	string File_Manager::get_file_name(const string& path)
	{
		if (_get_file_name_delegate == NULL)
		{
			throw runtime_error("The delegate for 'get_file_name' method is null.");
		}

		return strdup(_get_file_name_delegate(path.c_str()));
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

	string File_Manager::get_canonical_path(string path)
	{
		if (_get_canonical_path_delegate == NULL)
		{
			throw runtime_error("The delegate for 'get_canonical_path' method is null.");
		}

		return _get_canonical_path_delegate(path.c_str());
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

	string File_Manager::combine_paths(string base_path, string relative_path)
	{
		if (_combine_paths_delegate == NULL)
		{
			throw runtime_error("The delegate for 'combine_paths' method is null.");
		}

		return _combine_paths_delegate(base_path.c_str(), relative_path.c_str());
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

	string File_Manager::to_absolute_path(const string& relative_path, const string& current_directory_path)
	{
		if (_to_absolute_path_delegate == NULL)
		{
			throw runtime_error("The delegate for 'to_absolute_path' method is null.");
		}

		return _to_absolute_path_delegate(relative_path.c_str(), current_directory_path.c_str());
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

	string File_Manager::make_relative_path(const string&  from_path, const string&  to_path, const string&  current_directory_path)
	{
		if (_make_relative_path_delegate == NULL)
		{
			throw runtime_error("The delegate for 'make_relative_path' method is null.");
		}

		return _make_relative_path_delegate(from_path.c_str(), to_path.c_str(), current_directory_path.c_str());
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

	char* File_Manager::read_file(const string& path)
	{
		if (_read_file_delegate == NULL)
		{
			throw runtime_error("The delegate for 'read_file' method is null.");
		}

		return strdup(_read_file_delegate(path.c_str()));
	}

#pragma endregion

}