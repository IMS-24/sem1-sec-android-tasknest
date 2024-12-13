package at.avollmaier.tasknest.location.domain.service

import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.domain.config.AccessTokenInterceptor
import at.avollmaier.tasknest.location.data.Location
import at.avollmaier.tasknest.location.data.LocationDto
import at.avollmaier.tasknest.location.data.LocationEntity
import at.avollmaier.tasknest.location.data.MetaData
import at.avollmaier.tasknest.location.domain.LocationDatabase
import com.fasterxml.jackson.databind.DeserializationFeature
import com.fasterxml.jackson.databind.MapperFeature
import com.fasterxml.jackson.databind.ObjectMapper
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.jackson.JacksonConverterFactory
import java.time.Instant
import java.time.LocalDateTime
import java.util.TimeZone
import java.util.concurrent.TimeUnit


class LocationBackendService(context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)


    private val objectMapper = ObjectMapper().apply {

        registerModule(JavaTimeModule())
        configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
        enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS);
    }
    private val retrofit: Retrofit =
        Retrofit.Builder().baseUrl(context.getString(R.string.backend_url))
            .client(provideAccessOkHttpClient(AccessTokenInterceptor(context)))
            .addConverterFactory(JacksonConverterFactory.create(objectMapper))
            .build()

    private val api: ILocationBackendService = retrofit.create(ILocationBackendService::class.java)

    private fun provideAccessOkHttpClient(
        accessTokenInterceptor: AccessTokenInterceptor
    ): OkHttpClient {
        val loggingInterceptor = HttpLoggingInterceptor()
        loggingInterceptor.level = HttpLoggingInterceptor.Level.BODY
        return OkHttpClient.Builder().addInterceptor(accessTokenInterceptor)
            .addInterceptor(loggingInterceptor).connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS).writeTimeout(30, TimeUnit.SECONDS).build()
    }

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
                createdUtc = LocalDateTime.ofInstant(
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