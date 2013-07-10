// NativeProcessor.h

#pragma once

using namespace System;
#include <string>

public class NativeLibraryProcessor
{

private:
	HANDLE hLibrary;
	IMAGE_DOS_HEADER idh;
	IMAGE_OPTIONAL_HEADER ioh;
	IMAGE_IMPORT_DESCRIPTOR iid;
	_IMAGE_EXPORT_DIRECTORY pExportDescriptor;
	std::string name;

public:
	NativeLibraryProcessor(std::string path);
	~NativeLibraryProcessor();

	int GetImageBase();
};

public ref class LibraryProcessor
{

private:
	NativeLibraryProcessor *nativeLibraryProcessor;
	// TODO: Add your methods for this class here.
public:
	LibraryProcessor(String ^name);
	~LibraryProcessor();

	int GetImageBase();
};