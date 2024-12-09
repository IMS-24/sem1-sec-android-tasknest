package at.avollmaier.tasknest.location.data

import java.time.LocalDateTime

data class LocationDto(
    val createdUtc: LocalDateTime,
    val metaData: List<MetaData>,
    val location: Location
)

data class MetaData(
    val key: String,
    val value: String
)

data class Location(
    val x: Double,
    val y: Double
)