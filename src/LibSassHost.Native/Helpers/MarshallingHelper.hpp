#include <sass.h>

using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		namespace Helpers
		{
			ref class MarshallingHelper
			{
				public:

#pragma region String

					static char* MarshalString(String^ value);
					static String^ UnmarshalString(char* value);

#pragma endregion

#pragma region ConstString

					static const char* MarshalConstString(String^ value);
					static String^ UnmarshalConstString(const char* value);

#pragma endregion

#pragma region StringArray

					static char** MarshalStringArray(array<String^>^ items);
					static array<String^>^ UnmarshalStringArray(char** items);

#pragma endregion

				private:
					static char* custom_strdup(const char* value);
			};
		}
	}
}