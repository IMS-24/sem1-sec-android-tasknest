package net.mstoegerer.tasknest.worker

import LocationService
import android.content.Context
import androidx.work.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.util.concurrent.TimeUnit

class LocationPersistenceWorker(
    context: Context,
    params: WorkerParameters
) : CoroutineWorker(context, params) {

    override suspend fun doWork(): Result {
        return withContext(Dispatchers.IO) {
            val locationService = LocationService(applicationContext)

            // Schedule the next work
            val nextWorkRequest = OneTimeWorkRequestBuilder<LocationPersistenceWorker>()
                .setInitialDelay(1, TimeUnit.MINUTES)
                .build()

            WorkManager.getInstance(applicationContext).enqueueUniqueWork(
                "LocationPersistenceCoroutineWorker",
                ExistingWorkPolicy.REPLACE,
                nextWorkRequest
            )

            locationService.publishUnpersistedLocations()

            Result.success()
        }
    }
}