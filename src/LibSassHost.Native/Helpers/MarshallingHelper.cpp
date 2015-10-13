#include <string>

#include "MarshallingHelper.hpp"

using namespace System;
using namespace System::Runtime::InteropServices;

namespace LibSassHost
{
	namespace Native
	{
		namespace Helpers
		{

#pragma region String

			char* MarshallingHelper::MarshalString(String^ value)
			{
				if (!value)
				{
					return nullptr;
				}

				char* original_str = (char*)(Marshal::StringToCoTaskMemAnsi(value)).ToPointer();
				char* target_str = (char*)malloc(strlen(original_str) + 1);
				strcpy(target_str, original_str);

				FreeString(original_str);

				return target_str;
			}

			void MarshallingHelper::FreeString(char* value)
			{
				if (value)
				{
					Marshal::FreeCoTaskMem(IntPtr((void *)value));
				}
			}

#pragma endregion

#pragma region ConstString

			const char* MarshallingHelper::MarshalConstString(String^ value)
			{
				return (const char*)Marshal::StringToCoTaskMemAnsi(value).ToPointer();
			}

			void MarshallingHelper::FreeConstString(const char* value)
			{
				if (value)
				{
					Marshal::FreeCoTaskMem(IntPtr((void *)value));
				}
			}

#pragma endregion

#pragma region StringArray

			char** MarshallingHelper::MarshalStringArray(array<String^>^ items)
			{
				int itemCount = items->Length;
				char** result = new char*[itemCount];

				for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
				{
					result[itemIndex] = (char*)Marshal::StringToHGlobalAnsi(items[itemIndex]).ToPointer();
				}

				return result;
			}

			array<String^>^ MarshallingHelper::UnmarshalStringArray(char** items)
			{
				array<String^>^ result;
				int itemCount = 0;

				if (items != NULL)
				{
					int itemIndex = 0;

					while (items[itemIndex] != NULL)
					{
						itemCount++;
						itemIndex++;
					}
				}

				result = gcnew array<String^>(itemCount);

				if (itemCount > 0)
				{
					for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
					{
						result[itemIndex] = gcnew String(items[itemIndex]);
					}
				}

				return result;
			}

			void MarshallingHelper::FreeStringArray(char** items)
			{
				if (items != NULL)
				{
					int itemIndex = 0;

					while (items[itemIndex] != NULL)
					{
						Marshal::FreeHGlobal(IntPtr((void *)items[itemIndex]));

						itemIndex++;
					}

					delete[] items;
				}
			}

#pragma endregion

		}
	}
}