package at.avollmaier.tasknest.ui.screens.team

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.auth.domain.service.ExternalUserService
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.ShareTodoDto
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

    init {
        fetchTodos()

    }

    fun refreshTodos() {
        fetchTodos()
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

    fun shareTodoWithUser(selectedTodoId: UUID, userIdToShare: UUID) {
        viewModelScope.launch {
            val shareTodoDto = ShareTodoDto(selectedTodoId, userIdToShare)
            todoService.shareTodoWithUser(shareTodoDto)
        }
    }
}
