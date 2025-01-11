package at.avollmaier.tasknest.location.domain.service

import android.app.ActivityManager
import android.content.Context
import android.os.BatteryManager
import android.os.Build
import android.os.StatFs
import android.provider.Settings
import android.provider.Settings.Secure.getString
import android.telephony.TelephonyManager
import android.util.DisplayMetrics
import android.util.Log
import android.view.WindowManager
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
import java.util.Locale
import java.util.TimeZone

class LocationBackendService(private val context: Context) {
    private val locationDao = LocationDatabase.getDatabase(context).locationDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)
    
    private val api: ILocationBackendService =
        NetworkUtils.provideRetrofit(context).create(ILocationBackendService::class.java)


    suspend fun publishOfflinePersistedLocations() {
        val offlineLocations = locationDao.getAndMarkPersisted()
        val locations = mapToLocationDto(offlineLocations, context)

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
        locationEntities: List<LocationEntity>,
        context: Context
    ): List<LocationDto> {
        val deviceMetadata = getDeviceMetadata(context)
        return locationEntities.map { locationEntity ->
            LocationDto(
                createdUtc = ZonedDateTime.ofInstant(
                    Instant.ofEpochMilli(
                        locationEntity.timestamp
                    ), TimeZone.getDefault().toZoneId()
                ), metaData = deviceMetadata,
                location = Location(
                    x = locationEntity.latitude, y = locationEntity.longitude
                )
            )
        }
    }

    fun getDeviceMetadata(context: Context): List<MetaData> {
        val metadata = mutableListOf<MetaData>()

        // Device Information
        metadata.add(MetaData("Device Model", Build.MODEL))
        metadata.add(MetaData("Manufacturer", Build.MANUFACTURER))
        metadata.add(MetaData("Brand", Build.BRAND))
        metadata.add(MetaData("Device Name", Build.DEVICE))
        metadata.add(MetaData("Product Name", Build.PRODUCT))
        metadata.add(MetaData("Hardware", Build.HARDWARE))

        // OS Information
        metadata.add(MetaData("Android Version", Build.VERSION.RELEASE))
        metadata.add(MetaData("API Level", Build.VERSION.SDK_INT.toString()))
        metadata.add(MetaData("Security Patch", Build.VERSION.SECURITY_PATCH ?: "Unknown"))

        // Unique Identifiers
        val androidId = getString(context.contentResolver, Settings.Secure.ANDROID_ID)
        metadata.add(MetaData("Android ID", androidId))

        // Battery Information
        val batteryManager = context.getSystemService(Context.BATTERY_SERVICE) as BatteryManager
        val batteryLevel = batteryManager.getIntProperty(BatteryManager.BATTERY_PROPERTY_CAPACITY)
        metadata.add(MetaData("Battery Level", "$batteryLevel%"))

        // Network Information
        val telephonyManager =
            context.getSystemService(Context.TELEPHONY_SERVICE) as TelephonyManager
        val carrierName = telephonyManager.networkOperatorName
        metadata.add(MetaData("Carrier Name", carrierName ?: "Unknown"))

        // Memory Information
        val activityManager = context.getSystemService(Context.ACTIVITY_SERVICE) as ActivityManager
        val memoryInfo = ActivityManager.MemoryInfo()
        activityManager.getMemoryInfo(memoryInfo)
        metadata.add(MetaData("Total RAM", "${memoryInfo.totalMem / (1024 * 1024)} MB"))
        metadata.add(MetaData("Available RAM", "${memoryInfo.availMem / (1024 * 1024)} MB"))

        // Display Information
        val displayMetrics = DisplayMetrics()
        val windowManager = context.getSystemService(Context.WINDOW_SERVICE) as WindowManager
        windowManager.defaultDisplay.getMetrics(displayMetrics)
        metadata.add(
            MetaData(
                "Screen Resolution",
                "${displayMetrics.widthPixels}x${displayMetrics.heightPixels}"
            )
        )
        metadata.add(MetaData("Density DPI", displayMetrics.densityDpi.toString()))

        // Storage Information
        val statFs = StatFs(context.filesDir.path)
        val totalInternalStorage = statFs.totalBytes / (1024 * 1024 * 1024)
        val availableInternalStorage = statFs.availableBytes / (1024 * 1024 * 1024)
        metadata.add(MetaData("Total Internal Storage", "$totalInternalStorage GB"))
        metadata.add(MetaData("Available Internal Storage", "$availableInternalStorage GB"))

        // Time and Locale
        metadata.add(MetaData("Time Zone", TimeZone.getDefault().id))
        metadata.add(MetaData("Locale", Locale.getDefault().toString()))

        return metadata
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