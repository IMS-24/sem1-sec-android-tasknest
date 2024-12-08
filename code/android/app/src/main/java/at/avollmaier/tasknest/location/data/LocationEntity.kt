package at.avollmaier.tasknest.location.data

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "locations")
data class LocationEntity(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    val latitude: Double,
    val longitude: Double,
    val timestamp: Long,
    var persisted: Boolean = false


) {
    override fun toString(): String {
        return "id=$id,\ntimestamp=$timestamp\nlatitude=$latitude,\nlongitude=$longitude"
    }
}