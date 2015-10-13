using namespace System;

namespace LibSassHost
{
	namespace Native
	{
		namespace Helpers
		{
			static class MarshallingHelper
			{
				public:

#pragma region String

					static char* MarshalString(String^ value);
					static void FreeString(char* value);

#pragma endregion

#pragma region ConstString

					static const char* MarshalConstString(String^ value);
					static void FreeConstString(const char* value);

#pragma endregion

#pragma region StringArray

					static char** MarshalStringArray(array<String^>^ items);
					static array<String^>^ UnmarshalStringArray(char** items);
					static void FreeStringArray(char** items);

#pragma endregion

			};
		}
	}
}