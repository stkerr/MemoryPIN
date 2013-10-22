#include "MonitoringEvent.h"
#include "OsDefines.h"


#ifdef TARGET_WINDOWS
	WINDOWS::HANDLE hMonitoringEvent = 0;
#else
	namespace POSIX
	{
		#include <semaphore.h>
		#include <pthread.h>
		#include <semaphore.h>
		#include <time.h>
		sem_t *sMonitoringSem = 0;

		bool bMonitoring = true;

	}
#endif
	
bool CheckMonitoringEvent()
{
#ifdef TARGET_WINDOWS
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hMonitoringEvent, 0);

	if (result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	bool result = POSIX::bMonitoring;
	POSIX::sem_post(POSIX::sMonitoringSem);
	return result;
#endif

}



void EnableMonitoringEvent()
{
#ifdef TARGET_WINDOWS
	// signal the event
	WINDOWS::SetEvent(hMonitoringEvent);
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	POSIX::bMonitoring = true;
	POSIX::sem_post(POSIX::sMonitoringSem);
#endif
}


void DisableMonitoringEvent()
{
#ifdef TARGET_WINDOWS
	WINDOWS::ResetEvent(hMonitoringEvent);
#else
	POSIX::sem_wait(POSIX::sMonitoringSem);
	POSIX::bMonitoring = false;
	POSIX::sem_post(POSIX::sMonitoringSem);
#endif
}



void InitMonitoringEvent()
{
#ifdef TARGET_WINDOWS
	int error = 0;
	hMonitoringEvent = WINDOWS::OpenEvent(
		EVENT_ALL_ACCESS,
		false,
		"MonitoringEvent"
		);

	error = WINDOWS::GetLastError();
	if (hMonitoringEvent == 0 && error == 2)
	{
		hMonitoringEvent = WINDOWS::CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("MonitoringEvent")  // object name
			);
	}
#else
	// Create the semaphores
	POSIX::sMonitoringSem = POSIX::sem_open("MONITORING_SEMAPHORE", O_CREAT, S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP | S_IROTH | S_IWOTH, 0);
	POSIX::sem_init(POSIX::sMonitoringSem, 1, 0); // shared, not signaled
#endif
}
