// This is the main DLL file.

#include "stdafx.h"
#include <msclr\marshal_cppstd.h>

using namespace msclr::interop;

#include "NativeProcessor.h"
#include <Dbghelp.h>

LibraryProcessor::LibraryProcessor(String ^name)
{
	std::string standardString = marshal_as<std::string>(name);

	nativeLibraryProcessor = new NativeLibraryProcessor(
		standardString
		);
}

LibraryProcessor::~LibraryProcessor()
{
}

int LibraryProcessor::GetImageBase()
{
	return this->nativeLibraryProcessor->GetImageBase();
}

NativeLibraryProcessor::NativeLibraryProcessor(std::string name)
{
	this->name = name;
	
	HANDLE libFile = CreateFileA(name.c_str(), GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	HANDLE fileMapping = CreateFileMapping(libFile, 0, PAGE_READONLY, 0, 0, 0);
	LPVOID mappedView = MapViewOfFile(fileMapping, FILE_MAP_READ, 0, 0, 0);
	this->headers = ImageNtHeader(mappedView);
	CloseHandle(fileMapping);
	CloseHandle(libFile);
}

NativeLibraryProcessor::~NativeLibraryProcessor()
{
}

int NativeLibraryProcessor::GetImageBase()
{
	if(hLibrary == 0)
	{
		return 0;
	}

	return this->headers->OptionalHeader.ImageBase;
}