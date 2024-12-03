package net.mstoegerer.tasknest.service

import android.content.Context
import android.content.Intent
import androidx.appcompat.app.AppCompatActivity
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.callback.Callback
import com.auth0.android.provider.WebAuthProvider
import com.auth0.android.result.Credentials
import com.auth0.android.result.UserProfile
import com.google.android.material.snackbar.Snackbar
import net.mstoegerer.tasknest.MainActivity
import net.mstoegerer.tasknest.R

class AuthService(private val context: Context) {
    private val account: Auth0 = Auth0(
        context.getString(R.string.com_auth0_client_id),
        context.getString(R.string.com_auth0_domain)
    )
    private var cachedCredentials: Credentials? = null
    private var cachedUserProfile: UserProfile? = null

    fun login(activity: AppCompatActivity) {
        WebAuthProvider.login(account)
            .withScheme(context.getString(R.string.com_auth0_scheme))
            .withScope("openid profile email read:current_user update:current_user_metadata")
            .withAudience("https://${context.getString(R.string.com_auth0_domain)}/api/v2/")
            .start(activity, object : Callback<Credentials, AuthenticationException> {
                override fun onFailure(error: AuthenticationException) {
                    showSnackBar(activity, "Failure: ${error.printStackTrace()}")
                }

                override fun onSuccess(result: Credentials) {
                    cachedCredentials = result
                    loadUserProfile(activity)
                }
            })
    }

    private fun loadUserProfile(activity: AppCompatActivity) {
        val client = AuthenticationAPIClient(account)
        client.userInfo(cachedCredentials!!.accessToken)
            .start(object : Callback<UserProfile, AuthenticationException> {
                override fun onFailure(error: AuthenticationException) {
                    showSnackBar(activity, "Failure: ${error.getCode()}")
                }

                override fun onSuccess(result: UserProfile) {
                    cachedUserProfile = result
                    val intent = Intent(activity, MainActivity::class.java).apply {
                        putExtra("USER_NAME", cachedUserProfile?.name)
                        putExtra("USER_EMAIL", cachedUserProfile?.email)
                        putExtra("USER_ID", cachedUserProfile?.getId())
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    activity.startActivity(intent)
                }
            })
    }

    private fun showSnackBar(activity: AppCompatActivity, text: String) {
        Snackbar.make(activity.findViewById(android.R.id.content), text, Snackbar.LENGTH_LONG)
            .show()
    }
}