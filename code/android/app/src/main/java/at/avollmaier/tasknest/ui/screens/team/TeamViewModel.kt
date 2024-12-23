package at.avollmaier.tasknest.ui.screens.team

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.auth.domain.service.ExternalUserService
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.ShareTodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.todo.domain.service.TodoService
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import java.util.UUID

class TeamViewModel(
    private val todoService: TodoService,
    private val externalUserService: ExternalUserService
) : ViewModel() {
    private val _todos = MutableStateFlow<List<FetchTodoDto>>(emptyList())
    val todos: StateFlow<List<FetchTodoDto>> = _todos
    private val _hasNextPage = MutableStateFlow(false)
    val hasNextPage: StateFlow<Boolean> = _hasNextPage

    private var pageIndex = 0
    private val pageSize = 5

    init {
        fetchTodos()
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

    fun loadMoreTodos() {
        pageIndex++
        fetchTodos()
    }

    fun refreshTodos() {
        pageIndex = 0
        _todos.value = emptyList()
        fetchTodos()
    }

    fun shareTodoWithUser(selectedTodoId: UUID, userIdToShare: UUID) {
        viewModelScope.launch {
            val shareTodoDto = ShareTodoDto(selectedTodoId, userIdToShare)
            todoService.shareTodoWithUser(shareTodoDto)
        }
    }
}
