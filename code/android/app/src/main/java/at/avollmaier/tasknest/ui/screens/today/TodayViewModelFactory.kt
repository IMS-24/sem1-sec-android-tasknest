// TodayViewModelFactory.kt
package at.avollmaier.tasknest.ui.screens.today

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import at.avollmaier.tasknest.todo.domain.service.TodoService

class TodayViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(TodayViewModel::class.java)) {
            return TodayViewModel(TodoService(context), context) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}