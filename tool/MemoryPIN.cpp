#include "pin.H"
#include <iostream>
#include <fstream>
#include <boost/icl/split_interval_map.hpp>
#include <string>
#include <stdio.h>
#include <time.h>

#ifdef WINDOWS
namespace WINDOWS 
{
	#include<Windows.h>
	HANDLE hMonitoringEvent = 0;
	HANDLE hSnapshotEvent = 0;
}
#else
namespace POSIX
{
	#include <semaphore.h>
	#include <pthread.h>
	#include <semaphore.h>
	#include <time.h>
	sem_t *sMonitoringSem = 0;
	sem_t *sSnapshotSem = 0;
	
	bool bMonitoring = true;
	bool bSnapshoting = false;

}
#endif
void dumpRegion(VOID * region);

struct region
{
	unsigned char* start;
	unsigned char* end;
};

typedef struct logObject_t
{
	char filename[1024];
	void* buffer;
	unsigned int usedSize;
	unsigned int maxSize;
} logObject;

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
bool lastMonitoringStatus = 0; // record the last value for the monitoring event
int memDumpCount = 1;
unsigned long instructionCount = 0; // the number of instructions we have traced
std::ofstream TraceFile;
UINT32 count_trace = 0; // current trace number
unsigned int executionDepth = 0;
logObject logger;
logObject libraryLogger;

bool LogMessage(logObject *loggerObject, char* message)
{
	if(message == 0 || message[0] == 0)
	{
		return false;
	}
	if(loggerObject->buffer == 0)
	{
		loggerObject->buffer = malloc(1024 * 1024); // 1 MB log by default
		memset(loggerObject->buffer, 0, 1024 * 1024);
		loggerObject->maxSize = 1024*1024;
	}

	unsigned int length = strlen(message);
	
	//printf("log size: %d/%d\n", loggerObject->usedSize, loggerObject->maxSize);
	if(loggerObject->usedSize + length >= loggerObject->maxSize)
	{
		// allocate some new memory
		loggerObject->buffer = realloc(loggerObject->buffer, loggerObject->maxSize * 2);
		loggerObject->maxSize *= 2;
		printf("Resizing memory! %s\n", message);
	}

	strcat((char*)loggerObject->buffer + loggerObject->usedSize, message);
	loggerObject->usedSize = loggerObject->usedSize + length;
	
	return true;
}

bool CloseLog(logObject *loggerObject)
{
	if(loggerObject == 0)
	{
		printf("Logger is null!\n");
		return false;
	}

	printf("Writing log to: %s\n", loggerObject->filename);
	FILE* logFile = fopen(loggerObject->filename, "w");
	fprintf(logFile, "%s", loggerObject->buffer);
	fclose(logFile);
	return true;
}


bool CheckMonitoringEvent()
{
#ifdef WINDOWS
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hMonitoringEvent, 0);

	if(result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	bool result = POSIX::bMonitoring;
	POSIX::sem_post(POSIX::sMonitoringSem);
	return result;
#endif

}

bool CheckSnapshotEvent()
{
#ifdef WINDOWS
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hSnapshotEvent, 0);

	if(result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;	
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	bool result = POSIX::bSnapshoting;
	POSIX::sem_post(POSIX::sSnapshotSem);
	return result;
#endif
}

void EnableMonitoringEvent()
{
#ifdef WINDOWS
	// signal the event
	WINDOWS::SetEvent(hMonitoringEvent);
	pthread_mutexattr_setname_np(mMonitoringEvent);
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	POSIX::bMonitoring = true;
	POSIX::sem_post(POSIX::sMonitoringSem);
#endif
}

void EnableSnapshotEvent()
{
#ifdef WINDOWS
	WINDOWS::SetEvent(hSnapshotEvent);
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	POSIX::bSnapshoting = true;
	POSIX::sem_post(POSIX::sSnapshotSem);
#endif
}
void DisableMonitoringEvent()
{
#ifdef WINDOWS
	WINDOWS::ResetEvent(hMonitoringEvent);
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	POSIX::bMonitoring = false;
	POSIX::sem_post(POSIX::sMonitoringSem);
#endif
}

void DisableSnapshotEvent()
{
#ifdef WINDOWS
	WINDOWS::ResetEvent(hSnapshotEvent);
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	POSIX::bSnapshoting = false;
	POSIX::sem_post(POSIX::sSnapshotSem);
#endif
}

