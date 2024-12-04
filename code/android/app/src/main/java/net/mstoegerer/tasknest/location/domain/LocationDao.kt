package net.mstoegerer.tasknest.location.domain

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import androidx.room.Transaction
import net.mstoegerer.tasknest.location.data.LocationEntity

@Dao
interface LocationDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertLocation(location: LocationEntity)

    @Query("SELECT * FROM locations WHERE persisted = 0 ORDER BY timestamp DESC")
    suspend fun getOfflinePersistedLocations(): List<LocationEntity>

    @Query("SELECT * FROM locations ORDER BY timestamp DESC LIMIT 1")
    suspend fun getLastLocation(): LocationEntity

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun updateLocations(location: List<LocationEntity>)

    @Transaction
    suspend fun getAndMarkPersisted(): List<LocationEntity> {
        val locations = getOfflinePersistedLocations()
        locations.forEach { it.persisted = true }
        updateLocations(locations)
        return locations
    }
}