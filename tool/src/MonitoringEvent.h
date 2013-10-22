#ifndef __EVENT_SIGNALING__
#define __EVENT_SIGNALING__

void InitMonitoringEvent();

bool CheckMonitoringEvent();
bool CheckSnapshotEvent();

void EnableMonitoringEvent();
void EnableSnapshotEvent();

void DisableMonitoringEvent();
void DisableSnapshotEvent();

#endif