package net.mstoegerer.tasknest.repository

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import androidx.core.app.ActivityCompat
import com.google.android.gms.location.LocationServices
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.withContext
import net.mstoegerer.tasknest.database.LocationDatabase
import net.mstoegerer.tasknest.entity.LocationEntity

class LocationRepository(private val context: Context) {

    private val fusedLocationProviderClient =
        LocationServices.getFusedLocationProviderClient(context)
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()

    companion object {
        fun getCurrentLocationAndStoreInDb(locationRepository: LocationRepository) {
            if (ActivityCompat.checkSelfPermission(
                    locationRepository.context,
                    Manifest.permission.ACCESS_FINE_LOCATION
                ) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(
                    locationRepository.context,
                    Manifest.permission.ACCESS_COARSE_LOCATION
                ) != PackageManager.PERMISSION_GRANTED
            ) {
                // Request the required permissions
                // Handle permission request in your activity/fragment
                return
            }

            locationRepository.fusedLocationProviderClient.lastLocation.addOnSuccessListener { location ->
                if (location != null) {
                    val locationEntity = LocationEntity(
                        latitude = location.latitude,
                        longitude = location.longitude,
                        timestamp = System.currentTimeMillis()
                    )
                    //coroutine runs on a thread designed for IO tasks
                    CoroutineScope(Dispatchers.IO).launch {
                        locationRepository.locationDao.insertLocation(locationEntity)
                    }
                } else {
                    // Handle location not found case
                }
            }
        }

        fun getLastLocation(
            locationRepository: LocationRepository,
            callback: (LocationEntity?) -> Unit
        ) {
            CoroutineScope(Dispatchers.IO).launch {
                val lastLocation = 
            locationRepository.locationDao.getLastLocation()
                withContext(Dispatchers.Main) {
                    callback(lastLocation)
                }
            }
        }
    }
}
