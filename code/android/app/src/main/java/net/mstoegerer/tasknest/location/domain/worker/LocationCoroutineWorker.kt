package net.mstoegerer.tasknest.location.domain.worker

import net.mstoegerer.tasknest.location.domain.service.LocationDatabaseService
import android.content.Context
import androidx.work.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.util.concurrent.TimeUnit

class LocationCoroutineWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {

    override suspend fun doWork(): Result {
        return withContext(Dispatchers.IO) {
            val locationDatabaseService = LocationDatabaseService(applicationContext)
            locationDatabaseService.fetchAndStoreCurrentLocation(locationDatabaseService)

            // Schedule the next work
            val nextWorkRequest = OneTimeWorkRequestBuilder<LocationCoroutineWorker>()
                .setInitialDelay(10, TimeUnit.SECONDS)
                .build()

            WorkManager.getInstance(applicationContext).enqueueUniqueWork(
                "LocationCoroutineWorker",
                ExistingWorkPolicy.REPLACE,
                nextWorkRequest
            )

            Result.success()
        }
    }
}