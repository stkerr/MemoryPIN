#include "pin.H"
#include <iostream>
#include <fstream>
#include <boost/icl/split_interval_map.hpp>
#include <string>
#include <stdio.h>
#include <time.h>

namespace WINDOWS 
{
	#include<Windows.h>
}

void dumpRegion(VOID * region);

struct region
{
	unsigned char* start;
	unsigned char* end;
};

/*
 * Command line switches to define the memory region to monitor
 */
KNOB<int> KnobStartRegion(KNOB_MODE_OVERWRITE, "pintool", "startRegion", "0", "Indicates the start of the region to monitor");
KNOB<int> KnobEndRegion(KNOB_MODE_OVERWRITE, "pintool", "endRegion", "0", "Indicates the end of the region to monitor");
KNOB<std::string> KnobResultsFile(KNOB_MODE_OVERWRITE, "pintool", "resultsfile", "memorypin.txt", "The file to save MemoryPIN results to.");
KNOB<BOOL> KnobInstructionTrace(KNOB_MODE_OVERWRITE, "pintool", "instructionTrace", "false", "Enable instruction tracing.");
KNOB<BOOL> KnobLibraryLoadTrace(KNOB_MODE_OVERWRITE, "pintool", "libraryLoadTrace", "false", "Print library load tracing.");
KNOB<BOOL> KnobMonitorRegion(KNOB_MODE_OVERWRITE, "pintool", "monitorRegion", "false", "Enable region monitoring.");


// Globals needed to record the region in memory to monitor globally
struct region monitorRegion;
int writeInRange = 0;
std::ostream * out = &cerr;
bool instructionTraceManualMode = false;
boost::icl::interval_map<int, std::string> address_lib_interval_map;
FILE *resultsFile = 0;
FILE *instructionLogFile = 0;
WINDOWS::HANDLE hMonitoringEvent = 0;
WINDOWS::HANDLE hSnapshotEvent = 0;
bool lastMonitoringStatus = 0; // record the last value for the monitoring event
int memDumpCount = 1;
unsigned long instructionCount = 0; // the number of instructions we have traced

bool CheckMonitoringEvent()
{
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hMonitoringEvent, 0);

	if(result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;

}

bool CheckSnapshotEvent()
{
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hSnapshotEvent, 0);

	if(result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;	
}

void EnableMonitoringEvent()
{
	// signal the event
	WINDOWS::SetEvent(hMonitoringEvent);
}

void EnableSnapshotEvent()
{
	WINDOWS::SetEvent(hSnapshotEvent);
}
void DisableMonitoringEvent()
{
	WINDOWS::ResetEvent(hMonitoringEvent);
}

void DisableSnapshotEvent()
{
	WINDOWS::ResetEvent(hSnapshotEvent);
}

void InitEvents()
{
	int error = 0;

	hMonitoringEvent = WINDOWS::OpenEvent(
		EVENT_ALL_ACCESS,
		false,
		"MonitoringEvent"
		);

	error = WINDOWS::GetLastError();
	if(hMonitoringEvent == 0 && error == 2)
	{
		hMonitoringEvent = WINDOWS::CreateEvent( 
        NULL,               // default security attributes
        TRUE,               // manual-reset event
        FALSE,              // initial state is nonsignaled
        TEXT("MonitoringEvent")  // object name
        );	
	}

	hSnapshotEvent = WINDOWS::OpenEvent(
		EVENT_ALL_ACCESS,
		false,
		"SnapshotEvent"
		);
	
	error = WINDOWS::GetLastError();
	if(hSnapshotEvent == 0 && error == 2)
	{
		hSnapshotEvent = WINDOWS::CreateEvent( 
	        NULL,               // default security attributes
	        TRUE,               // manual-reset event
	        FALSE,              // initial state is nonsignaled
	        TEXT("SnapshotEvent")  // object name
	        );
	}
}

