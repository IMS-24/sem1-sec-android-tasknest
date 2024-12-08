package at.avollmaier.tasknest.todo.data

import java.util.*

data class AttachmentDto(
    val id: UUID,
    val name: String,
    val fileName: String,
    val contentType: String,
    val data: String,
    val size: Long,
    val todoId: UUID,
    val uploadedById: UUID,
)