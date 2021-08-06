#ifndef SASS_FILE_MANAGER_H
#define SASS_FILE_MANAGER_H

// sass.hpp must go before all system headers to get the
// __EXTENSIONS__ fix on Solaris.
#include "sass.hpp"

#include <string>
#include "file_manager.h"

namespace Sass
{
  class File_Manager
  {
    private:
      File_Manager();
      File_Manager(File_Manager const&);
      void operator=(File_Manager const&);

    public:
      bool is_initialized;
      bool supports_conversion_to_absolute_path;
      Func_String get_current_directory_delegate;
      Func_String_Boolean file_exists_delegate;
      Func_String_Boolean is_absolute_path_delegate;
      Func_String_String to_absolute_path_delegate;
      Func_String_String read_file_delegate;

      static File_Manager& get_instance();

      sass::string get_current_directory();
      bool file_exists(const sass::string& path);
      bool is_absolute_path(const sass::string& path);
      sass::string to_absolute_path(const sass::string& path);
      char* read_file(const sass::string& path);
  };
}

#endif
