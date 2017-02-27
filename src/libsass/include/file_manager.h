#ifndef SASS_C_FILE_MANAGER_H
#define SASS_C_FILE_MANAGER_H

#include <sass/base.h>

#ifdef _WIN32
typedef bool(__stdcall *Func_String_Boolean)(const wchar_t* p);
typedef const wchar_t*(__stdcall *Func_String)();
typedef const wchar_t*(__stdcall *Func_String_String)(const wchar_t* p);
#else
#define __stdcall

typedef bool(__stdcall *Func_String_Boolean)(const char* p);
typedef const char*(__stdcall *Func_String)();
typedef const char*(__stdcall *Func_String_String)(const char* p);
#endif

#ifdef __cplusplus
extern "C" {
#endif

// Getters for File_Manager properties
ADDAPI bool ADDCALL sass_file_manager_get_is_initialized();

// Setters for File_Manager properties
ADDAPI void ADDCALL sass_file_manager_set_is_initialized(bool is_initialized);
ADDAPI void ADDCALL sass_file_manager_set_get_current_directory_delegate (Func_String del);
ADDAPI void ADDCALL sass_file_manager_set_file_exists_delegate (Func_String_Boolean del);
ADDAPI void ADDCALL sass_file_manager_set_is_absolute_path_delegate (Func_String_Boolean del);
ADDAPI void ADDCALL sass_file_manager_set_to_absolute_path_delegate (Func_String_String del);
ADDAPI void ADDCALL sass_file_manager_set_read_file_delegate (Func_String_String del);

// Unsetters for File_Manager properties
ADDAPI void ADDCALL sass_file_manager_unset_get_current_directory_delegate ();
ADDAPI void ADDCALL sass_file_manager_unset_file_exists_delegate ();
ADDAPI void ADDCALL sass_file_manager_unset_is_absolute_path_delegate ();
ADDAPI void ADDCALL sass_file_manager_unset_to_absolute_path_delegate ();
ADDAPI void ADDCALL sass_file_manager_unset_read_file_delegate ();

#ifdef __cplusplus
} // __cplusplus defined.
#endif

#endif