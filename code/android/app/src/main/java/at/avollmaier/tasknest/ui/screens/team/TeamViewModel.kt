package at.avollmaier.tasknest.ui.screens.team

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.auth.data.ExternalUserDto
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

    private val _users = MutableStateFlow<List<ExternalUserDto>>(emptyList())
    val users: StateFlow<List<ExternalUserDto>> = _users

    private val _share = MutableStateFlow<List<ShareTodoDto>>(emptyList())
    val share: StateFlow<List<ShareTodoDto>> = _share

    init {
        fetchTodos()

    }


    fun refreshTodos() {
        fetchTodos()
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos { fetchedTodos ->
                fetchedTodos?.let {
                    _todos.value = filterActiveTodos(fetchedTodos)
                    fetchTodoShareInfo()

                }
            }

        }
    }

    private fun fetchTodoShareInfo() {
        viewModelScope.launch {
            todoService.getTodoShareInfo { info ->
                info?.let {
                    _share.value = info
                    fetchUsers()
                }
            }

        }
    }

    private fun getTeamMembersFromTodos(todos: List<FetchTodoDto>): List<UUID> {
        val teamMembers = mutableListOf<UUID>()
        todos.forEach { todo ->
            _share.value.forEach { share ->
                if (share.todoId == todo.id) {
                    teamMembers.addAll(share.sharedWithId)
                }
            }
        }
        return teamMembers
    }

    private fun filterActiveTodos(todos: List<FetchTodoDto>): List<FetchTodoDto> {
        return todos.filter { it.status == TodoStatus.NEW }
    }

    private fun fetchUsers() {
        viewModelScope.launch {
            getTeamMembersFromTodos(_todos.value).forEach { userId ->
                externalUserService.getUser(userId, { user ->
                    user?.let {
                        _users.value += user
                    }
                })
            }

        }
    }

    fun shareTodoWithUser(selectedTodoId: UUID, userIdToShare: UUID) {
        viewModelScope.launch {
            val shareTodoDto = ShareTodoDto(selectedTodoId, emptyList())
            todoService.shareTodoWithUser(shareTodoDto)
        }
    }
}
