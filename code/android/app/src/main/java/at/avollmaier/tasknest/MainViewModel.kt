package at.avollmaier.tasknest

import android.content.Context
import android.util.Log
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import at.avollmaier.tasknest.auth.data.User
import at.avollmaier.tasknest.auth.domain.service.AuthProviderService
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class MainViewModel : ViewModel() {
    private val _userIsAuthenticated = MutableStateFlow(false)
    val userIsAuthenticated: StateFlow<Boolean> = _userIsAuthenticated
    private var user: User? = null

    fun loginUser(context: Context, authProvider: AuthProviderService) {
        viewModelScope.launch {
            authProvider.loginWithBrowser(
                onError = { error ->
                    Log.d("MainViewModel", "Error: $error")
                },
                onSuccess = { result ->
                    _userIsAuthenticated.value = true
                    user = result
                },
                context = context
            )
        }
    }

    fun getUser(): User {
        return user ?: throw IllegalStateException("User is not authenticated")
    }
}