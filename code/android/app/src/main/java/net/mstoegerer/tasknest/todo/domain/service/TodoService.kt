package net.mstoegerer.tasknest.todo.domain.service

import android.content.Context
import android.util.Log
import com.auth0.android.Auth0
import com.auth0.android.authentication.AuthenticationAPIClient
import com.auth0.android.authentication.storage.SecureCredentialsManager
import com.auth0.android.authentication.storage.SharedPreferencesStorage
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.auth.domain.config.AccessTokenInterceptor
import net.mstoegerer.tasknest.todo.data.TodoDto
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.UUID
import java.util.concurrent.TimeUnit

class TodoService(private val context: Context) {
    private val ioScope = CoroutineScope(Dispatchers.IO)
    private val account: Auth0 = Auth0(
        context.getString(R.string.com_auth0_client_id),
        context.getString(R.string.com_auth0_domain)
    )
    private val apiClient = AuthenticationAPIClient(account)
    private val credentialsManager =
        SecureCredentialsManager(context, apiClient, SharedPreferencesStorage(context))

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(context.getString(R.string.backend_url))
        .client(provideAccessOkHttpClient(AccessTokenInterceptor(credentialsManager)))
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    private val api: ITodoService = retrofit.create(ITodoService::class.java)

    private fun provideAccessOkHttpClient(
        accessTokenInterceptor: AccessTokenInterceptor
    ): OkHttpClient {
        val loggingInterceptor = HttpLoggingInterceptor()
        loggingInterceptor.level = HttpLoggingInterceptor.Level.BODY
        return OkHttpClient.Builder()
            .addInterceptor(accessTokenInterceptor)
            .addInterceptor(loggingInterceptor)
            .connectTimeout(30, TimeUnit.SECONDS)
            .readTimeout(30, TimeUnit.SECONDS)
            .writeTimeout(30, TimeUnit.SECONDS)
            .build()
    }

    private fun <T> makeApiCall(
        apiCall: suspend () -> Response<T>,
        onSuccess: (T?) -> Unit,
        onError: () -> Unit
    ) {
        ioScope.launch {
            try {
                val response = apiCall()
                if (response.isSuccessful) {
                    onSuccess(response.body())
                } else {
                    Log.e("TodoService", "API call failed: ${response.raw()}")
                    onError()
                }
            } catch (e: Exception) {
                Log.e("TodoService", "Exception during API call", e)
                onError()
            }
        }
    }

    fun getTodo(id: UUID, callback: (TodoDto?) -> Unit) {
        makeApiCall(
            apiCall = { api.getTodo(id).execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
    }

    fun getTodos(callback: (List<TodoDto>?) -> Unit) {
        makeApiCall(
            apiCall = { api.getTodos().execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
    }

    fun createTodo(todoDto: TodoDto, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.createTodo(todoDto).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }

    fun patchTodo(id: UUID, todoDto: TodoDto, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.patchTodo(id, todoDto).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }

    fun deleteTodo(id: UUID, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.deleteTodo(id).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }
}