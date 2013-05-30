#include "pin.H"
#include <iostream>
#include <fstream>
#include <boost/icl/split_interval_map.hpp>
#include <string>
#include <stdio.h>

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
std::ostream * out = &cerr;

boost::icl::interval_map<int, std::string> address_lib_interval_map;

FILE *resultsFile = 0;

void instructionTrace(INS ins, void* v)
{
    fprintf(resultsFile, "Instruction Address: 0x%08x : ", (int)INS_Address(ins));

    boost::icl::interval_map<int, std::string>::iterator it = address_lib_interval_map.begin();

    while(it != address_lib_interval_map.end())
    {
        boost::icl::discrete_interval<int> range = (*it).first;
        std::string name = (*it++).second;
        if(boost::icl::contains(range, (int)INS_Address(ins)))
        {
            fprintf(resultsFile, "%s", name.c_str());
            //std::cout << range << ":" << name << std::endl;
            break;
        }
    }
    fprintf(resultsFile, "\n");
 }

void imageLoaded(IMG img, void* data)
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

void dumpRegion(struct region *memRegion)
{
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
					fprintf(resultsFile, "\n")
					:
					false
					;
		}
		fprintf(resultsFile, "\n");
		fprintf(resultsFile, "-----\n");
}

void appStartFunction(void* region)
{
	dumpRegion((struct region*)region);
}

void appEndFunction(int exitCode, void* region)
{
	dumpRegion((struct region*)region);
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
    PIN_Init(argc,argv);

    resultsFile = fopen(KnobResultsFile.Value().c_str(), "w");

    IMG_AddInstrumentFunction(imageLoaded, 0);

    if(KnobInstructionTrace.Value())
    {
    	INS_AddInstrumentFunction(instructionTrace, 0);
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
    	struct region memRegion;
    	memRegion.start = reinterpret_cast<unsigned char*>(KnobStartRegion.Value());
    	memRegion.end = reinterpret_cast<unsigned char*>(KnobEndRegion.Value());

    	// Add a start and end of application log function
    	PIN_AddApplicationStartFunction(appStartFunction, &memRegion);
    	PIN_AddFiniFunction(appEndFunction, &memRegion);

    	fprintf(resultsFile, "Monitoring 0x%08x to 0x%08x\n",KnobStartRegion.Value(), KnobEndRegion.Value());
    }

    // Start the program, never returns
    PIN_StartProgram();
    
    fclose(resultsFile);

    return 0;
}

/* ===================================================================== */
/* eof */
/* ===================================================================== */
