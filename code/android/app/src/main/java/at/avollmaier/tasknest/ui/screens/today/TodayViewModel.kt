// TodayViewModel.kt
package at.avollmaier.tasknest.ui.screens.today

import android.content.Context
import android.net.Uri
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.location.domain.service.LocationDatabaseService
import at.avollmaier.tasknest.todo.data.AttachmentDto
import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.PointDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.todo.domain.service.TodoService
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import java.time.ZonedDateTime
import java.util.UUID

// TodayViewModel.kt
class TodayViewModel(private val todoService: TodoService, context: Context) : ViewModel() {
    private val _todos = MutableStateFlow<List<FetchTodoDto>>(emptyList())
    val todos: StateFlow<List<FetchTodoDto>> = _todos


    init {
        fetchTodos()
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos { fetchedTodos ->
                fetchedTodos?.let {
                    _todos.value = filterActiveTodos(fetchedTodos)
                }

            }
        }
    }

    private fun filterActiveTodos(todos: List<FetchTodoDto>): List<FetchTodoDto> {
        return todos.filter { it.status == TodoStatus.NEW }
    }

    fun refreshTodos() {
        fetchTodos()
    }

    fun addTodo(
        title: String,
        content: String,
        dueDateTime: ZonedDateTime,
        context: Context,
        attachments: MutableList<Uri>
    ) {
        viewModelScope.launch {
            val location = LocationDatabaseService(context).getCurrentLocation()
            val id = UUID.randomUUID()

            todoService.createTodo(
                CreateTodoDto(
                    title = title,
                    content = content,
                    status = TodoStatus.NEW,
                    location = PointDto(location.x, location.y),
                    dueUtc = dueDateTime,
                    attachments = handleFilePickerResult(context, id, attachments)
                )
            ) {
                fetchTodos()
            }
        }
    }

    fun finishTodo(todo: FetchTodoDto) {
        viewModelScope.launch {
            val todoToUpdate = _todos.value.find { it.id == todo.id }
            todoToUpdate?.let {
                todoService.finishTodo(it.id) {
                    fetchTodos()
                }
            }
        }
    }

    private fun handleFilePickerResult(
        context: Context,
        uuid: UUID,
        uris: List<Uri>
    ): List<AttachmentDto> {
        return uris.map { uri ->
            val fileName = uri.path?.substringAfterLast('/') ?: "Unknown"
            val contentType = context.contentResolver.getType(uri) ?: "application/octet-stream"
            val data: ByteArray =
                context.contentResolver.openInputStream(uri)?.use { it.readBytes() } ?: ByteArray(0)
            AttachmentDto(
                name = fileName,
                fileName = fileName,
                contentType = contentType,
                data = data,
                todoId = uuid,
            )
        }
    }
}