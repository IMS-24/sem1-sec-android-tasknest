package at.avollmaier.tasknest.todo.data

import com.fasterxml.jackson.annotation.JsonCreator
import com.fasterxml.jackson.annotation.JsonProperty

data class PointDto @JsonCreator constructor(
    @JsonProperty("x") val x: Double,
    @JsonProperty("y") val y: Double
)