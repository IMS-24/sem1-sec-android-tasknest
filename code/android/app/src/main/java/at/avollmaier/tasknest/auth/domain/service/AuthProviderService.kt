package at.avollmaier.tasknest.auth.domain.service

import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.data.User
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
import kotlin.coroutines.resume
import kotlin.coroutines.suspendCoroutine

class AuthProviderService : AuthProvider {

    private lateinit var account: Auth0
    private lateinit var context: Context
    private lateinit var apiClient: AuthenticationAPIClient
    private lateinit var credentialsManager: SecureCredentialsManager


    override fun setUp(context: Context) {
        this.context = context
        this.account = Auth0(
            context.getString(R.string.com_auth0_client_id),
            context.getString(R.string.com_auth0_domain),
        )
        this.apiClient = AuthenticationAPIClient(account)
        credentialsManager = SecureCredentialsManager(
            context,
            apiClient,
            SharedPreferencesStorage(context)
        )
    }

    override fun loginWithBrowser(
        context: Context,
        onError: (String) -> Unit,
        onSuccess: (User) -> Unit,
    ) {
        WebAuthProvider.login(account)
            .withScheme(context.getString(R.string.com_auth0_scheme))
            .withScope("openid profile email offline_access")
            .withAudience("https://${context.getString(R.string.com_auth0_domain)}/api/v2/")
            .start(
                context,
                object : Callback<Credentials, AuthenticationException> {

                    override fun onFailure(error: AuthenticationException) {
                        onError(error.getCode())
                    }

                    override fun onSuccess(result: Credentials) {
                        credentialsManager.saveCredentials(result)
                        apiClient.userInfo(result.accessToken)
                            .start(object : Callback<UserProfile, AuthenticationException> {
                                override fun onFailure(error: AuthenticationException) {
                                    onError(error.getCode())
                                }

                                override fun onSuccess(result: UserProfile) {
                                    val user = mapUserProfileToUser(result)
                                    onSuccess(user)
                                }
                            })
                    }
                },
            )
    }

    override fun logOut(context: Context, onError: (String) -> Unit, onSuccess: () -> Unit) {

        WebAuthProvider.logout(account)
            .withScheme(context.getString(R.string.com_auth0_scheme))
            .start(context, object : Callback<Void?, AuthenticationException> {
                override fun onSuccess(result: Void?) {
                    onSuccess()
                    credentialsManager.clearCredentials()
                }

                override fun onFailure(error: AuthenticationException) {
                    onError(error.getCode())
                }
            })
    }

    override suspend fun getAccessToken(): String {
        return suspendCoroutine { continuation ->
            credentialsManager.getCredentials(object : Callback<Credentials, CredentialsManagerException> {
                override fun onSuccess(result: Credentials) {
                    continuation.resume(result.accessToken)
                }

                override fun onFailure(error: CredentialsManagerException) {
                    Log.e("AuthProviderImpl", "Error fetching access token: ${error.message}")
                }
            })
        }
    }


    private fun mapUserProfileToUser(profile: UserProfile): User {
        return User(
            id = profile.getId(),
            name = profile.name,
            nickname = profile.nickname,
            pictureURL = profile.pictureURL,
            email = profile.email,
            isEmailVerified = profile.isEmailVerified,
            familyName = profile.familyName,
            createdAt = profile.createdAt
        )
    }
}