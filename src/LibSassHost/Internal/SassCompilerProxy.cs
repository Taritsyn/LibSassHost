using System;
using System.Reflection;

using LibSassHost.Internal.Native;

namespace LibSassHost.Internal
{
	internal static class SassCompilerProxy
	{
		public static string GetVersion()
		{
			return Sass_Api.libsass_version();
		}

		public static string GetLanguageVersion()
		{
			return Sass_Api.libsass_language_version();
		}

		public static void Compile(SassDataContext dataContext)
		{
			Sass_Data_Context data_ctx = Sass_Api.sass_make_data_context(dataContext.SourceString);
			Sass_Options data_ctx_options = Sass_Api.sass_data_context_get_options(data_ctx);

			FillUnmanagedContextOptions(ref data_ctx_options, dataContext);

			try
			{
				// Compile Sass-code by using context
				int result = Sass_Api.sass_compile_data_context(data_ctx);

				// Copy resulting fields from unmanaged object to managed
				Sass_Context base_ctx = Sass_Api.sass_data_context_get_context(data_ctx);
				if (result == 0)
				{
					FillManagedContextOutput(dataContext, ref base_ctx);
				}
				else
				{
					FillManagedContextError(dataContext, ref base_ctx);
				}
			}
			finally
			{
				// Free resources
				Sass_Api.sass_delete_data_context(data_ctx);
			}
		}

		public static void CompileFile(SassFileContext fileContext)
		{
			Sass_File_Context file_ctx = Sass_Api.sass_make_file_context(fileContext.InputPath);
			Sass_Options file_ctx_options = Sass_Api.sass_file_context_get_options(file_ctx);

			FillUnmanagedContextOptions(ref file_ctx_options, fileContext);

			try
			{
				// Compile Sass-file by using context
				int result = Sass_Api.sass_compile_file_context(file_ctx);

				// Copy resulting fields from unmanaged object to managed
				Sass_Context base_ctx = Sass_Api.sass_file_context_get_context(file_ctx);
				if (result == 0)
				{
					FillManagedContextOutput(fileContext, ref base_ctx);
				}
				else
				{
					FillManagedContextError(fileContext, ref base_ctx);
				}
			}
			finally
			{
				// Free resources
				Sass_Api.sass_delete_file_context(file_ctx);
			}
		}

		private static void FillUnmanagedContextOptions(ref Sass_Options ctx_options, SassContextBase context)
		{
			SassOptions options = context.Options;

			foreach (string importExtension in options.AdditionalImportExtensions)
			{
				Sass_Api.sass_option_push_import_extension(ctx_options, importExtension);
			}

			Sass_Api.sass_option_set_include_path(ctx_options, options.IncludePath);
			Sass_Api.sass_option_set_indent(ctx_options, options.Indent);
			Sass_Api.sass_option_set_input_path(ctx_options, context.InputPath);
			Sass_Api.sass_option_set_is_indented_syntax_src(ctx_options, context.IsIndentedSyntaxSource);
			Sass_Api.sass_option_set_linefeed(ctx_options, options.LineFeed);
			Sass_Api.sass_option_set_omit_source_map_url(ctx_options, options.OmitSourceMapUrl);
			Sass_Api.sass_option_set_output_path(ctx_options, context.OutputPath);
			Sass_Api.sass_option_set_output_style(ctx_options, (Sass_Output_Style)options.OutputStyle);
			Sass_Api.sass_option_set_precision(ctx_options, options.Precision);
			Sass_Api.sass_option_set_source_comments(ctx_options, options.SourceComments);
			Sass_Api.sass_option_set_source_map_contents(ctx_options, options.SourceMapContents);
			Sass_Api.sass_option_set_source_map_embed(ctx_options, options.SourceMapEmbed);
			Sass_Api.sass_option_set_source_map_file(ctx_options, context.SourceMapFile);
			Sass_Api.sass_option_set_source_map_file_urls(ctx_options, options.SourceMapFileUrls);
			Sass_Api.sass_option_set_source_map_root(ctx_options, options.SourceMapRoot);
		}

		private static void FillManagedContextOutput(SassContextBase context, ref Sass_Context ctx)
		{
			context.OutputString = Sass_Api.sass_context_get_output_string(ctx);
			context.SourceMapString = Sass_Api.sass_context_get_source_map_string(ctx);
			context.IncludedFiles = Sass_Api.sass_context_get_included_files(ctx);
		}

		private static void FillManagedContextError(SassContextBase context, ref Sass_Context ctx)
		{
			var error = new SassErrorInfo
			{
				Status = Sass_Api.sass_context_get_error_status(ctx),
				Text = Sass_Api.sass_context_get_error_text(ctx),
				Message = Sass_Api.sass_context_get_error_message(ctx),
				File = Sass_Api.sass_context_get_error_file(ctx),
				Line = Sass_Api.sass_context_get_error_line(ctx),
				Column = Sass_Api.sass_context_get_error_column(ctx),
				Source = Sass_Api.sass_context_get_error_src(ctx)
			};

			context.Error = error;
		}
	}
}