package at.avollmaier.tasknest.location.data

import java.time.ZonedDateTime

data class LocationDto(
    val createdUtc: ZonedDateTime,
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