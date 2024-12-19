package at.avollmaier.tasknest.ui.screens.overview

import android.content.Context
import android.net.Uri
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.common.ManifestUtils
import at.avollmaier.tasknest.todo.data.AttachmentDto
import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.PointDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.todo.domain.service.TodoService
import com.google.android.libraries.places.api.Places
import com.google.android.libraries.places.api.net.PlacesClient
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import java.time.ZonedDateTime
import java.util.UUID

class OverviewViewModel(private val todoService: TodoService, context: Context) : ViewModel() {
    private val _todos = MutableStateFlow<List<FetchTodoDto>>(emptyList())
    val todos: StateFlow<List<FetchTodoDto>> = _todos
    private lateinit var placesClient: PlacesClient

    init {
        val apiKey = ManifestUtils.getApiKeyFromManifest(context)

        if (!Places.isInitialized()) {
            apiKey?.let { key -> Places.initialize(context, key) }
        }
        placesClient = Places.createClient(context)
        fetchTodos()
    }

    fun getPlaceClient(): PlacesClient {
        return placesClient
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getNewTodos { fetchedTodos ->
                fetchedTodos.let {
                    _todos.value = fetchedTodos
                }

            }
        }
    }

    fun refreshTodos() {
        fetchTodos()
    }

    fun addTodo(
        title: String,
        content: String,
        dueDateTime: ZonedDateTime,
        context: Context,
        attachments: List<Uri>,
        location: PointDto
    ) {
        viewModelScope.launch {
            val id = UUID.randomUUID()

            todoService.createTodo(
                CreateTodoDto(
                    title = title,
                    content = content,
                    status = TodoStatus.NEW,
                    location = location,
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