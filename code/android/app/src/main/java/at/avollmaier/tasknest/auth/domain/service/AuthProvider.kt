package at.avollmaier.tasknest.auth.domain.service

import android.content.Context
import at.avollmaier.tasknest.auth.data.User

interface AuthProvider {
    fun setUp(context: Context)

    fun loginWithBrowser(
        context: Context,
        onError: (String) -> Unit,
        onSuccess: (User) -> Unit,
    )

    fun logOut(
        context: Context,
        onError: (String) -> Unit,
        onSuccess: () -> Unit,
    )

    suspend fun getAccessToken(): String
}