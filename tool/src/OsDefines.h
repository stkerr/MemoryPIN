#ifndef __OS_DEFINES__
#define __OS_DEFINES__

#include "pin.H"
#include "Configuration.h"

#ifdef TARGET_WINDOWS
	
	#define snprintf _snprintf

/*
This namespace would normally cause an error, since you can't
namespace standard headers (in general), but when pin.H is included,
it stomps on some of the global namespace defines, so it is necessary
to namespace windows.h into a new namespace.

Without pin.H included, this WILL cause all sorts of terrible compiler errors
in sourceannotations.h
*/
	namespace WINDOWS
	{
		#include <Windows.h>		
	}

	
#endif

#endif