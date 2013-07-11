// NativeProcessor.h

#pragma once

using namespace System;
#include <string>

public class NativeLibraryProcessor
{

private:
	HANDLE hLibrary;
	PIMAGE_NT_HEADERS headers;
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