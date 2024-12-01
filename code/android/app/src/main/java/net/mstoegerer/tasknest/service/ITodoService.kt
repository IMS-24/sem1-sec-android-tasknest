package net.mstoegerer.tasknest.service
import net.mstoegerer.tasknest.dto.TodoDto
import retrofit2.Call
import retrofit2.http.*
import java.util.UUID

interface ITodoService {
    @GET("api/todo/{id}")
    fun getTodo(@Path("id") id: UUID): Call<TodoDto>

    @GET("api/todo")
    fun getTodos(): Call<List<TodoDto>>

    @POST("api/todo")
    fun createTodo(@Body todoDto: TodoDto): Call<Void>

    @PATCH("api/todo/{id}")
    fun patchTodo(@Path("id") id: UUID, @Body todoDto: TodoDto): Call<Void>

    @DELETE("api/todo/{id}")
    fun deleteTodo(@Path("id") id: UUID): Call<Void>
}