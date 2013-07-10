// This is the main DLL file.

#include "stdafx.h"
#include <msclr\marshal_cppstd.h>

using namespace msclr::interop;

#include "NativeProcessor.h"

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

	// open the file
	FILE* libraryFile = fopen(name.c_str(), "r");
	if(libraryFile == 0)
		return;
	fseek(libraryFile, 0, SEEK_END);
	int length = ftell(libraryFile);
	char* fileBuffer = (char*)malloc(sizeof(char) * length);
	fseek(libraryFile, 0, SEEK_SET);
	fread(fileBuffer, 1, length, libraryFile);

//	IMAGE_NT_HEADERS* header = ImageNtHeader(fileBuffer);

	memcpy(&idh, fileBuffer, sizeof(IMAGE_DOS_HEADER));
	//idh = (IMAGE_DOS_HEADER*)fileBuffer;

	// This code get the optional header from DOS 
	// header based on the offset from DOS header.
	memcpy(&ioh, ((BYTE*)fileBuffer + this->idh.e_lfanew + 24), sizeof(IMAGE_OPTIONAL_HEADER));
	// ioh = (IMAGE_OPTIONAL_HEADER*)((BYTE*)hLibrary + this->idh->e_lfanew + 24);

	// This code gets the import descriptor from the base 
	// address by taking the offset of start address and address of       
	// IMAGE_DIRECTORY_ENTRY_IMPORT, after getting IMAGE_IMPORT_DESCRIPTOR, 
	// you can get the imported functions or ordinals.
	//memcpy(&iid, (BYTE*)fileBuffer + this->ioh.DataDirectory[ IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress, sizeof(IMAGE_IMPORT_DESCRIPTOR));
	//iid = 	(IMAGE_IMPORT_DESCRIPTOR*)((BYTE*)hLibrary + this->ioh->DataDirectory[ IMAGE_DIRECTORY_ENTRY_IMPORT].VirtualAddress);

	// Same as above code below will give the Export information.
	//memcpy(&pExportDescriptor, ((BYTE*)fileBuffer + this->ioh.DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].VirtualAddress), sizeof(_IMAGE_EXPORT_DIRECTORY));
	//pExportDescriptor = (_IMAGE_EXPORT_DIRECTORY*)((BYTE*)hLibrary + this->ioh->DataDirectory[IMAGE_DIRECTORY_ENTRY_EXPORT].VirtualAddress);
	free(fileBuffer);
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
	 DWORD                AddressOfEntryPoint;
  DWORD                BaseOfCode;
  DWORD                BaseOfData;
  DWORD                ImageBase;
	int x = this->ioh.AddressOfEntryPoint;
	x = this->ioh.BaseOfCode;
	x = this->ioh.BaseOfData;
	return this->ioh.ImageBase;
}