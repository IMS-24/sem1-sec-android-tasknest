package at.avollmaier.tasknest.ui.screens.overview

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import at.avollmaier.tasknest.todo.domain.service.TodoService

class OverviewViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(OverviewViewModel::class.java)) {
            return OverviewViewModel(TodoService(context)) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}