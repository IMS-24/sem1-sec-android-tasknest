package net.mstoegerer.tasknest

import LocationService
import android.content.pm.PackageManager
import android.os.Bundle
import android.util.Log
import androidx.appcompat.app.AppCompatActivity
import androidx.core.app.ActivityCompat
import androidx.core.content.ContextCompat
import androidx.fragment.app.Fragment
import com.google.android.gms.maps.MapsInitializer
import com.google.android.gms.maps.OnMapsSdkInitializedCallback
import androidx.work.*
import com.google.android.material.bottomnavigation.BottomNavigationView
import net.mstoegerer.tasknest.ui.map.MapsFragment
import net.mstoegerer.tasknest.ui.team.TeamFragment
import net.mstoegerer.tasknest.ui.today.TodayFragment
import net.mstoegerer.tasknest.worker.LocationCoroutineWorker
import net.mstoegerer.tasknest.worker.LocationPersistenceWorker
import java.util.concurrent.TimeUnit

class MainActivity : AppCompatActivity(), OnMapsSdkInitializedCallback {

    private lateinit var locationService: LocationService
    private lateinit var bottomNav: BottomNavigationView
    private val LOCATION_PERMISSION_REQUEST_CODE = 1

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
        initializeComponents()

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
            scheduleLocationWorkers()
        }
    }

    private fun initializeComponents() {
        bottomNav = findViewById(R.id.bottomNav)
        locationService = LocationService(this)

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
        supportFragmentManager.beginTransaction()
            .replace(R.id.frameLayout, fragment)
            .commit()
    }

    private fun scheduleLocationWorkers() {
        val locationCoroutineWorkRequest = OneTimeWorkRequestBuilder<LocationCoroutineWorker>()
            .setInitialDelay(10, TimeUnit.SECONDS)
            .build()

        val locationPersistenceWorkRequest = OneTimeWorkRequestBuilder<LocationPersistenceWorker>()
            .setInitialDelay(1, TimeUnit.MINUTES)
            .build()

        WorkManager.getInstance(this).enqueueUniqueWork(
            "LocationCoroutineWorker",
            ExistingWorkPolicy.REPLACE,
            locationCoroutineWorkRequest
        )

        WorkManager.getInstance(this).enqueueUniqueWork(
            "LocationPersistenceWorker",
            ExistingWorkPolicy.REPLACE,
            locationPersistenceWorkRequest
        )
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
            scheduleLocationWorkers()
        }
    }
}
