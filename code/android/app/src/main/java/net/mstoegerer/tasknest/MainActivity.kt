package net.mstoegerer.tasknest

import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Log
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import com.google.android.gms.maps.MapsInitializer
import com.google.android.gms.maps.OnMapsSdkInitializedCallback
import com.google.android.material.bottomnavigation.BottomNavigationView
import net.mstoegerer.tasknest.repository.LocationRepository
import net.mstoegerer.tasknest.ui.map.MapsFragment
import net.mstoegerer.tasknest.ui.team.TeamFragment
import net.mstoegerer.tasknest.ui.today.TodayFragment

class MainActivity : AppCompatActivity(), OnMapsSdkInitializedCallback {

    private lateinit var locationRepository: LocationRepository
    private lateinit var bottomNav: BottomNavigationView

    //private lateinit var locationTextView: TextView
    private fun fetchLocation() {
        LocationRepository.getCurrentLocationAndStoreInDb(locationRepository)
//        LocationRepository.getLastLocation(locationRepository) { location ->
//            locationTextView.text = location?.toString() ?: "No location found"
//        }
    }

    override fun onMapsSdkInitialized(renderer: MapsInitializer.Renderer) {
        when (renderer) {
            MapsInitializer.Renderer.LATEST -> Log.d(
                "MapsDemo",
                "The latest version of the renderer is used."
            )

            MapsInitializer.Renderer.LEGACY -> Log.d(
                "MapsDemo",
                "The legacy version of the renderer is used."
            )
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        MapsInitializer.initialize(applicationContext, MapsInitializer.Renderer.LATEST, this)
        setContentView(R.layout.activity_main)
        bottomNav = findViewById(R.id.bottomNav)
        locationRepository = LocationRepository(this)
        //locationTextView = findViewById(R.id.location_text_view)

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
        replaceFragment(TodayFragment())

        bottomNav.setOnItemSelectedListener {
            when (it.itemId) {
                R.id.navigation_today -> replaceFragment(TodayFragment())
                R.id.navigation_map -> replaceFragment(MapsFragment())
                R.id.navigation_team -> replaceFragment(TeamFragment())
            }
            true
        }


    }

    private fun replaceFragment(fragment: Fragment) {
        val fragmentTransaction = supportFragmentManager.beginTransaction()
        fragmentTransaction.replace(R.id.frameLayout, fragment).commit()
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
