#include <boost/icl/split_interval_map.hpp>

#include "InstructionTracer.h"
#include "Logging.h"
#include "MonitoringEvent.h"
#include "Snapshots.h"
#include "RegionMonitoring.h"

unsigned long instructionCount = 0; // the number of instructions we have traced

unsigned int executionDepth = 0; // will trace how "deep" we are, in terms of calls and returns

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

	instruction_trace trace;
	trace.tid = PIN_GetTid();
	trace.address = INS_Address(ins);

	memset(trace.library_name, 0, 260);
	std::string libraryName;
	if (CHECK_LIBRARY_NAMES && GetLibraryName(trace.address, libraryName))
	{
		snprintf(trace.library_name, sizeof(trace.library_name), "%s", libraryName.c_str());
	}
	

	if (INS_IsCall(ins) == true)
	{
		trace.execution_depth = executionDepth++;
	}
	else if (INS_IsRet(ins) == true)
	{
		trace.execution_depth = executionDepth--;
	}
	else
		trace.execution_depth = executionDepth;

	trace.instruction_count = instructionCount++;
	
#ifdef TARGET_WINDOWS
	trace.execution_time = WINDOWS::GetTickCount();
#else
	timeval time;
	gettimeofday(&time, NULL);
	unsigned long millis = (time.tv_sec * 1000) + (time.tv_usec / 1000);
	trace.execution_time = millis;
#endif

	LogStruct(trace);
}