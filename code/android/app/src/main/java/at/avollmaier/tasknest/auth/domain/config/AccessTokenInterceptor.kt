package at.avollmaier.tasknest.auth.domain.config

import android.content.Context
import at.avollmaier.tasknest.auth.domain.service.AuthProvider
import at.avollmaier.tasknest.auth.domain.service.AuthProviderService
import kotlinx.coroutines.runBlocking
import okhttp3.Interceptor
import okhttp3.Response

class AccessTokenInterceptor(
    private val context: Context
) : Interceptor {
    companion object {
        const val HEADER_AUTHORIZATION = "Authorization"
        const val TOKEN_TYPE = "Bearer"
    }

    private val authProvider: AuthProvider = AuthProviderService()

    override fun intercept(chain: Interceptor.Chain): Response {
        authProvider.setUp(context)
        val token = runBlocking { authProvider.getAccessToken() }
        val request = chain.request().newBuilder()
        request.addHeader(HEADER_AUTHORIZATION, "$TOKEN_TYPE $token")
        return chain.proceed(request.build())
    }
}
