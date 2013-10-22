#include "BasicBlockTracer.h"
#include "Logging.h"

UINT32 count_trace = 0; // current trace number

void traceBBL(const string *s)
{
	TraceFile.write(s->c_str(), s->size());
}

VOID BBLTrace(TRACE trace, VOID *v)
{
	for (BBL bbl = TRACE_BblHead(trace); BBL_Valid(bbl); bbl = BBL_Next(bbl))
	{
		string traceString = "";

		for (INS ins = BBL_InsHead(bbl); INS_Valid(ins); ins = INS_Next(ins))
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