void DumpAllMemoryRegions(char* filename)
{
	FILE *memoryFile = fopen(filename, "w");
	
	boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

	while(it != address_lib_interval_map.end())
    {
        boost::icl::discrete_interval<int> range = (*it).first;
        std::string name = (*it++).second;
        fprintf(memoryFile, "%s\n",name.c_str());
        fprintf(memoryFile, "0x%08x 0x%08x\n", range.lower(), range.upper());

        unsigned char* regionIterator = (unsigned char*)range.lower();

        
        while(regionIterator != (unsigned char*)range.upper())
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

void InstructionTrace(INS ins, void* v)
{
	// check if we just received a snapshot request
	bool snapshotStatus = CheckSnapshotEvent();
	if(snapshotStatus)
	{
		// We just detected a monitoring change! Take a memory snapshot now
		char filename[1024];
		memset(filename, 0, 1024);
		sprintf(filename,"memorydump.%d.dmp",memDumpCount++);
		DumpAllMemoryRegions(filename);

		// reset the event
		DisableSnapshotEvent();
	}

	// check the monitoring event if we should still be monitoring or not 
	bool monitoringStatus = CheckMonitoringEvent();
	if(!monitoringStatus)
	{
		return;
	}
	
	fprintf(instructionLogFile, "Thread ID: %8d |", PIN_GetTid());

    fprintf(instructionLogFile, "Instruction Address: %08d |", (int)INS_Address(ins));

    boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

    while(it != address_lib_interval_map.end())
    {
        boost::icl::discrete_interval<int> range = (*it).first;
        std::string name = (*it++).second;
        if(boost::icl::contains(range, (int)INS_Address(ins)))
        {
            fprintf(instructionLogFile, "Library Name: %s |", name.c_str());
            //std::cout << range << ":" << name << std::endl;
            break;
        }
    }
    fprintf(instructionLogFile, "Instruction Count: %d |", instructionCount++);
	fprintf(instructionLogFile, "Time: %d |", WINDOWS::GetTickCount());
    fprintf(instructionLogFile, "\n");
 }

void ImageLoadedFunction(IMG img, void* data)
{
	if(KnobLibraryLoadTrace.Value())
	{
		//printf("Image loaded\n");
		fprintf(resultsFile, "------\n");
		fprintf(resultsFile, "Loading Image: %s\n", IMG_Name(img).c_str());
		fprintf(resultsFile, "\tLoading Location:\n");
		fprintf(resultsFile, "\t0x%08x to 0x%08x\n", (int)IMG_LowAddress(img), (int)IMG_HighAddress(img));
		fprintf(resultsFile, "\tFirst Instruction address: 0x%08x\n", (int)IMG_Entry(img));
		//fprintf(resultsFile, "\tFirst Section: 0x%08x\n", (int)IMG_SecHead(img));
		//fprintf(resultsFile, "\tLast Section: 0x%08x\n", (int)IMG_SecTail(img));
		fprintf(resultsFile, "------\n");
		fflush(resultsFile);
	}
    boost::icl::discrete_interval<int> itv = boost::icl::discrete_interval<int>::right_open(
        (int)IMG_LowAddress(img),
        (int)IMG_HighAddress(img)
    );

    address_lib_interval_map.add(
        make_pair(
            itv,
            std::string(IMG_Name(img))
        )
        );

}

// Potentially set a memory dump after the memory write if it's in the region we care about
VOID RecordMemWrite(VOID * ip, VOID * addr)
{
	struct region * memRegion = &monitorRegion;;

	writeInRange = 0;

	if((unsigned char*)addr >= memRegion->start && (unsigned char*)addr < memRegion->end)
	{
		// printf("%p: W %p\n", ip, addr);
		writeInRange = 1;
	}

}


void dumpRegion(VOID * region)
{
	struct region * memRegion = (struct region*)region;
	printf("Dumping: %p %p\n", memRegion->start, memRegion->end);

	if(memRegion->end - memRegion->start < 0)
	{
		fprintf(resultsFile, "Invalid memory region specified!\n");
		return;
	}
	fprintf(resultsFile, "-----\n");
	fprintf(resultsFile, "Dumping region: %p to %p\n", memRegion->start, memRegion->end);

	unsigned char* iterator = 0;
	int charCount = 0;
	for(iterator = memRegion->start; iterator != memRegion->end; iterator++)
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

void appStartFunction(void* region)
{
	dumpRegion(&monitorRegion);
}

void appEndFunction(int exitCode, void* region)
{
	dumpRegion(&monitorRegion);
}

void memoryMonitorFunction(INS ins, void* region)
{
	// does this instruction write memory?
	if(INS_IsMemoryWrite(ins) != true)
	{
		return;
	}

	// iterate over all operands
	for(unsigned int i = 0; i < INS_OperandCount(ins); i++)
	{
		// check if it is a memory operand
		if(INS_OperandIsMemory(ins, i) != true)
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

			if(writeInRange != 0)
			{
				printf("Found it!\n");
				INS_InsertCall(
					ins, IPOINT_AFTER, (AFUNPTR)dumpRegion,
					IARG_PTR, &monitorRegion,
					IARG_END);
			}
		}
	}
}

/*!
 * The main procedure of the tool.
 * This function is called when the application image is loaded but not yet started.
 * @param[in]   argc            total number of elements in the argv array
 * @param[in]   argv            array of command line arguments, 
 *                              including pin -t <toolname> -- ...
 */
int main(int argc, char *argv[])
{
    // Initialize PIN library. Print help message if -h(elp) is specified
    // in the command line or the command line is invalid 
    if( PIN_Init(argc,argv) )
    {
        std::cerr << KNOB_BASE::StringKnobSummary() << endl;
        return -1;
    }

    // Initialize monitoring and snapshot events
    InitEvents();

    resultsFile = fopen(KnobResultsFile.Value().c_str(), "w");

    IMG_AddInstrumentFunction(ImageLoadedFunction, 0);

    if(KnobInstructionTrace.Value())
    {
    	
    	INS_AddInstrumentFunction(InstructionTrace, 0);
    	instructionLogFile = fopen("instructionTrace.txt","w");
    }

    if(KnobMonitorRegion.Value())
    {
    	if(KnobStartRegion.Value() == 0)
    	{
    		printf("You must specify a start region address!\n");
    		fprintf(resultsFile, "You must specify a start region address!\n");
    		return -1;
    	}

    	if(KnobEndRegion.Value() == 0)
		{
			printf("You must specify an end region address!\n");
			fprintf(resultsFile, "You must specify an end region address!\n");
			return -2;
		}

    	// Set up the region to monitor
    	monitorRegion.start = reinterpret_cast<unsigned char*>(KnobStartRegion.Value());
    	monitorRegion.end = reinterpret_cast<unsigned char*>(KnobEndRegion.Value());

    	// Add a start and end of application log function
    	PIN_AddApplicationStartFunction(appStartFunction, 0);
    	PIN_AddFiniFunction(appEndFunction, 0);

    	// Add an instruction instrumentation function to monitor the memory region
    	INS_AddInstrumentFunction(memoryMonitorFunction, 0);

    	fprintf(resultsFile, "Monitoring 0x%08x to 0x%08x\n",KnobStartRegion.Value(), KnobEndRegion.Value());
    }

    // Start the program, never returns
    PIN_StartProgram();
    
    fclose(resultsFile);
    fclose(instructionLogFile);

    return 0;
}

/* ===================================================================== */
/* eof */
/* ===================================================================== */
