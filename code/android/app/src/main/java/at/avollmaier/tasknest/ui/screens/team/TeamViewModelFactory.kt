package at.avollmaier.tasknest.ui.screens.team

import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.ViewModelProvider
import at.avollmaier.tasknest.auth.domain.service.ExternalUserService
import at.avollmaier.tasknest.todo.domain.service.TodoService

class TeamViewModelFactory(private val context: Context) : ViewModelProvider.Factory {
    override fun <T : ViewModel> create(modelClass: Class<T>): T {
        if (modelClass.isAssignableFrom(TeamViewModel::class.java)) {
            return TeamViewModel(TodoService(context), ExternalUserService(context)) as T
        }
        throw IllegalArgumentException("Unknown ViewModel class")
    }
}