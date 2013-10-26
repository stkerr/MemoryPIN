#include "RegionMonitoring.h"
#include "Logging.h"

boost::icl::interval_map<int, std::string> address_lib_interval_map;
struct region monitorRegion;
int writeInRange = 0;

// Potentially set a memory dump after the memory write if it's in the region we care about
VOID RecordMemWrite(VOID * ip, VOID * addr)
{
	struct region * memRegion = &monitorRegion;;

	writeInRange = 0;

	if ((unsigned char*)addr >= memRegion->start && (unsigned char*)addr < memRegion->end)
	{
		// printf("%p: W %p\n", ip, addr);
		writeInRange = 1;
	}

}

void DumpRegion(VOID * region)
{
	struct region * memRegion = (struct region*)region;
	printf("Dumping: %p %p\n", memRegion->start, memRegion->end);

	if (memRegion->end - memRegion->start < 0)
	{
		fprintf(resultsFile, "Invalid memory region specified!\n");
		return;
	}
	fprintf(resultsFile, "-----\n");
	fprintf(resultsFile, "Dumping region: %p to %p\n", memRegion->start, memRegion->end);

	unsigned char* iterator = 0;
	int charCount = 0;
	for (iterator = memRegion->start; iterator != memRegion->end; iterator++)
	{

		fprintf(resultsFile, "%02x ", *iterator);
		charCount++;
		(charCount >= 16) ?
			fprintf(resultsFile, "\n"),
			charCount = 0
			:
			false
			;
	}
	fprintf(resultsFile, "\n");
	fprintf(resultsFile, "-----\n");
	fflush(resultsFile);
}

void DumpAllMemoryRegions(char* filename)
{
	FILE *memoryFile = fopen(filename, "w");

	boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

	while (it != address_lib_interval_map.end())
	{
		boost::icl::discrete_interval<int> range = (*it).first;
		std::string name = (*it++).second;
		fprintf(memoryFile, "%s\n", name.c_str());
		fprintf(memoryFile, "0x%08x 0x%08x\n", range.lower(), range.upper());

		unsigned char* regionIterator = (unsigned char*)range.lower();


		while (regionIterator != (unsigned char*)range.upper())
		{
			//if(!WINDOWS::IsBadCodePtr((WINDOWS::FARPROC)regionIterator)) // DANGER! This will not protect against another thread mucking around with region access permissions
			//{
			// regionIterator++;
			// continue;
			// }
			fprintf(memoryFile, "%c", *regionIterator);
			regionIterator++;
		}
		fflush(memoryFile);
		break;
	}
	fclose(memoryFile);
}

void MemoryMonitorFunction(INS ins, void* region)
{
	// does this instruction write memory?
	if (INS_IsMemoryWrite(ins) != true)
	{
		return;
	}

	// iterate over all operands
	for (unsigned int i = 0; i < INS_OperandCount(ins); i++)
	{
		// check if it is a memory operand
		if (INS_OperandIsMemory(ins, i) != true)
		{
			continue;
		}

		if (INS_MemoryOperandIsWritten(ins, i))
		{
			INS_InsertCall(
				ins, IPOINT_BEFORE, (AFUNPTR)RecordMemWrite,
				IARG_INST_PTR,
				IARG_MEMORYOP_EA, i,
				IARG_END);

			if (writeInRange != 0)
			{
				printf("Found it!\n");
				INS_InsertCall(
					ins, IPOINT_AFTER, (AFUNPTR)DumpRegion,
					IARG_PTR, &monitorRegion,
					IARG_END);
			}
		}
	}
}

bool GetLibraryName(const int address, std::string name)
{
	boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

	while (it != address_lib_interval_map.end())
	{
		boost::icl::discrete_interval<int> range = (*it).first;
		
		if (boost::icl::contains(range, address))
		{
			name = (*it++).second;
			return true;
		}
	}
	return false;
}