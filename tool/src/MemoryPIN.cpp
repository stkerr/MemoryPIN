#include <iostream>
#include <fstream>
#include <string>
#include <stdio.h>
#include <time.h>

#include "OsDefines.h"
#include "MonitoringEvent.h"
#include "RegionMonitoring.h"
#include "Logging.h"
#include "Snapshots.h"
#include "InstructionTracer.h"
#include "BasicBlockTracer.h"

/*
 * Command line switches to define the memory region to monitor
 */
KNOB<int> KnobStartRegion(KNOB_MODE_OVERWRITE, "pintool", "startRegion", "0", "Indicates the start of the region to monitor");
KNOB<int> KnobEndRegion(KNOB_MODE_OVERWRITE, "pintool", "endRegion", "0", "Indicates the end of the region to monitor");
KNOB<std::string> KnobResultsFile(KNOB_MODE_OVERWRITE, "pintool", "resultsfile", "memorypin.txt", "The file to save MemoryPIN results to.");
KNOB<BOOL> KnobInstructionTrace(KNOB_MODE_OVERWRITE, "pintool", "instructionTrace", "false", "Enable instruction tracing.");
KNOB<BOOL> KnobLibraryLoadTrace(KNOB_MODE_OVERWRITE, "pintool", "libraryLoadTrace", "false", "Print library load tracing.");
KNOB<BOOL> KnobMonitorRegion(KNOB_MODE_OVERWRITE, "pintool", "monitorRegion", "false", "Enable region monitoring.");


void ImageLoadedFunction(IMG img, void* data)
{
	if(KnobLibraryLoadTrace.Value())
	{
		char buffer[2048];
		memset(buffer, 0, 2048);
		snprintf(buffer, 2048, "Library Name: %50s | Start Address: %20d | End Address: %20d | Entry Address: %20d\n", IMG_Name(img).c_str(), IMG_LowAddress(img), IMG_HighAddress(img), IMG_Entry(img));

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


void appStartFunction(void* region)
{
	DumpRegion(&monitorRegion);
}

void appEndFunction(int exitCode, void* region)
{
	DumpRegion(&monitorRegion);
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
	strncpy(logger.filename, "instructionTrace.txt", 1024);
	strncpy(libraryLogger.filename, "libraryTrace.txt", 1024);

	PIN_InitSymbols();
	// Initialize PIN library. Print help message if -h(elp) is specified
	// in the command line or the command line is invalid 
	if (PIN_Init(argc, argv))
	{
		std::cerr << KNOB_BASE::StringKnobSummary() << endl;
		return -1;
	}

	// Initialize monitoring and snapshot events
	InitSnapshottingEvent();
	InitMonitoringEvent();

	resultsFile = fopen(KnobResultsFile.Value().c_str(), "w");
	TraceFile.open("tracefile.txt");

	PIN_SetSyntaxIntel();

	if (USE_BBLTRACE)
	{
		printf("Enabling BBLTrace() functionality.");
		TRACE_AddInstrumentFunction(BBLTrace, 0);
	}
	else
	{
		printf("Disabling BBLTrace() functionality.");
	}

	if (USE_IMAGELOADED)
	{
		IMG_AddInstrumentFunction(ImageLoadedFunction, 0);
		printf("Enabling ImageLoadedFunction()");
	}
	else
	{
		printf("Disabling ImageLoadedFunction()");
	}
    
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
    	INS_AddInstrumentFunction(MemoryMonitorFunction, 0);

    	fprintf(resultsFile, "Monitoring 0x%08x to 0x%08x\n",KnobStartRegion.Value(), KnobEndRegion.Value());
    }

    PIN_AddFiniFunction(FinishFunction, 0);

    // Start the program, never returns
    PIN_StartProgram();
    
    

    return 0;
}

/* ===================================================================== */
/* eof */
/* ===================================================================== */
