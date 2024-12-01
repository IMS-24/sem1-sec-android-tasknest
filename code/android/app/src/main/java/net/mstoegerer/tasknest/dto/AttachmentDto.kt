package net.mstoegerer.tasknest.dto

import java.util.*

data class AttachmentDto(
    val id: UUID,
    val name: String,
    val fileName: String,
    val contentType: String,
    val data: ByteArray,
    val size: Long,
    val todoId: UUID,
    val uploadedById: UUID,
    val createdUtc: Date,
    val updatedUtc: Date
)