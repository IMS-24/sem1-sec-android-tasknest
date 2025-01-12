package at.avollmaier.tasknest.location.domain.service

import android.Manifest
import android.annotation.SuppressLint
import android.content.Context
import android.content.pm.PackageManager
import android.util.Log
import androidx.core.app.ActivityCompat
import at.avollmaier.tasknest.location.data.Location
import at.avollmaier.tasknest.location.data.LocationEntity
import at.avollmaier.tasknest.location.domain.LocationDatabase
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await

class LocationDatabaseService(private val context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)
    private val fusedLocationClient: FusedLocationProviderClient =
        LocationServices.getFusedLocationProviderClient(context)

    private fun hasLocationPermission(): Boolean {
        return ActivityCompat.checkSelfPermission(
            context,
            Manifest.permission.ACCESS_FINE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED || ActivityCompat.checkSelfPermission(
            context,
            Manifest.permission.ACCESS_COARSE_LOCATION
        ) == PackageManager.PERMISSION_GRANTED
    }

    @SuppressLint("MissingPermission")
    suspend fun getCurrentLocation(): Location {
        if (!hasLocationPermission()) {
            throw IllegalStateException("Location permission not granted")
        }
        val location = fusedLocationClient.lastLocation.await()
        return location.let { Location(it.latitude, it.longitude) }
    }

    @SuppressLint("MissingPermission")
    fun fetchAndStoreCurrentLocation() {
        if (!hasLocationPermission()) return

        fusedLocationClient.lastLocation.addOnSuccessListener { location ->
            location?.let {
                val locationEntity = LocationEntity(
                    latitude = it.latitude,
                    longitude = it.longitude,
                    timestamp = System.currentTimeMillis()
                )
                ioScope.launch {
                    locationDao.insertLocation(locationEntity)
                }
            } ?: run {
                Log.w("LocationService", "Location not found")
            }
        }
    }
}