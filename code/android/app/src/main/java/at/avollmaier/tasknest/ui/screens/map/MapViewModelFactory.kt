package at.avollmaier.tasknest.ui.screens.map

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import at.avollmaier.tasknest.todo.domain.service.TodoService

class MapViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(MapViewModel::class.java)) {
            return MapViewModel(TodoService(context)) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}