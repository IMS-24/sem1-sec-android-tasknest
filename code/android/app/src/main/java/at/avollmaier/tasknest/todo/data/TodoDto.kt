package at.avollmaier.tasknest.todo.data

import java.util.*

data class TodoDto(
    val id: UUID,
    val title: String,
    val content: String,
    val status: String,
    val userId: UUID,
    val assignedToId: UUID,
    val location: PointDto?,
    val attachments: List<AttachmentDto> = emptyList()
)