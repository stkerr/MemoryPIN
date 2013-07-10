#include <iostream>
#include <Windows.h>

__declspec(dllexport)
	void parseLibraryEntry(const char* libraryName, int *imageBase)
{
	HANDLE hLib = LoadLibraryA(libraryName);
	IMAGE_DOS_HEADER* idh = (IMAGE_DOS_HEADER*)hLib;
	IMAGE_OPTIONAL_HEADER* ioh = (IMAGE_OPTIONAL_HEADER*)((BYTE*)hLib + idh->e_lfanew + 24);
	*imageBase = ioh->ImageBase;
}

BOOLEAN WINAPI DllMain( IN HINSTANCE hDllHandle, 
	IN DWORD     nReason, 
	IN LPVOID    Reserved )
{
	BOOLEAN bSuccess = TRUE;
	//  Perform global initialization.
	switch ( nReason )
	{
	case DLL_PROCESS_ATTACH:
		//  For optimization.
		DisableThreadLibraryCalls( hDllHandle );
		break;
	case DLL_PROCESS_DETACH:
		break;
	}
	return bSuccess;
}