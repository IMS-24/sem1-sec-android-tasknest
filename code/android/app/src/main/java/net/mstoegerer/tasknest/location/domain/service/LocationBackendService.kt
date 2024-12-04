package net.mstoegerer.tasknest.location.domain.service

import android.content.Context
import android.os.Build
import android.util.Log
import androidx.annotation.RequiresApi
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.storage.SecureCredentialsManager
import com.auth0.android.authentication.storage.SharedPreferencesStorage
import com.google.gson.Gson
import com.google.gson.GsonBuilder
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.auth.domain.config.AccessTokenInterceptor
import net.mstoegerer.tasknest.auth.domain.service.AuthService
import net.mstoegerer.tasknest.location.data.Location
import net.mstoegerer.tasknest.location.data.LocationDto
import net.mstoegerer.tasknest.location.data.LocationEntity
import net.mstoegerer.tasknest.location.data.MetaData
import net.mstoegerer.tasknest.location.domain.LocationDatabase
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.time.Instant
import java.util.Date
import java.util.concurrent.TimeUnit


class LocationBackendService(context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)
    private val account: Auth0 = Auth0(
        context.getString(R.string.com_auth0_client_id),
        context.getString(R.string.com_auth0_domain)
    )
    private val apiClient = AuthenticationAPIClient(account)
    private val credentialsManager =
        SecureCredentialsManager(context, apiClient, SharedPreferencesStorage(context))

    private var gson: Gson = GsonBuilder()
        .setDateFormat("yyyy-MM-dd'T'HH:mm:ss")
        .create()

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(context.getString(R.string.backend_url))
        .client(provideAccessOkHttpClient(AccessTokenInterceptor(credentialsManager)))
        .addConverterFactory(GsonConverterFactory.create(gson))
        .build()

    private val api: ILocationBackendService = retrofit.create(ILocationBackendService::class.java)

    private fun provideAccessOkHttpClient(
        accessTokenInterceptor: AccessTokenInterceptor
    ): OkHttpClient {
        val loggingInterceptor = HttpLoggingInterceptor()
        loggingInterceptor.level = HttpLoggingInterceptor.Level.BODY
        return OkHttpClient.Builder()
            .addInterceptor(accessTokenInterceptor)
            .addInterceptor(loggingInterceptor)
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .build()
    }

    @RequiresApi(Build.VERSION_CODES.O)
    suspend fun publishOfflinePersistedLocations() {
        val offlineLocations = locationDao.getAndMarkPersisted()
        val locations = AuthService.fetchUserProfile(credentialsManager, apiClient).getId()
            ?.let { mapToLocationDto(it, offlineLocations) } ?: emptyList()

        ioScope.launch {
            try {
                val response = api.postLocations(locations).execute()
                if (response.isSuccessful) {
                    Log.d("LocationService", "Locations sent to backend: $locations")
                } else {
                    Log.e(
                        "LocationService",
                        "Failed to send locations: ${response.raw()}"
                    )
                    restorePersistedFlag(offlineLocations)
                }
            } catch (e: Exception) {
                Log.e("LocationService", "Exception while sending locations", e)
                restorePersistedFlag(offlineLocations)
            }
        }
    }


    @RequiresApi(Build.VERSION_CODES.O)
    fun mapToLocationDto(
        userId: String,
        locationEntities: List<LocationEntity>
    ): List<LocationDto> {
        return locationEntities.map { locationEntity ->
            LocationDto(
                userId = userId,
                createdUtc = Date.from(Instant.ofEpochMilli(locationEntity.timestamp)),
                metaData =    listOf(
                    MetaData(
                        key = "fdsa",
                        value = "fdsa"
                    )
                ),
                location = Location(
                    x = locationEntity.latitude,
                    y = locationEntity.longitude
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