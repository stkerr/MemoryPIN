#ifndef __REGION_MONITORING__
#define __REGION_MONITORING__

#include "OsDefines.h"

#include <boost/icl/split_interval_map.hpp>
extern boost::icl::interval_map<int, std::string> address_lib_interval_map;

struct region
{
	unsigned char* start;
	unsigned char* end;
};

void RecordMemWrite(void* ip, void* addr);
void DumpRegion(void* region);
void DumpAllMemoryRegions(char* filename);
void MemoryMonitorFunction(INS ins, void* region);
bool GetLibraryName(int address, std::string name);

extern struct region monitorRegion;



#endif