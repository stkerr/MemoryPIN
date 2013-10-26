#ifndef __LOGGING__
#define __LOGGING__

#include <stdio.h>
#include <fstream>

#include "OsDefines.h"
#include "InstructionTracer.h"

extern FILE *resultsFile;
extern FILE *instructionLogFile;

typedef struct logObject_t
{
	char filename[1024];
	void* buffer;
	unsigned int usedSize;
	unsigned int maxSize;
} logObject;

extern logObject logger;
extern logObject libraryLogger;
extern std::ofstream TraceFile;

bool LogMessage(logObject *loggerObject, std::string message);
bool LogMessage(logObject *loggerObject, char* message);
bool CloseLog(logObject *loggerObject);

void FinishFunction(int code, void* data);

bool LogStruct(instruction_trace t);

#endif