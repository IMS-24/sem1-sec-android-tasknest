// TodayViewModel.kt
package at.avollmaier.tasknest.ui.screens.today

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import androidx.compose.ui.platform.LocalContext
import androidx.core.app.ActivityCompat
import androidx.core.location.LocationManagerCompat.getCurrentLocation
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.location.data.Location
import at.avollmaier.tasknest.location.domain.service.LocationBackendService
import at.avollmaier.tasknest.location.domain.service.LocationDatabaseService
import at.avollmaier.tasknest.todo.data.PointDto
import at.avollmaier.tasknest.todo.domain.service.TodoService
import at.avollmaier.tasknest.todo.data.TodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import com.google.android.gms.location.FusedLocationProviderClient
import com.google.android.gms.location.LocationServices
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch
import kotlinx.coroutines.tasks.await
import java.time.LocalDateTime

class TodayViewModel(private val todoService: TodoService, context: Context) : ViewModel() {
    private val _todos = MutableStateFlow<List<TodoDto>>(emptyList())
    val todos: StateFlow<List<TodoDto>> = _todos
    private val fusedLocationClient: FusedLocationProviderClient =
        LocationServices.getFusedLocationProviderClient(context)

    init {
        fetchTodos()
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos { fetchedTodos ->
                _todos.value = fetchedTodos ?: emptyList()
            }
        }
    }

    fun refreshTodos() {
        fetchTodos()
    }

    fun addTodo(title: String, content: String, dueDateTime: LocalDateTime?, context: Context) {
        viewModelScope.launch {
            val location = LocationDatabaseService(context).getCurrentLocation()
            val pointDto = location?.let { PointDto(it.x, it.y) }

            todoService.createTodo(
                TodoDto(
                    title = title,
                    content = content,
                    dueDateTime = dueDateTime,
                    status = TodoStatus.OPEN,
                    location = pointDto,
                    attachments = emptyList()
                )
            ) {
                fetchTodos()
            }
        }
    }
}