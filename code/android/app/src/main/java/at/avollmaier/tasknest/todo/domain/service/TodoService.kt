package at.avollmaier.tasknest.todo.domain.service

import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.common.NetworkUtils
import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.ShareTodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response
import java.util.UUID

class TodoService(context: Context) {
    private val ioScope = CoroutineScope(Dispatchers.IO)


    private val api: ITodoService =
        NetworkUtils.provideRetrofit(context).create(ITodoService::class.java)


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

    fun getTodos(callback: (List<FetchTodoDto>?) -> Unit) {
        makeApiCall(
            apiCall = { api.getTodos().execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
    }

    fun createTodo(createTodoDto: CreateTodoDto, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.createTodo(createTodoDto).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }

    fun finishTodo(id: UUID, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.finishTodo(id).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }

    fun shareTodoWithUser(shareTodoDto: ShareTodoDto) {
        makeApiCall(
            apiCall = { api.shareTodoWithUser(shareTodoDto).execute() },
            onSuccess = { },
            onError = { }
        )
    }

    fun getNewTodos(callback: (List<FetchTodoDto>) -> Unit) {
        getTodos { todos ->
            todos?.filter { it.status == TodoStatus.NEW }?.let { callback(it) }
        }
    }

}