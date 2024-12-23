package at.avollmaier.tasknest.location.domain.worker

import android.content.Context
import android.location.Location
import android.util.Log
import androidx.work.CoroutineWorker
import androidx.work.ExistingPeriodicWorkPolicy
import androidx.work.PeriodicWorkRequestBuilder
import androidx.work.WorkManager
import androidx.work.WorkerParameters
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.location.domain.service.LocationDatabaseService
import at.avollmaier.tasknest.todo.domain.service.TodoService
import java.util.concurrent.TimeUnit

class LocationCheckWorker(
    context: Context,
    workerParams: WorkerParameters,
) : CoroutineWorker(context, workerParams) {


    override suspend fun doWork(): Result {
        val location = LocationDatabaseService(applicationContext).getCurrentLocation()

        val todoService = TodoService(applicationContext)


        todoService.getNewTodos(0, Int.MAX_VALUE) { result ->
            result.forEach { todo ->
                val todoLocation = todo.location
                if (isWithinRange(
                        location.x,
                        location.y,
                        todoLocation.x,
                        todoLocation.y
                    )
                ) {
                    todoService.finishTodo(
                        todo.id,
                        callback = {
                            Log.e("LocationCheckWorker", "Finished todo with id ${todo.id}")
                        }
                    )
                }
            }
        }


        return Result.success()
    }

    private fun isWithinRange(
        lat1: Double,
        lon1: Double,
        lat2: Double,
        lon2: Double,
        range: Double = applicationContext.resources.getString(R.string.todo_location_radius)
            .toDouble()
    ): Boolean {
        val results = FloatArray(1)
        Location.distanceBetween(lat1, lon1, lat2, lon2, results)
        return results[0] <= range
    }

    companion object {
        fun schedule(current: Context) {
            val locationWorkRequest = PeriodicWorkRequestBuilder<LocationCheckWorker>(
                15,
                TimeUnit.MINUTES
            ).build()

            WorkManager.getInstance(current).enqueueUniquePeriodicWork(
                "LocationCheckWorker",
                ExistingPeriodicWorkPolicy.CANCEL_AND_REENQUEUE,
                locationWorkRequest
            )
        }
    }
}
