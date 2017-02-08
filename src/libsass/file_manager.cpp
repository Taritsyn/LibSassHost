#include "file_manager.hpp"
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

#ifndef __GNUC__
#pragma region get_current_directory
#endif

  void File_Manager::set_get_current_directory_delegate(Func_String del)
  {
    _get_current_directory_delegate = del;
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

#ifdef _WIN32
    const wchar_t* wcurrent_directory_name = _get_current_directory_delegate();
    std::string current_directory_name = UTF_8::convert_from_utf16(wcurrent_directory_name);
#else
    std::string current_directory_name = _get_current_directory_delegate();
#endif

    return current_directory_name;
  }

#ifndef __GNUC__
#pragma endregion
#endif

#ifndef __GNUC__
#pragma region file_exists
#endif

  void File_Manager::set_file_exists_delegate(Func_String_Boolean del)
  {
    _file_exists_delegate = del;
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

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    bool result = _file_exists_delegate(wpath.c_str());
#else
    bool result = _file_exists_delegate(path.c_str());
#endif

    return result;
  }

#ifndef __GNUC__
#pragma endregion
#endif

#ifndef __GNUC__
#pragma region is_absolute_path
#endif

  void File_Manager::set_is_absolute_path_delegate(Func_String_Boolean del)
  {
    _is_absolute_path_delegate = del;
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

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    bool result = _is_absolute_path_delegate(wpath.c_str());
#else
    bool result = _is_absolute_path_delegate(path.c_str());
#endif

    return result;
  }

#ifndef __GNUC__
#pragma endregion
#endif

#ifndef __GNUC__
#pragma region to_absolute_path
#endif

  void File_Manager::set_to_absolute_path_delegate(Func_String_String del)
  {
    _to_absolute_path_delegate = del;
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

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    std::string absolute_path = UTF_8::convert_from_utf16(_to_absolute_path_delegate(wpath.c_str()));
#else
    std::string absolute_path = _to_absolute_path_delegate(path.c_str());
#endif

    return absolute_path;
  }

#ifndef __GNUC__
#pragma endregion
#endif

#ifndef __GNUC__
#pragma region read_file
#endif

  void File_Manager::set_read_file_delegate(Func_String_String del)
  {
    _read_file_delegate = del;
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

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    const wchar_t* wcontent = _read_file_delegate(wpath.c_str());
    std::string content = UTF_8::convert_from_utf16(std::wstring(wcontent));
#else
    std::string content = _read_file_delegate(path.c_str());
#endif

    return sass_copy_c_string(content.c_str());
  }

#ifndef __GNUC__
#pragma endregion
#endif

}

extern "C" {
  using namespace Sass;

  void ADDCALL sass_file_manager_set_get_current_directory_delegate (Func_String del)
  {
    File_Manager::get_instance().set_get_current_directory_delegate(del);
  }

  void ADDCALL sass_file_manager_set_file_exists_delegate (Func_String_Boolean del)
  {
    File_Manager::get_instance().set_file_exists_delegate(del);
  }

  void ADDCALL sass_file_manager_set_is_absolute_path_delegate (Func_String_Boolean del)
  {
    File_Manager::get_instance().set_is_absolute_path_delegate(del);
  }

  void ADDCALL sass_file_manager_set_to_absolute_path_delegate (Func_String_String del)
  {
    File_Manager::get_instance().set_to_absolute_path_delegate(del);
  }

  void ADDCALL sass_file_manager_set_read_file_delegate(Func_String_String del)
  {
    File_Manager::get_instance().set_read_file_delegate(del);
  }


  void ADDCALL sass_file_manager_unset_get_current_directory_delegate ()
  {
    File_Manager::get_instance().unset_get_current_directory_delegate();
  }

  void ADDCALL sass_file_manager_unset_file_exists_delegate()
  {
    File_Manager::get_instance().unset_file_exists_delegate();
  }

  void ADDCALL sass_file_manager_unset_is_absolute_path_delegate()
  {
    File_Manager::get_instance().unset_is_absolute_path_delegate();
  }

  void ADDCALL sass_file_manager_unset_to_absolute_path_delegate()
  {
    File_Manager::get_instance().unset_to_absolute_path_delegate();
  }

  void ADDCALL sass_file_manager_unset_read_file_delegate()
  {
    File_Manager::get_instance().unset_read_file_delegate();
  }
}