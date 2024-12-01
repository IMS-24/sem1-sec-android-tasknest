package net.mstoegerer.tasknest.service

import android.content.Context
import android.util.Log
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import net.mstoegerer.tasknest.R
import net.mstoegerer.tasknest.dto.TodoDto
import okhttp3.OkHttpClient
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

    fun getTodo(id: UUID, callback: (TodoDto?) -> Unit) {
        ioScope.launch {
            try {
                val response = api.getTodo(id).execute()
                if (response.isSuccessful) {
                    callback(response.body())
                } else {
                    Log.e("TodoService", "Failed to get todo: ${response.errorBody()?.string()}")
                    callback(null)
                }
            } catch (e: Exception) {
                Log.e("TodoService", "Exception while getting todo", e)
                callback(null)
            }
        }
    }

    fun getTodos(callback: (List<TodoDto>?) -> Unit) {
        ioScope.launch {
            try {
                val response = api.getTodos().execute()
                if (response.isSuccessful) {
                    callback(response.body())
                } else {
                    Log.e("TodoService", "Failed to get todos: ${response.errorBody()?.string()}")
                    callback(null)
                }
            } catch (e: Exception) {
                Log.e("TodoService", "Exception while getting todos", e)
                callback(null)
            }
        }
    }

    fun createTodo(todoDto: TodoDto, callback: (Boolean) -> Unit) {
        ioScope.launch {
            try {
                val response = api.createTodo(todoDto).execute()
                callback(response.isSuccessful)
            } catch (e: Exception) {
                Log.e("TodoService", "Exception while creating todo", e)
                callback(false)
            }
        }
    }

    fun patchTodo(id: UUID, todoDto: TodoDto, callback: (Boolean) -> Unit) {
        ioScope.launch {
            try {
                val response = api.patchTodo(id, todoDto).execute()
                callback(response.isSuccessful)
            } catch (e: Exception) {
                Log.e("TodoService", "Exception while patching todo", e)
                callback(false)
            }
        }
    }

    fun deleteTodo(id: UUID, callback: (Boolean) -> Unit) {
        ioScope.launch {
            try {
                val response = api.deleteTodo(id).execute()
                callback(response.isSuccessful)
            } catch (e: Exception) {
                Log.e("TodoService", "Exception while deleting todo", e)
                callback(false)
            }
        }
    }
}