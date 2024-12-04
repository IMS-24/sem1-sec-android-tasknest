package net.mstoegerer.tasknest.auth.domain.config

import com.auth0.android.authentication.storage.SecureCredentialsManager
import kotlinx.coroutines.runBlocking
import net.mstoegerer.tasknest.auth.domain.service.AuthService
import okhttp3.Interceptor
import okhttp3.Response

class AccessTokenInterceptor(
    private val credentialsManager: SecureCredentialsManager
) : Interceptor {
    companion object {
        const val HEADER_AUTHORIZATION = "Authorization"
        const val TOKEN_TYPE = "Bearer"
    }

    override fun intercept(chain: Interceptor.Chain): Response {
        val token = runBlocking {
            AuthService.fetchAccessToken(credentialsManager)
        }
        val request = chain.request().newBuilder()
        request.addHeader(HEADER_AUTHORIZATION, "$TOKEN_TYPE $token")
        return chain.proceed(request.build())
    }
}
