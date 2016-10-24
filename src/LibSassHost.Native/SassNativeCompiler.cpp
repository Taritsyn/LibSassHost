#include <exception>
#include <string>

#include "Helpers/MarshallingHelper.hpp"
#include "SassNativeCompiler.hpp"

using namespace std;

using namespace LibSassHost::Native::Helpers;

namespace LibSassHost
{
	namespace Native
	{
		void SassNativeCompiler::Compile(SassDataContext^ dataContext)
		{
			int result = -1;

			struct Sass_Data_Context* data_ctx = sass_make_data_context(
				MarshallingHelper::MarshalString(dataContext->SourceString));
			struct Sass_Options* data_ctx_options = sass_data_context_get_options(data_ctx);

			FillUnmanagedContextOptions(data_ctx_options, dataContext);

			try
			{
				// Compile Sass-code by using context
				result = sass_compile_data_context(data_ctx);

				// Copy resulting fields from unmanaged object to managed
				struct Sass_Context* base_ctx = sass_data_context_get_context(data_ctx);
				if (result == 0)
				{
					FillManagedContextOutput(dataContext, base_ctx);
				}
				else
				{
					FillManagedContextError(dataContext, base_ctx);
				}
			}
			catch (exception& e)
			{
				throw gcnew Exception(MarshallingHelper::UnmarshalConstString(e.what()));
			}
			catch (...)
			{
				throw gcnew Exception("Unhandled exception in native code");
			}
			finally
			{
				// Free resources
				sass_delete_data_context(data_ctx);
			}
		}

		void SassNativeCompiler::CompileFile(SassFileContext^ fileContext)
		{
			int result = -1;

			struct Sass_File_Context* file_ctx = sass_make_file_context(
				MarshallingHelper::MarshalConstString(fileContext->InputPath));
			struct Sass_Options* file_ctx_options = sass_file_context_get_options(file_ctx);

			FillUnmanagedContextOptions(file_ctx_options, fileContext);

			try
			{
				// Compile Sass-file by using context
				result = sass_compile_file_context(file_ctx);

				// Copy resulting fields from unmanaged object to managed
				struct Sass_Context* base_ctx = sass_file_context_get_context(file_ctx);
				if (result == 0)
				{
					FillManagedContextOutput(fileContext, base_ctx);
				}
				else
				{
					FillManagedContextError(fileContext, base_ctx);
				}
			}
			catch (exception& e)
			{
				throw gcnew Exception(MarshallingHelper::UnmarshalConstString(e.what()));
			}
			catch (...)
			{
				throw gcnew Exception("Unhandled exception in native code");
			}
			finally
			{
				// Free resources
				sass_delete_file_context(file_ctx);
			}
		}

		Sass_Output_Style SassNativeCompiler::ConvertOutputStyleEnumValueToConstant(int outputStyle)
		{
			Sass_Output_Style constant;

			switch (outputStyle)
			{
				case 0:
					constant = SASS_STYLE_NESTED;
					break;
				case 1:
					constant = SASS_STYLE_EXPANDED;
					break;
				case 2:
					constant = SASS_STYLE_COMPACT;
					break;
				case 3:
					constant = SASS_STYLE_COMPRESSED;
					break;
				default:
					throw gcnew NotSupportedException("Conversion failed.");
			}

			return constant;
		}

		void SassNativeCompiler::FillUnmanagedContextOptions(struct Sass_Options* ctx_options, SassContext^ context)
		{
			SassOptions^ options = context->Options;
			sass_option_set_include_path(ctx_options, MarshallingHelper::MarshalString(options->IncludePath));
			sass_option_set_indent(ctx_options, MarshallingHelper::MarshalConstString(options->Indent));
			sass_option_set_input_path(ctx_options, MarshallingHelper::MarshalString(context->InputPath));
			sass_option_set_is_indented_syntax_src(ctx_options, options->IsIndentedSyntaxSource);
			sass_option_set_linefeed(ctx_options, MarshallingHelper::MarshalConstString(options->LineFeed));
			sass_option_set_omit_source_map_url(ctx_options, options->OmitSourceMapUrl);
			sass_option_set_output_path(ctx_options, MarshallingHelper::MarshalString(context->OutputPath));
			sass_option_set_output_style(ctx_options, ConvertOutputStyleEnumValueToConstant(options->OutputStyle));
			sass_option_set_precision(ctx_options, options->Precision);
			sass_option_set_source_comments(ctx_options, options->SourceComments);
			sass_option_set_source_map_contents(ctx_options, options->SourceMapContents);
			sass_option_set_source_map_embed(ctx_options, options->SourceMapEmbed);
			sass_option_set_source_map_file(ctx_options, MarshallingHelper::MarshalString(options->SourceMapFile));
			sass_option_set_source_map_file_urls(ctx_options, options->SourceMapFileUrls);
			sass_option_set_source_map_root(ctx_options, MarshallingHelper::MarshalString(options->SourceMapRoot));
		}

		void SassNativeCompiler::FillManagedContextOutput(SassContext^ context, struct Sass_Context* ctx)
		{
			context->OutputString = MarshallingHelper::UnmarshalConstString(sass_context_get_output_string(ctx));
			context->SourceMapString = MarshallingHelper::UnmarshalConstString(sass_context_get_source_map_string(ctx));
			context->IncludedFiles = MarshallingHelper::UnmarshalStringArray(sass_context_get_included_files(ctx));
		}

		void SassNativeCompiler::FillManagedContextError(SassContext^ context, struct Sass_Context* ctx)
		{
			SassErrorInfo^ error = gcnew SassErrorInfo();
			error->Status = sass_context_get_error_status(ctx);
			error->Text = MarshallingHelper::UnmarshalConstString(sass_context_get_error_text(ctx));
			error->Message = MarshallingHelper::UnmarshalConstString(sass_context_get_error_message(ctx));
			error->File = MarshallingHelper::UnmarshalConstString(sass_context_get_error_file(ctx));
			error->Line = sass_context_get_error_line(ctx);
			error->Column = sass_context_get_error_column(ctx);
			error->Source = MarshallingHelper::UnmarshalConstString(sass_context_get_error_src(ctx));

			context->Error = error;
		}
	}
}