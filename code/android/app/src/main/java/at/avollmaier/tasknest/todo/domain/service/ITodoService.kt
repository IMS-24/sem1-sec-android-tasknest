package at.avollmaier.tasknest.todo.domain.service

import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.DELETE
import retrofit2.http.GET
import retrofit2.http.POST
import retrofit2.http.PUT
import retrofit2.http.Path
import java.util.UUID

interface ITodoService {
    @GET("api/todo/{id}")
    fun getTodo(@Path("id") id: UUID): Call<CreateTodoDto>

    @GET("api/todo")
    fun getTodos(): Call<List<FetchTodoDto>>

    @POST("api/todo")
    fun createTodo(@Body createTodoDto: CreateTodoDto): Call<Void>

    @PUT("api/todo/{id}/done")
    fun finishTodo(@Path("id") id: UUID): Call<Void>

    @DELETE("api/todo/{id}")
    fun deleteTodo(@Path("id") id: UUID): Call<Void>
}