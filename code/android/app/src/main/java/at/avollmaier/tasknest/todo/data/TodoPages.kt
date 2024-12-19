package at.avollmaier.tasknest.todo.data

import com.fasterxml.jackson.annotation.JsonCreator
import com.fasterxml.jackson.annotation.JsonProperty

data class TodoPages @JsonCreator constructor(
    @JsonProperty("items") val items: List<FetchTodoDto>,
    @JsonProperty("pageIndex") val pageIndex: Int,
    @JsonProperty("pageSize") val pageSize: Int,
    @JsonProperty("totalCount") val totalCount: Int,
    @JsonProperty("totalPages") val totalPages: Int,
    @JsonProperty("hasPreviousPage") val hasPreviousPage: Boolean,
    @JsonProperty("hasNextPage") val hasNextPage: Boolean
)