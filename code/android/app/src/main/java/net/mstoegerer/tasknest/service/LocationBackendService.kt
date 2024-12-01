package net.mstoegerer.tasknest.service

import ILocationBackendService
import android.content.Context
import android.util.Log
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.database.LocationDatabase
import net.mstoegerer.tasknest.entity.LocationEntity
import okhttp3.OkHttpClient
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory

class LocationBackendService(private val context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(context.getString(R.string.backend_url))
        .client(OkHttpClient.Builder().build())
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    private val api: ILocationBackendService = retrofit.create(ILocationBackendService::class.java)

    suspend fun publishOfflinePersistedLocations() {
        val locations = locationDao.getAndMarkPersisted()

        ioScope.launch {
            try {
                val response = api.postLocations(locations).execute()
                if (response.isSuccessful) {
                    Log.d("LocationService", "Locations sent to backend: $locations")
                } else {
                    Log.e(
                        "LocationService",
                        "Failed to send locations: ${response.errorBody()?.string()}"
                    )
                    restorePersistedFlag(locations)
                }
            } catch (e: Exception) {
                Log.e("LocationService", "Exception while sending locations", e)
                restorePersistedFlag(locations)
            }
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