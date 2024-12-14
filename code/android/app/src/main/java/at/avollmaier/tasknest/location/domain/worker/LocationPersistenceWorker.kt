package at.avollmaier.tasknest.location.domain.worker

import android.content.Context
import androidx.work.CoroutineWorker
import androidx.work.ExistingWorkPolicy
import androidx.work.OneTimeWorkRequestBuilder
import androidx.work.WorkManager
import androidx.work.WorkerParameters
import at.avollmaier.tasknest.location.domain.service.LocationBackendService
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.withContext
import java.util.concurrent.TimeUnit.MINUTES

class LocationPersistenceWorker(
    context: Context,
    params: WorkerParameters,
) : CoroutineWorker(context, params) {

    override suspend fun doWork(): Result {
        return withContext(Dispatchers.IO) {
            val locationBackendService = LocationBackendService(applicationContext)

            val nextWorkRequest = OneTimeWorkRequestBuilder<LocationPersistenceWorker>()
                .setInitialDelay(1, MINUTES)
                .build()

            WorkManager.getInstance(applicationContext).enqueueUniqueWork(
                "LocationPersistenceCoroutineWorker",
                ExistingWorkPolicy.APPEND,
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