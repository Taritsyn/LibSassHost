#ifndef SASS_FILE_MANAGER_H
#define SASS_FILE_MANAGER_H

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

      std::string get_current_directory();
      bool file_exists(const std::string& path);
      bool is_absolute_path(const std::string& path);
      std::string to_absolute_path(const std::string& path);
      char* read_file(const std::string& path);
  };
}

#endif