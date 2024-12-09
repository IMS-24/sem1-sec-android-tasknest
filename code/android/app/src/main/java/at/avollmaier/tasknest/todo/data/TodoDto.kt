package at.avollmaier.tasknest.todo.data

import java.time.LocalDateTime
import java.util.Date

data class TodoDto(
    val title: String,
    val content: String,
    val status: TodoStatus,
    val location: PointDto?,
    val attachments: List<AttachmentDto> = emptyList(),
    val dueDateTime: LocalDateTime? = null,
)