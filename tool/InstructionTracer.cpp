#include <boost/icl/split_interval_map.hpp>

#include "InstructionTracer.h"
#include "Logging.h"
#include "MonitoringEvent.h"
#include "Snapshots.h"
#include "RegionMonitoring.h"

unsigned long instructionCount = 0; // the number of instructions we have traced

unsigned int executionDepth = 0;

void InstructionTrace(INS ins, void* v)
{
	// check if we just received a snapshot request
	bool snapshotStatus = CheckSnapshotEvent();
	if (snapshotStatus)
	{
		// We just detected a monitoring change! Take a memory snapshot now
		TakeSnapshot();

		// reset the event
		DisableSnapshotEvent();
	}

	// check the monitoring event if we should still be monitoring or not 
	bool monitoringStatus = CheckMonitoringEvent();
	if (!monitoringStatus)
	{
		return;
	}

	char buffer[1024];
	memset(buffer, 0, 1024);
	snprintf(buffer, 1024, "Thread ID: %8d |", PIN_GetTid());
	LogMessage(&logger, buffer);

	memset(buffer, 0, 1024);
	snprintf(buffer, 1024, "Instruction Address: %d |", INS_Address(ins));
	LogMessage(&logger, buffer);
	
	std::string libraryName;
	if (GetLibraryName(INS_Address(ins), &libraryName))
	{
		memset(buffer, 0, 256);
		snprintf(buffer, 256, "Library Name: %s |", libraryName.c_str());
		LogMessage(&logger, buffer);
	}
	else
	{
		memset(buffer, 0, 256);
		snprintf(buffer, 256, "Library Name: %s |", "UNKNOWN");
		LogMessage(&logger, buffer);
	}


	if (INS_IsCall(ins) == true)
	{
		executionDepth++;
	}
	else if (INS_IsRet(ins) == true)
	{
		executionDepth--;
	}

	memset(buffer, 0, 256);
	snprintf(buffer, 256, "Instruction Count: %lu |", instructionCount++);
	LogMessage(&logger, buffer);

#ifdef TARGET_WINDOWS
	memset(buffer, 0, 256);
	snprintf(buffer, 256, "Time: %d |", WINDOWS::GetTickCount());
	LogMessage(&logger, buffer);
#else
	timeval time;
	gettimeofday(&time, NULL);
	unsigned long millis = (time.tv_sec * 1000) + (time.tv_usec / 1000);
	memset(buffer, 0, 256);
	snprintf(buffer, 256, "Time: %lu |", millis);
	LogMessage(&logger, buffer);
#endif

	memset(buffer, 0, 256);
	snprintf(buffer, 256, "Depth: %d |", executionDepth);
	LogMessage(&logger, buffer);

	memset(buffer, 0, 256);
	snprintf(buffer, 256, "\n");
	LogMessage(&logger, buffer);
}