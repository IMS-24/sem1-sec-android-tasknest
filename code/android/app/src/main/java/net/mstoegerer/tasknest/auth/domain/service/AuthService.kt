package net.mstoegerer.tasknest.auth.domain.service

import android.content.Context
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.authentication.storage.CredentialsManagerException
import com.auth0.android.authentication.storage.SecureCredentialsManager
import com.auth0.android.authentication.storage.SharedPreferencesStorage
import com.auth0.android.callback.Callback
import com.auth0.android.provider.WebAuthProvider
import com.auth0.android.result.Credentials
import com.auth0.android.result.UserProfile
import com.google.android.material.snackbar.Snackbar
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.MainActivity
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.auth.domain.exception.UnauthorizedException
import kotlin.coroutines.resume
import kotlin.coroutines.resumeWithException
import kotlin.coroutines.suspendCoroutine

class AuthService(
    private val context: Context,
    private val uiScope: CoroutineScope = CoroutineScope(Dispatchers.Main)
) {
    private val account: Auth0 = createAuth0Instance(context)
    private val apiClient: AuthenticationAPIClient = AuthenticationAPIClient(account)
    private val credentialsManager: SecureCredentialsManager =
        createCredentialsManager(context, apiClient)

    fun login(activity: AppCompatActivity) {
        WebAuthProvider.login(account)
            .applyDefaultSettings(context)
            .start(activity, LoginCallback(activity, credentialsManager, apiClient, uiScope))
    }

    companion object {
        fun WebAuthProvider.Builder.applyDefaultSettings(context: Context): WebAuthProvider.Builder {
            return withScheme(context.getString(R.string.com_auth0_scheme))
                .withScope("openid profile email read:current_user update:current_user_metadata")
                .withAudience("https://${context.getString(R.string.com_auth0_domain)}/api/v2/")
        }

        private fun createAuth0Instance(context: Context): Auth0 {
            return Auth0(
                context.getString(R.string.com_auth0_client_id),
                context.getString(R.string.com_auth0_domain)
            )
        }

        private fun createCredentialsManager(
            context: Context,
            apiClient: AuthenticationAPIClient
        ): SecureCredentialsManager {
            return SecureCredentialsManager(
                context,
                apiClient,
                SharedPreferencesStorage(context)
            )
        }

        suspend fun fetchUserProfile(
            credentialsManager: SecureCredentialsManager,
            apiClient: AuthenticationAPIClient
        ): UserProfile {
            val accessToken = fetchAccessToken(credentialsManager)
            return suspendCoroutine { continuation ->
                apiClient.userInfo(accessToken).start(object : Callback<UserProfile, AuthenticationException> {
                    override fun onFailure(error: AuthenticationException) {
                        continuation.resumeWithException(UnauthorizedException())
                    }

                    override fun onSuccess(result: UserProfile) {
                        continuation.resume(result)
                    }
                })
            }
        }

        suspend fun fetchAccessToken(credentialsManager: SecureCredentialsManager): String {
            return suspendCoroutine { continuation ->
                credentialsManager.getCredentials(object : Callback<Credentials, CredentialsManagerException> {
                    override fun onSuccess(result: Credentials) {
                        continuation.resume(result.accessToken)
                    }

                    override fun onFailure(error: CredentialsManagerException) {
                        continuation.resumeWithException(UnauthorizedException())
                    }
                })
            }
        }
    }
}

private class LoginCallback(
    private val activity: AppCompatActivity,
    private val credentialsManager: SecureCredentialsManager,
    private val apiClient: AuthenticationAPIClient,
    private val uiScope: CoroutineScope
) : Callback<Credentials, AuthenticationException> {

    override fun onFailure(error: AuthenticationException) {
        showSnackBar(activity, "Login failed: ${error.localizedMessage}")
    }

    override fun onSuccess(result: Credentials) {
        credentialsManager.saveCredentials(result)
        uiScope.launch {
            handleProfileLoading(activity, credentialsManager, apiClient)
        }
    }

    private suspend fun handleProfileLoading(
        activity: AppCompatActivity,
        credentialsManager: SecureCredentialsManager,
        apiClient: AuthenticationAPIClient
    ) {
        try {
            val userProfile = AuthService.fetchUserProfile(credentialsManager, apiClient)
            launchMainActivity(activity, userProfile)
        } catch (e: UnauthorizedException) {
            showSnackBar(activity, "Unable to load user profile: ${e.message}")
        }
    }

    private fun launchMainActivity(activity: AppCompatActivity, userProfile: UserProfile) {
        val intent = Intent(activity, MainActivity::class.java).apply {
            putExtra("USER_NAME", userProfile.name)
            putExtra("USER_EMAIL", userProfile.email)
            putExtra("USER_ID", userProfile.getId())
            flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
        }
        activity.startActivity(intent)
    }

    private fun showSnackBar(activity: AppCompatActivity, message: String) {
        Snackbar.make(activity.findViewById(android.R.id.content), message, Snackbar.LENGTH_LONG)
            .show()
    }
}
