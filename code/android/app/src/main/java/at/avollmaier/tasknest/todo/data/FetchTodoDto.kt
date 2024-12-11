package at.avollmaier.tasknest.todo.data

import com.fasterxml.jackson.annotation.JsonCreator
import com.fasterxml.jackson.annotation.JsonProperty
import java.time.LocalDateTime
import java.util.UUID

data class FetchTodoDto @JsonCreator constructor(
    @JsonProperty("id") val id: UUID,
    @JsonProperty("title") val title: String,
    @JsonProperty("content") val content: String,
    @JsonProperty("status") val status: TodoStatus,
    @JsonProperty("location") val location: PointDto,
    @JsonProperty("dueUtc") val dueUtc: LocalDateTime
)