#ifndef __CONFIGURATION__
#define __CONFIGURATION__

// This will check, at runtime (i.e. slow), what library the instruction resides in
const bool CHECK_LIBRARY_NAMES = false;

// This will perform a function callback whenever a library is loaded
const bool USE_IMAGELOADED = true;

// This will cause a trace event to occur for every basic block
const bool USE_BBLTRACE = false;

#endif