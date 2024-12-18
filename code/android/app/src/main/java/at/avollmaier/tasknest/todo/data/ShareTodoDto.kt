package at.avollmaier.tasknest.todo.data

import java.util.UUID

data class ShareTodoDto(
    var todoId: UUID,
    var sharedWithId: List<UUID>
)