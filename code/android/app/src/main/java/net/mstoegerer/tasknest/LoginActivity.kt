package net.mstoegerer.tasknest

import android.content.Intent
import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.AuthenticationException
import com.auth0.android.callback.Callback
import com.auth0.android.provider.WebAuthProvider
import com.auth0.android.result.Credentials
import com.auth0.android.result.UserProfile
import com.google.android.material.snackbar.Snackbar
import net.mstoegerer.tasknest.databinding.ActivityLoginBinding

class LoginActivity : AppCompatActivity() {
    private lateinit var account: Auth0
    private lateinit var binding: ActivityLoginBinding
    private var cachedCredentials: Credentials? = null
    private var cachedUserProfile: UserProfile? = null

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // Set up the account object with the Auth0 application details
        account = Auth0(
            getString(R.string.com_auth0_client_id), getString(R.string.com_auth0_domain)
        )

        // Bind the button click with the login action
        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)
        binding.buttonLogin.setOnClickListener { loginWithBrowser() }
    }

    private fun loginWithBrowser() {
        // Setup the WebAuthProvider, using the custom scheme and scope.
        WebAuthProvider.login(account).withScheme(getString(R.string.com_auth0_scheme))
            .withScope("openid profile email read:current_user update:current_user_metadata")
            .withAudience("https://${getString(R.string.com_auth0_domain)}/api/v2/")

            // Launch the authentication passing the callback where the results will be received
            .start(this, object : Callback<Credentials, AuthenticationException> {
                override fun onFailure(exception: AuthenticationException) {
                    showSnackBar("Failure: ${exception.printStackTrace()}")
                }

                override fun onSuccess(credentials: Credentials) {
                    cachedCredentials = credentials
                    loadUserProfile()
                }
            })
    }

    private fun loadUserProfile() {
        val client = AuthenticationAPIClient(account)

        client.userInfo(cachedCredentials!!.accessToken!!)
            .start(object : Callback<UserProfile, AuthenticationException> {
                override fun onFailure(exception: AuthenticationException) {
                    showSnackBar("Failure: ${exception.getCode()}")
                }

                override fun onSuccess(profile: UserProfile) {
                    cachedUserProfile = profile;

                    // Start the main activity and pass the user profile information
                    val intent = Intent(this@LoginActivity, MainActivity::class.java).apply {
                        putExtra("USER_NAME", cachedUserProfile?.name)
                        putExtra("USER_EMAIL", cachedUserProfile?.email)
                        putExtra("USER_ID", cachedUserProfile?.getId())
                        flags = Intent.FLAG_ACTIVITY_NEW_TASK or Intent.FLAG_ACTIVITY_CLEAR_TASK
                    }
                    startActivity(intent)
                }
            })
    }

    private fun showSnackBar(text: String) {
        Snackbar.make(
            binding.root, text, Snackbar.LENGTH_LONG
        ).show()
    }
}