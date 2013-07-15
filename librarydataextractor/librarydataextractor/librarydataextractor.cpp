#include <Windows.h>
#include "imagehlp.h"
#include <sstream>
#include <string>
#include <vector>
#include <iostream>
#include <fstream>
#include <map>


using namespace std;


unsigned int GetImageBase(std::string name)
{
	HANDLE libFile = CreateFileA(name.c_str(), GENERIC_READ, FILE_SHARE_READ, 0, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, 0);
	HANDLE fileMapping = CreateFileMapping(libFile, 0, PAGE_READONLY, 0, 0, 0);
	LPVOID mappedView = MapViewOfFile(fileMapping, FILE_MAP_READ, 0, 0, 0);
	IMAGE_NT_HEADERS* headers = ImageNtHeader(mappedView);
	CloseHandle(fileMapping);
	CloseHandle(libFile);
	return headers->OptionalHeader.ImageBase;

}

void ListDLLFunctions(string sADllName, map<string, pair<unsigned int, unsigned int>>& slListOfDllFunctions)
{
    DWORD *dNameRVAs(0);
    _IMAGE_EXPORT_DIRECTORY *ImageExportDirectory;
    unsigned long cDirSize;
    _LOADED_IMAGE LoadedImage;
    string sName;
    slListOfDllFunctions.clear();
	unsigned int imageBase =  GetImageBase(sADllName);
    if (MapAndLoad(sADllName.c_str(), NULL, &LoadedImage, TRUE, TRUE))
    {
        ImageExportDirectory = (_IMAGE_EXPORT_DIRECTORY*)
            ImageDirectoryEntryToData(LoadedImage.MappedAddress,
            false, IMAGE_DIRECTORY_ENTRY_EXPORT, &cDirSize);
        if (ImageExportDirectory != NULL)
        {
            dNameRVAs = (DWORD *)ImageRvaToVa(LoadedImage.FileHeader, 
                LoadedImage.MappedAddress,
            ImageExportDirectory->AddressOfNames, NULL);
            for(size_t i = 0; i < ImageExportDirectory->NumberOfNames; i++)
            {
                sName = (char *)ImageRvaToVa(LoadedImage.FileHeader, 
                        LoadedImage.MappedAddress,
                       dNameRVAs[i], NULL);
				slListOfDllFunctions[sName] = pair<unsigned int, unsigned int>(dNameRVAs[i],dNameRVAs[i] + imageBase);
            }
        }
        UnMapAndLoad(&LoadedImage);
    }
}

int main(int argc, char** argv)
{
	if(argc < 2)
	{
		printf("Usage: %s filename\n", argv[0]);
		printf("\tEach line should contain the full path of a library to list symbols for.\n");
		return -1;
	}

	ifstream infile(argv[1]);
	if(infile.is_open())
	{
		std::string line;
		while (std::getline(infile, line))
		{
			map<string, pair<unsigned int, unsigned int>> names;
			map<string, pair<unsigned int, unsigned int>>::iterator it;
			ListDLLFunctions(line.c_str(), names);
			for(it = names.begin(); it != names.end(); it++)
			{
				printf("%s:%08X:%08X\n",it->first.c_str(),it->second.first,it->second.second);
			}
		    //printf("%s\n",line.c_str());
		}
		infile.close();
	}

}