void InitEvents()
{
#ifdef WINDOWS
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
#else
	// Create the semaphores
	POSIX::sMonitoringSem = POSIX::sem_open("MONITORING_SEMAPHORE", O_CREAT, S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP | S_IROTH | S_IWOTH, 0);
	POSIX::sem_init(POSIX::sMonitoringSem, 1, 0); // shared, not signaled

	POSIX::sSnapshotSem = POSIX::sem_open("SNAPSHOT_SEMAPHORE", O_CREAT, S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP | S_IROTH | S_IWOTH, 0);
	POSIX::sem_init(POSIX::sSnapshotSem, 1, 0); // shared, not signaled

#endif
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
	
	char buffer[1024];
	memset(buffer, 0, 1024);
	snprintf(buffer, 1024, "Thread ID: %8d |", PIN_GetTid());
	LogMessage(&logger, buffer);

	memset(buffer, 0, 1024);
	snprintf(buffer, 1024, "Instruction Address: %llx |", INS_Address(ins));
	LogMessage(&logger, buffer);

    boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

    while(it != address_lib_interval_map.end())
    {
        boost::icl::discrete_interval<int> range = (*it).first;
        std::string name = (*it++).second;
        if(boost::icl::contains(range, (int)INS_Address(ins)))
        {
		memset(buffer, 0, 256);
		snprintf(buffer, 256, "Library Name: %s |", name.c_str());
		LogMessage(&logger,buffer);
            break;
        }
    }

    if(INS_IsCall(ins) == true)
    {
    	executionDepth++;
    }
    else if(INS_IsRet(ins) == true)
    {
    	executionDepth--;
    }

    memset(buffer, 0, 256);
    snprintf(buffer, 256, "Instruction Count: %lu |", instructionCount++);
    LogMessage(&logger,buffer);

#ifdef WINDOWS
    memset(buffer, 0, 256);
    snprintf(buffer, 256, "Time: %d |", WINDOWS::GetTickCount());
    LogMessage(&logger,buffer);
#else
    timeval time;
    gettimeofday(&time, NULL);
    unsigned long millis = (time.tv_sec * 1000) + (time.tv_usec / 1000);
    memset(buffer, 0, 256);
    snprintf(buffer, 256, "Time: %lu |", millis);
    LogMessage(&logger,buffer);
#endif
    memset(buffer, 0, 256);
    snprintf(buffer, 256, "Depth: %d |", executionDepth);
    LogMessage(&logger,buffer);

    memset(buffer, 0, 256);
    snprintf(buffer, 256, "\n");
    LogMessage(&logger,buffer);
 }

void ImageLoadedFunction(IMG img, void* data)
{
	if(KnobLibraryLoadTrace.Value())
	{
		char buffer[2048];
		memset(buffer, 0, 2048);
		snprintf(buffer, 2048, "Library Name: %50s | Start Address: %16llx | End Address: %16llx | Entry Address: %16llx\n", IMG_Name(img).c_str(), IMG_LowAddress(img), IMG_HighAddress(img), IMG_Entry(img));
		LogMessage(&libraryLogger, buffer);
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

void finishFunction(int code, void* data)
{
	if(resultsFile != 0)
		fclose(resultsFile);

	if(instructionLogFile != 0)
		fclose(instructionLogFile);

	CloseLog(&logger);
	CloseLog(&libraryLogger);

	TraceFile.close();
}

void traceBBL(const string *s)
{
	TraceFile.write(s->c_str(), s->size());
}

VOID BBLTrace(TRACE trace, VOID *v)
{
    for (BBL bbl = TRACE_BblHead(trace); BBL_Valid(bbl); bbl = BBL_Next(bbl))
    {
        string traceString = "";
        
        for ( INS ins = BBL_InsHead(bbl); INS_Valid(ins); ins = INS_Next(ins))
        {
            //traceString +=  "%" + INS_Disassemble(ins) + "\n";
        }
        

        // we try to keep the overhead small 
        // so we only insert a call where control flow may leave the current trace
        
        INS_InsertCall(BBL_InsTail(bbl), IPOINT_BEFORE, AFUNPTR(traceBBL),
        			   IARG_PTR, new string(traceString),
                       IARG_END);
        // Identify traces with an id
        count_trace++;

        // Write the actual trace once at instrumentation time
        string m = "@" + decstr(count_trace) + "\n";
        TraceFile.write(m.c_str(), m.size());            
        TraceFile.write(traceString.c_str(), traceString.size());
        
        
        // at run time, just print the id
        string *s = new string(decstr(count_trace) + "\n");
        INS_InsertCall(BBL_InsTail(bbl), IPOINT_BEFORE, AFUNPTR(traceBBL),
                       IARG_PTR, s,
                       IARG_END);
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
    strncpy(logger.filename,"instructionTrace.txt",1024);
    strncpy(libraryLogger.filename,"libraryTrace.txt",1024);

    PIN_InitSymbols();
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
    TraceFile.open("tracefile.txt");

	PIN_SetSyntaxIntel();
	TRACE_AddInstrumentFunction(BBLTrace, 0);
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

    PIN_AddFiniFunction(finishFunction, 0);

    // Start the program, never returns
    PIN_StartProgram();
    
    

    return 0;
}

/* ===================================================================== */
/* eof */
/* ===================================================================== */
