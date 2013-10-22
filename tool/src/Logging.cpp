#include "Logging.h"

#include <stdlib.h>
#include <string.h>

FILE *resultsFile = 0;
FILE *instructionLogFile = 0;

logObject logger;
logObject libraryLogger;
std::ofstream TraceFile;

bool LogMessage(logObject *loggerObject, char* message)
{
	if (message == 0 || message[0] == 0)
	{
		return false;
	}
	if (loggerObject->buffer == 0)
	{
		loggerObject->buffer = malloc(1024 * 1024); // 1 MB log by default
		memset(loggerObject->buffer, 0, 1024 * 1024);
		loggerObject->maxSize = 1024 * 1024;
	}

	unsigned int length = strlen(message);

	//printf("log size: %d/%d\n", loggerObject->usedSize, loggerObject->maxSize);
	if (loggerObject->usedSize + length >= loggerObject->maxSize)
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
	if (loggerObject == 0)
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

void FinishFunction(int code, void* data)
{
	if (resultsFile != 0)
		fclose(resultsFile);

	if (instructionLogFile != 0)
		fclose(instructionLogFile);

	CloseLog(&logger);
	CloseLog(&libraryLogger);

	TraceFile.close();
}