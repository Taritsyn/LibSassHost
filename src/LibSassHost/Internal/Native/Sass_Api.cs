using System.Runtime.InteropServices;

namespace LibSassHost.Internal.Native
{
	/// <summary>
	/// Sass API
	/// </summary>
	internal static class Sass_Api
	{
		const string DllName = "libsass";

		#region Version information

		[DllImport(DllName)]
		internal static extern Utf8_String libsass_version();

		[DllImport(DllName)]
		internal static extern Utf8_String libsass_language_version();

		#endregion

		#region Create and initialize a specific context

		[DllImport(DllName)]
		internal static extern Sass_File_Context sass_make_file_context(Utf8_String input_path);

		[DllImport(DllName)]
		internal static extern Sass_Data_Context sass_make_data_context(Utf8_String source_string);

		#endregion

		#region Call the compilation step for the specific context

		[DllImport(DllName)]
		internal static extern int sass_compile_file_context(Sass_File_Context ctx);

		[DllImport(DllName)]
		internal static extern int sass_compile_data_context(Sass_Data_Context ctx);

		#endregion

		#region Release all memory allocated and also ourself

		[DllImport(DllName)]
		internal static extern void sass_delete_file_context(Sass_File_Context ctx);

		[DllImport(DllName)]
		internal static extern void sass_delete_data_context(Sass_Data_Context ctx);

		#endregion

		#region Getters for context from specific implementation

		[DllImport(DllName)]
		internal static extern Sass_Context sass_file_context_get_context(Sass_File_Context file_ctx);

		[DllImport(DllName)]
		internal static extern Sass_Context sass_data_context_get_context(Sass_Data_Context data_ctx);

		#endregion

		#region Getters for Sass_Options from Sass_Context

		[DllImport(DllName)]
		internal static extern Sass_Options sass_file_context_get_options(Sass_File_Context file_ctx);

		[DllImport(DllName)]
		internal static extern Sass_Options sass_data_context_get_options(Sass_Data_Context data_ctx);

		#endregion

		#region Setters for Sass_Options values

		[DllImport(DllName)]
		internal static extern void sass_option_set_precision(Sass_Options options, int precision);

		[DllImport(DllName)]
		internal static extern void sass_option_set_output_style(Sass_Options options,
			Sass_Output_Style output_style);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_comments(Sass_Options options, bool source_comments);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_map_embed(Sass_Options options, bool source_map_embed);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_map_contents(Sass_Options options,
			bool source_map_contents);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_map_file_urls(Sass_Options options,
			bool source_map_file_urls);

		[DllImport(DllName)]
		internal static extern void sass_option_set_omit_source_map_url(Sass_Options options,
			bool omit_source_map_url);

		[DllImport(DllName)]
		internal static extern void sass_option_set_is_indented_syntax_src(Sass_Options options,
			bool is_indented_syntax_src);

		[DllImport(DllName)]
		internal static extern void sass_option_set_indent(Sass_Options options, Utf8_String indent);

		[DllImport(DllName)]
		internal static extern void sass_option_set_linefeed(Sass_Options options, Utf8_String linefeed);

		[DllImport(DllName)]
		internal static extern void sass_option_set_input_path(Sass_Options options, Utf8_String input_path);

		[DllImport(DllName)]
		internal static extern void sass_option_set_output_path(Sass_Options options, Utf8_String output_path);

		[DllImport(DllName)]
		internal static extern void sass_option_set_include_path(Sass_Options options, Utf8_String include_path);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_map_file(Sass_Options options,
			Utf8_String source_map_file);

		[DllImport(DllName)]
		internal static extern void sass_option_set_source_map_root(Sass_Options options,
			Utf8_String source_map_root);

		#endregion

		#region Getters for Sass_Context values

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_output_string(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern int sass_context_get_error_status(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_error_text(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_error_message(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_error_file(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_error_src(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern size_t sass_context_get_error_line(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern size_t sass_context_get_error_column(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String sass_context_get_source_map_string(Sass_Context ctx);

		[DllImport(DllName)]
		internal static extern Utf8_String_Array sass_context_get_included_files(Sass_Context ctx);

		#endregion

		#region Getters for File_Manager properties

		[DllImport(DllName)]
		internal static extern bool sass_file_manager_get_is_initialized();

		[DllImport(DllName)]
		internal static extern bool sass_file_manager_get_supports_conversion_to_absolute_path();

		#endregion

		#region Setters for File_Manager properties

		[DllImport(DllName)]
		internal static extern void sass_file_manager_set_is_initialized(bool is_initialized);

		[DllImport(DllName)]
		internal static extern void sass_file_manager_set_supports_conversion_to_absolute_path(
			bool supports_conversion_to_absolute_path);


		[DllImport(DllName, EntryPoint = "sass_file_manager_set_get_current_directory_delegate", CharSet = CharSet.Ansi)]
		internal static extern void sass_file_manager_set_get_current_directory_delegate_utf8(Func_StringAnsi del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_file_exists_delegate", CharSet = CharSet.Ansi)]
		internal static extern void sass_file_manager_set_file_exists_delegate_utf8(Func_StringAnsi_Boolean del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_is_absolute_path_delegate", CharSet = CharSet.Ansi)]
		internal static extern void sass_file_manager_set_is_absolute_path_delegate_utf8(Func_StringAnsi_Boolean del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_to_absolute_path_delegate", CharSet = CharSet.Ansi)]
		internal static extern void sass_file_manager_set_to_absolute_path_delegate_utf8(Func_StringAnsi_StringAnsi del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_read_file_delegate", CharSet = CharSet.Ansi)]
		internal static extern void sass_file_manager_set_read_file_delegate_utf8(Func_StringAnsi_StringAnsi del);


		[DllImport(DllName, EntryPoint = "sass_file_manager_set_get_current_directory_delegate", CharSet = CharSet.Unicode)]
		internal static extern void sass_file_manager_set_get_current_directory_delegate_utf16(Func_StringUni del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_file_exists_delegate", CharSet = CharSet.Unicode)]
		internal static extern void sass_file_manager_set_file_exists_delegate_utf16(Func_StringUni_Boolean del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_is_absolute_path_delegate", CharSet = CharSet.Unicode)]
		internal static extern void sass_file_manager_set_is_absolute_path_delegate_utf16(Func_StringUni_Boolean del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_to_absolute_path_delegate", CharSet = CharSet.Unicode)]
		internal static extern void sass_file_manager_set_to_absolute_path_delegate_utf16(Func_StringUni_StringUni del);

		[DllImport(DllName, EntryPoint = "sass_file_manager_set_read_file_delegate", CharSet = CharSet.Unicode)]
		internal static extern void sass_file_manager_set_read_file_delegate_utf16(Func_StringUni_StringUni del);

		#endregion

		#region Unsetters for File_Manager properties

		[DllImport(DllName)]
		internal static extern void sass_file_manager_unset_get_current_directory_delegate();

		[DllImport(DllName)]
		internal static extern void sass_file_manager_unset_file_exists_delegate();

		[DllImport(DllName)]
		internal static extern void sass_file_manager_unset_is_absolute_path_delegate();

		[DllImport(DllName)]
		internal static extern void sass_file_manager_unset_to_absolute_path_delegate();

		[DllImport(DllName)]
		internal static extern void sass_file_manager_unset_read_file_delegate();

		#endregion
	}
}