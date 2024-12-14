package at.avollmaier.tasknest.location.domain.worker

import android.content.Context
import androidx.work.CoroutineWorker
import androidx.work.ExistingWorkPolicy
import androidx.work.OneTimeWorkRequestBuilder
import androidx.work.WorkManager
import androidx.work.WorkerParameters
import at.avollmaier.tasknest.location.domain.service.LocationDatabaseService
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
            locationDatabaseService.fetchAndStoreCurrentLocation()

            val nextWorkRequest = OneTimeWorkRequestBuilder<LocationCoroutineWorker>()
                .setInitialDelay(10, TimeUnit.SECONDS)
                .build()

            WorkManager.getInstance(applicationContext).enqueueUniqueWork(
                "LocationCoroutineWorker",
                ExistingWorkPolicy.APPEND,
                nextWorkRequest
            )

            Result.success()
        }
    }

    companion object {
        fun schedule(current: Context) {
            val locationWorkRequest = OneTimeWorkRequestBuilder<LocationCoroutineWorker>()
                .setInitialDelay(10, TimeUnit.SECONDS)
                .build()

            WorkManager.getInstance(current).enqueueUniqueWork(
                "LocationCoroutineWorker",
                ExistingWorkPolicy.REPLACE,
                locationWorkRequest
            )
        }
    }
}