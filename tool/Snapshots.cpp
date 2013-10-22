#include "Snapshots.h"
#include "OsDefines.h"
#include "RegionMonitoring.h"

int memDumpCount = 1;

#ifdef TARGET_WINDOWS

WINDOWS::HANDLE hSnapshotEvent = 0;

#else
namespace POSIX
{
	#include <semaphore.h>
	#include <pthread.h>
	#include <semaphore.h>
	#include <time.h>

	sem_t *sSnapshotSem = 0;
	bool bSnapshotting = false;

}
#endif

bool TakeSnapshot()
{
	char filename[1024];
	memset(filename, 0, 1024);
	sprintf(filename, "memorydump.%d.dmp", memDumpCount++);
	DumpAllMemoryRegions(filename);
	return true;
}

bool CheckSnapshotEvent()
{
#ifdef TARGET_WINDOWS
	// Check the handle but return immediately
	WINDOWS::DWORD result = WINDOWS::WaitForSingleObject(hSnapshotEvent, 0);

	if (result == 0) // WAIT_OBJECT_0
		return true;
	else
		return false;
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	bool result = POSIX::bSnapshotting;
	POSIX::sem_post(POSIX::sSnapshotSem);
	return result;
#endif
}

void EnableSnapshotEvent()
{
#ifdef TARGET_WINDOWS
	WINDOWS::SetEvent(hSnapshotEvent);
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	POSIX::bSnapshotting = true;
	POSIX::sem_post(POSIX::sSnapshotSem);
#endif
}

void DisableSnapshotEvent()
{
#ifdef TARGET_WINDOWS
	WINDOWS::ResetEvent(hSnapshotEvent);
#else
	POSIX::sem_wait(POSIX::sSnapshotSem);
	POSIX::bSnapshotting = false;
	POSIX::sem_post(POSIX::sSnapshotSem);
#endif
}

void InitSnapshottingEvent()
{
#ifdef TARGET_WINDOWS
	int error = 0;
	
	hSnapshotEvent = WINDOWS::OpenEvent(
		EVENT_ALL_ACCESS,
		false,
		"SnapshotEvent"
		);

	error = WINDOWS::GetLastError();
	if (hSnapshotEvent == 0 && error == 2)
	{
		hSnapshotEvent = WINDOWS::CreateEvent(
			NULL,               // default security attributes
			TRUE,               // manual-reset event
			FALSE,              // initial state is nonsignaled
			TEXT("SnapshotEvent")  // object name
			);
	}
#else
	// Create the semaphores
	POSIX::sSnapshotSem = POSIX::sem_open("SNAPSHOT_SEMAPHORE", O_CREAT, S_IRUSR | S_IWUSR | S_IRGRP | S_IWGRP | S_IROTH | S_IWOTH, 0);
	POSIX::sem_init(POSIX::sSnapshotSem, 1, 0); // shared, not signaled

#endif
}

void DumpAllMemoryRegions(char* filename);