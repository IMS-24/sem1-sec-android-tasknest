package at.avollmaier.tasknest.todo.domain.service

import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.ShareTodoDto
import at.avollmaier.tasknest.todo.data.TodoPages
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import retrofit2.http.Query
import java.util.UUID

interface ITodoService {
    @GET("api/todo")
    fun getTodos(
        @Query("pageIndex") pageIndex: Int,
        @Query("pageSize") pageSize: Int
    ): Call<TodoPages>

    @POST("api/todo")
    fun createTodo(@Body createTodoDto: CreateTodoDto): Call<Void>

    @PUT("api/todo/{id}/done")
    fun finishTodo(@Path("id") id: UUID): Call<Void>

    @POST("api/todo/share")
    fun shareTodoWithUser(@Body shareTodoDto: ShareTodoDto): Call<Void>
}