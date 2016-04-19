#include <string>

#include "MarshallingHelper.hpp"

using namespace std;

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace System::Text;

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

				array<Byte>^ bytes = Encoding::UTF8->GetBytes(value);
				int byteCount = bytes->Length;

				char* result = new char[byteCount + 1];
				Marshal::Copy(bytes, 0, IntPtr(result), byteCount);
				result[byteCount] = '\0';

				return result;
			}

			String^ MarshallingHelper::UnmarshalString(char* value)
			{
				if (!value)
				{
					return nullptr;
				}

				size_t byteCount = strlen(value) * sizeof(*value);
				String^ result;

				if (byteCount > 0)
				{
					array<unsigned char, 1>^ bytes = gcnew array<Byte>(byteCount);
					pin_ptr<Byte> pinnedBytes = &bytes[0];
					memcpy(pinnedBytes, value, byteCount);

					result = Encoding::UTF8->GetString(bytes);
				}
				else
				{
					result = String::Empty;
				}

				return result;
			}

#pragma endregion

#pragma region ConstString

			const char* MarshallingHelper::MarshalConstString(String^ value)
			{
				return (const char*)MarshalString(value);
			}

			String^ MarshallingHelper::UnmarshalConstString(const char* value)
			{
				return UnmarshalString(custom_strdup(value));
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
						result[itemIndex] = UnmarshalString(items[itemIndex]);
					}
				}

				return result;
			}

#pragma endregion

			char* MarshallingHelper::custom_strdup(const char* value)
			{
				if (value == NULL)
				{
					return NULL;
				}

				if (value == nullptr)
				{
					return nullptr;
				}

				size_t length = strlen(value) + 1;
				char* result = (char*)sass_alloc_memory(length);
				std::memcpy(result, value, length);

				return result;
			}
		}
	}
}