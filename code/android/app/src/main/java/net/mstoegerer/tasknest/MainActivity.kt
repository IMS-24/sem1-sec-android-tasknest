package net.mstoegerer.tasknest

import android.content.pm.PackageManager
import android.os.Bundle
import android.widget.TextView
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import net.mstoegerer.tasknest.repository.LocationRepository

class MainActivity : AppCompatActivity() {

    private lateinit var locationRepository: LocationRepository

    private lateinit var locationTextView: TextView
    private fun fetchLocation() {
        LocationRepository.getCurrentLocationAndStoreInDb(locationRepository)
        LocationRepository.getLastLocation(locationRepository) { location ->
            locationTextView.text = location?.toString() ?: "No location found"
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_main)

        locationRepository = LocationRepository(this)
        locationTextView = findViewById(R.id.location_text_view)
        if (ContextCompat.checkSelfPermission(
                this,
                android.Manifest.permission.ACCESS_FINE_LOCATION
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            ActivityCompat.requestPermissions(
                this,
                arrayOf(android.Manifest.permission.ACCESS_FINE_LOCATION),
                LOCATION_PERMISSION_REQUEST_CODE
            )
        } else {
            fetchLocation()
        }
    }

    //Static constant for location permission request code
    companion object {
        const val LOCATION_PERMISSION_REQUEST_CODE = 1
    }

    override fun onRequestPermissionsResult(
        requestCode: Int,
        permissions: Array<out String>,
        grantResults: IntArray
    ) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults)
        if (requestCode == LOCATION_PERMISSION_REQUEST_CODE &&
            grantResults.isNotEmpty() && grantResults[0] == PackageManager.PERMISSION_GRANTED
        ) {
            fetchLocation()
        }
    }
}
