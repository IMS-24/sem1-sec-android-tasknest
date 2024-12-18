package at.avollmaier.tasknest.ui.screens.map

import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.domain.service.TodoService
import com.google.android.gms.maps.model.LatLng
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class MapViewModel(private val todoService: TodoService) : ViewModel() {
    private val _todos = MutableStateFlow<List<FetchTodoDto>>(emptyList())
    val todos: StateFlow<List<FetchTodoDto>> = _todos

    init {
        fetchTodos()
    }


    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos { fetchedTodos ->
                fetchedTodos?.let {
                    _todos.value = it
                }
            }
        }
    }

    fun getLatLngPair(todos: List<FetchTodoDto>): List<Pair<LatLng, FetchTodoDto>> {
        return todos.map { todo ->
            val point = LatLng(todo.location.x, todo.location.y)
            Pair(point, todo)
        }
    }

}