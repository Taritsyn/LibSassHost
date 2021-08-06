// sass.hpp must go before all system headers to get the
// __EXTENSIONS__ fix on Solaris.
#include "sass.hpp"

#include "file_manager.hpp"
#include "utf8_string.hpp"
#include "util.hpp"

namespace Sass
{
  File_Manager::File_Manager()
  {
    is_initialized = false;
    get_current_directory_delegate = NULL;
    file_exists_delegate = NULL;
    is_absolute_path_delegate = NULL;
    to_absolute_path_delegate = NULL;
    read_file_delegate = NULL;
  }


  File_Manager& File_Manager::get_instance()
  {
    static File_Manager _instance;

    return _instance;
  }

  sass::string File_Manager::get_current_directory()
  {
    if (get_current_directory_delegate == NULL)
    {
      throw std::runtime_error("The delegate for 'get_current_directory' method is null.");
    }

#ifdef _WIN32
    const wchar_t* wcurrent_directory_name = get_current_directory_delegate();
    sass::string current_directory_name = UTF_8::convert_from_utf16(wcurrent_directory_name);
#else
    sass::string current_directory_name = get_current_directory_delegate();
#endif

    return current_directory_name;
  }

  bool File_Manager::file_exists(const sass::string& path)
  {
    if (file_exists_delegate == NULL)
    {
      throw std::runtime_error("The delegate for 'file_exists' method is null.");
    }

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    bool result = file_exists_delegate(wpath.c_str());
#else
    bool result = file_exists_delegate(path.c_str());
#endif

    return result;
  }

  bool File_Manager::is_absolute_path(const sass::string& path)
  {
    if (is_absolute_path_delegate == NULL)
    {
      throw std::runtime_error("The delegate for 'is_absolute_path' method is null.");
    }

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    bool result = is_absolute_path_delegate(wpath.c_str());
#else
    bool result = is_absolute_path_delegate(path.c_str());
#endif

    return result;
  }

  sass::string File_Manager::to_absolute_path(const sass::string& path)
  {
    if (to_absolute_path_delegate == NULL)
    {
      throw std::runtime_error("The delegate for 'to_absolute_path' method is null.");
    }

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    sass::string absolute_path = UTF_8::convert_from_utf16(to_absolute_path_delegate(wpath.c_str()));
#else
    sass::string absolute_path = to_absolute_path_delegate(path.c_str());
#endif

    return absolute_path;
  }

  char* File_Manager::read_file(const sass::string& path)
  {
    if (read_file_delegate == NULL)
    {
      throw std::runtime_error("The delegate for 'read_file' method is null.");
    }

#ifdef _WIN32
    std::wstring wpath = UTF_8::convert_to_utf16(path);
    const wchar_t* wcontent = read_file_delegate(wpath.c_str());
    sass::string content = UTF_8::convert_from_utf16(std::wstring(wcontent));
#else
    sass::string content = read_file_delegate(path.c_str());
#endif

    return sass_copy_c_string(content.c_str());
  }
}

extern "C" {
  using namespace Sass;

  bool ADDCALL sass_file_manager_get_is_initialized ()
  {
    return File_Manager::get_instance().is_initialized;
  }

  bool ADDCALL sass_file_manager_get_supports_conversion_to_absolute_path ()
  {
    return File_Manager::get_instance().supports_conversion_to_absolute_path;
  }


  void ADDCALL sass_file_manager_set_is_initialized (bool is_initialized)
  {
    File_Manager::get_instance().is_initialized = is_initialized;
  }

  void ADDCALL sass_file_manager_set_supports_conversion_to_absolute_path (
    bool supports_conversion_to_absolute_path)
  {
    File_Manager::get_instance().supports_conversion_to_absolute_path = supports_conversion_to_absolute_path;
  }

  void ADDCALL sass_file_manager_set_get_current_directory_delegate (Func_String del)
  {
    File_Manager::get_instance().get_current_directory_delegate = del;
  }

  void ADDCALL sass_file_manager_set_file_exists_delegate (Func_String_Boolean del)
  {
    File_Manager::get_instance().file_exists_delegate = del;
  }

  void ADDCALL sass_file_manager_set_is_absolute_path_delegate (Func_String_Boolean del)
  {
    File_Manager::get_instance().is_absolute_path_delegate = del;
  }

  void ADDCALL sass_file_manager_set_to_absolute_path_delegate (Func_String_String del)
  {
    File_Manager::get_instance().to_absolute_path_delegate = del;
  }

  void ADDCALL sass_file_manager_set_read_file_delegate(Func_String_String del)
  {
    File_Manager::get_instance().read_file_delegate = del;
  }


  void ADDCALL sass_file_manager_unset_get_current_directory_delegate ()
  {
    File_Manager::get_instance().get_current_directory_delegate = NULL;
  }

  void ADDCALL sass_file_manager_unset_file_exists_delegate()
  {
    File_Manager::get_instance().file_exists_delegate = NULL;
  }

  void ADDCALL sass_file_manager_unset_is_absolute_path_delegate()
  {
    File_Manager::get_instance().is_absolute_path_delegate = NULL;
  }

  void ADDCALL sass_file_manager_unset_to_absolute_path_delegate()
  {
    File_Manager::get_instance().to_absolute_path_delegate = NULL;
  }

  void ADDCALL sass_file_manager_unset_read_file_delegate()
  {
    File_Manager::get_instance().read_file_delegate = NULL;
  }
}
