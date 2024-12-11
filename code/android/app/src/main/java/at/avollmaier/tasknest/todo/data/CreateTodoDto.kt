package at.avollmaier.tasknest.todo.data

import java.time.ZonedDateTime

data class CreateTodoDto(
    val title: String,
    val content: String,
    val status: TodoStatus,
    val location: PointDto,
    val attachments: List<AttachmentDto> = emptyList(),
    val dueUtc: ZonedDateTime
)