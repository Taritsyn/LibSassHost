#ifndef SASS_FILE_MANAGER_H
#define SASS_FILE_MANAGER_H

#include <string>
#include "file_manager.h"

namespace Sass
{
  class File_Manager
  {
    private:
      Func_String _get_current_directory_delegate;
      Func_String_Boolean _file_exists_delegate;
      Func_String_Boolean _is_absolute_path_delegate;
      Func_String_String _to_absolute_path_delegate;
      Func_String_String _read_file_delegate;

      File_Manager();
      File_Manager(File_Manager const&);
      void operator=(File_Manager const&);

    public:
      static File_Manager& get_instance();

#pragma region get_current_directory

      void set_get_current_directory_delegate(Func_String del);
      void unset_get_current_directory_delegate();
      std::string get_current_directory();

#pragma endregion

#pragma region file_exists

      void set_file_exists_delegate(Func_String_Boolean del);
      void unset_file_exists_delegate();
      bool file_exists(const std::string& path);

#pragma endregion

#pragma region is_absolute_path

      void set_is_absolute_path_delegate(Func_String_Boolean del);
      void unset_is_absolute_path_delegate();
      bool is_absolute_path(const std::string& path);

#pragma endregion

#pragma region to_absolute_path

      void set_to_absolute_path_delegate(Func_String_String del);
      void unset_to_absolute_path_delegate();
      std::string to_absolute_path(const std::string& path);

#pragma endregion

#pragma region read_file

      void set_read_file_delegate(Func_String_String del);
      void unset_read_file_delegate();
      char* read_file(const std::string& path);

#pragma endregion

  };
}

#endif