package at.avollmaier.tasknest.location.domain.service

import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.common.NetworkUtils
import at.avollmaier.tasknest.location.data.Location
import at.avollmaier.tasknest.location.data.LocationDto
import at.avollmaier.tasknest.location.data.LocationEntity
import at.avollmaier.tasknest.location.data.MetaData
import at.avollmaier.tasknest.location.domain.LocationDatabase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.time.Instant
import java.time.ZonedDateTime
import java.util.TimeZone


class LocationBackendService(context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)


    private val api: ILocationBackendService =
        NetworkUtils.provideRetrofit(context).create(ILocationBackendService::class.java)


    suspend fun publishOfflinePersistedLocations() {
        val offlineLocations = locationDao.getAndMarkPersisted()
        val locations = mapToLocationDto(offlineLocations)

        ioScope.launch {
            try {
                val response = api.postLocations(locations).execute()
                if (response.isSuccessful) {
                    Log.d("LocationService", "Locations sent to backend: $locations")
                } else {
                    Log.e(
                        "LocationService", "Failed to send locations: ${response.raw()}"
                    )
                    restorePersistedFlag(offlineLocations)
                }
            } catch (e: Exception) {
                Log.e("LocationService", "Exception while sending locations", e)
                restorePersistedFlag(offlineLocations)
            }
        }
    }


    private fun mapToLocationDto(
        locationEntities: List<LocationEntity>
    ): List<LocationDto> {
        return locationEntities.map { locationEntity ->
            LocationDto(
                createdUtc = ZonedDateTime.ofInstant(
                    Instant.ofEpochMilli(
                        locationEntity.timestamp
                    ), TimeZone.getDefault().toZoneId()
                ), metaData = listOf(
                    MetaData(
                        key = "fdsa", value = "fdsa"
                    )
                ), location = Location(
                    x = locationEntity.latitude, y = locationEntity.longitude
                )
            )
        }
    }

    private fun restorePersistedFlag(locations: List<LocationEntity>) {
        for (location in locations) {
            location.persisted = false
        }
        ioScope.launch {
            locationDao.updateLocations(locations)
        }
    }
}