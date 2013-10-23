#ifndef __INSTRUCTION_TRACE__
#define __INSTRUCTION_TRACE__

#include "OsDefines.h"

typedef struct _instruction_trace_t
{
	int tid;
	int address;
	int execution_depth;
	int execution_time;
	int instruction_count;
	char library_name[260];
} instruction_trace;

void InstructionTrace(INS ins, void* v);

#endif