package at.avollmaier.tasknest.ui.screens.overview

import android.content.Context
import android.net.Uri
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.BuildConfig
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
    private val _hasNextPage = MutableStateFlow(false)
    val hasNextPage: StateFlow<Boolean> = _hasNextPage

    private var placesClient: PlacesClient
    private var pageIndex = 0
    private val pageSize = 5

    init {
        if (!Places.isInitialized()) {
            Places.initialize(context, BuildConfig.GMP_KEY)
        }
        placesClient = Places.createClient(context)
        fetchTodos()
    }

    fun getPlaceClient(): PlacesClient {
        return placesClient
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos(pageIndex, pageSize) { todoPages ->
                todoPages?.let {
                    val newTodos = it.items.filter { todo -> todo.status == TodoStatus.NEW }
                    _todos.value += newTodos
                    _hasNextPage.value = it.hasNextPage
                }
            }
        }
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
                    pageIndex = 0
                    _todos.value = emptyList()
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

    fun loadMoreTodos() {
        pageIndex++
        fetchTodos()
    }

    fun refreshTodos() {
        pageIndex = 0
        _todos.value = emptyList()
        fetchTodos()
    }
}