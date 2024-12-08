package at.avollmaier.tasknest.location.domain.worker

import android.content.Context
import androidx.work.*
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import at.avollmaier.tasknest.location.domain.service.LocationBackendService
import java.util.concurrent.TimeUnit
import java.util.concurrent.TimeUnit.MINUTES
import java.util.concurrent.TimeUnit.SECONDS

class LocationPersistenceWorker(
    context: Context,
    params: WorkerParameters,
) : CoroutineWorker(context, params) {

    override suspend fun doWork(): Result {
        return withContext(Dispatchers.IO) {
            val locationBackendService = LocationBackendService(applicationContext)

            // Schedule the next work
            val nextWorkRequest = OneTimeWorkRequestBuilder<LocationPersistenceWorker>()
                .setInitialDelay(1, TimeUnit.MINUTES)
                .build()

            WorkManager.getInstance(applicationContext).enqueueUniqueWork(
                "LocationPersistenceCoroutineWorker",
                ExistingWorkPolicy.REPLACE,
                nextWorkRequest
            )

            locationBackendService.publishOfflinePersistedLocations()

            Result.success()
        }
    }

    companion object {
        fun schedule(context: Context) {
            val locationPersistenceWorkRequest =
                OneTimeWorkRequestBuilder<LocationPersistenceWorker>()
                    .setInitialDelay(1, MINUTES)
                    .build()

            WorkManager.getInstance(context).enqueueUniqueWork(
                "LocationPersistenceWorker",
                ExistingWorkPolicy.REPLACE,
                locationPersistenceWorkRequest
            )
        }
    }
}