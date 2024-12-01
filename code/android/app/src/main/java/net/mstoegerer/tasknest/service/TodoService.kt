package net.mstoegerer.tasknest.service

import android.content.Context
import android.util.Log
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.dto.TodoDto
import okhttp3.OkHttpClient
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.gson.GsonConverterFactory
import java.util.UUID

class TodoService(private val context: Context) {
    private val ioScope = CoroutineScope(Dispatchers.IO)

    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(context.getString(R.string.backend_url))
        .client(OkHttpClient.Builder().build())
        .addConverterFactory(GsonConverterFactory.create())
        .build()

    private val api: ITodoService = retrofit.create(ITodoService::class.java)

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
                    Log.e("TodoService", "API call failed: ${response.errorBody()?.string()}")
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