import android.Manifest
import android.annotation.SuppressLint
import android.content.Context
import android.content.pm.PackageManager
import android.util.Log
import androidx.core.app.ActivityCompat
import com.google.android.gms.location.LocationServices.getFusedLocationProviderClient
import com.google.gson.Gson
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.delay
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.callback.CronetCallback
import net.mstoegerer.tasknest.database.LocationDatabase
import net.mstoegerer.tasknest.entity.LocationEntity
import net.mstoegerer.tasknest.util.PropertiesReader
import org.chromium.net.CronetEngine
import org.chromium.net.apihelpers.UploadDataProviders
import java.nio.charset.StandardCharsets
import java.util.concurrent.Executors

class LocationService(private val context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)
    private val gson = Gson()
    private val cronetEngine: CronetEngine = CronetEngine.Builder(context).build()
    private val propertiesReader = PropertiesReader(context)

    @SuppressLint("MissingPermission")
    fun fetchAndStoreCurrentLocation(locationService: LocationService) {
        if (!locationService.hasLocationPermission()) return

        getFusedLocationProviderClient(context).lastLocation.addOnSuccessListener { location ->
            location?.let {
                val locationEntity = LocationEntity(
                    latitude = it.latitude,
                    longitude = it.longitude,
                    timestamp = System.currentTimeMillis()
                )
                locationService.ioScope.launch {
                    locationService.locationDao.insertLocation(locationEntity)
                    Log.d("LocationService", "New location stored: $locationEntity")
                }
            } ?: run {
                // Handle location not found case
            }
        }
    }

    private fun hasLocationPermission(): Boolean {
        return ActivityCompat.checkSelfPermission(
            context, Manifest.permission.ACCESS_FINE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(
            context, Manifest.permission.ACCESS_COARSE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED
    }

    private fun markLocationAsPersisted(locationEntitys: List<LocationEntity>) {
        locationEntitys.forEach {
            it.persisted = true
        }
        ioScope.launch {
            locationDao.updateLocations(locationEntitys)
        }
    }

    suspend fun publishUnpersistedLocations() {
        val locations = locationDao.getAndMarkPersisted()

        val json = gson.toJson(locations)
        val backendUrl = propertiesReader.getProperty("env.backend.url") + "/locations"
        val requestBuilder = cronetEngine.newUrlRequestBuilder(
            backendUrl,
            CronetCallback(onSuccess = {
                Log.d("LocationService", "Locations sent to backend: $locations")
            }, onFailure = {

                Log.e(
                    "LocationService",
                    "Failed to send locations to ${backendUrl}: $locations - Restoring persisted flag"
                )
                for (location in locations) {
                    location.persisted = false
                }
                ioScope.launch {
                    locationDao.updateLocations(locations)
                }
            }),
            Executors.newSingleThreadExecutor()
        )

        val request =
            requestBuilder
                .setHttpMethod("POST")
                .addHeader("Content-Type", "application/json")
                .setUploadDataProvider(
                    UploadDataProviders.create(json.toByteArray(StandardCharsets.UTF_8)),
                    Executors.newSingleThreadExecutor()
                ).build()

        Log.d("LocationService", "Sending locations to backend: $json")

        request.start()
    }
}