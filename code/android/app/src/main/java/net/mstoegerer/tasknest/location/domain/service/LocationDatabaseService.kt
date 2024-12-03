package net.mstoegerer.tasknest.location.domain.service

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import android.util.Log
import androidx.core.app.ActivityCompat
import com.google.android.gms.location.LocationServices.getFusedLocationProviderClient
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.location.domain.LocationDatabase
import net.mstoegerer.tasknest.location.data.LocationEntity

class LocationDatabaseService(private val context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)

    fun fetchAndStoreCurrentLocation(locationDatabaseService: LocationDatabaseService) {
        if (ActivityCompat.checkSelfPermission(
                context,
                Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED && ActivityCompat.checkSelfPermission(
                context,
                Manifest.permission.ACCESS_COARSE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) return

        getFusedLocationProviderClient(context).lastLocation.addOnSuccessListener { location ->
            location?.let {
                val locationEntity = LocationEntity(
                    latitude = it.latitude,
                    longitude = it.longitude,
                    timestamp = System.currentTimeMillis()
                )
                locationDatabaseService.ioScope.launch {
                    locationDatabaseService.locationDao.insertLocation(locationEntity)
                    Log.d("LocationService", "New location stored: $locationEntity")
                }
            } ?: run {
                // Handle location not found case
            }
        }
    }
}