package at.avollmaier.tasknest.todo.domain.service

import ZonedDateTimeToUtcSerializer
import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.domain.config.AccessTokenInterceptor
import at.avollmaier.tasknest.todo.data.CreateTodoDto
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import com.fasterxml.jackson.databind.DeserializationFeature
import com.fasterxml.jackson.databind.MapperFeature
import com.fasterxml.jackson.databind.ObjectMapper
import com.fasterxml.jackson.databind.SerializationFeature
import com.fasterxml.jackson.databind.module.SimpleModule
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Response
import retrofit2.Retrofit
import retrofit2.converter.jackson.JacksonConverterFactory
import java.time.ZonedDateTime
import java.util.UUID
import java.util.concurrent.TimeUnit

class TodoService(private val context: Context) {
    private val ioScope = CoroutineScope(Dispatchers.IO)


    private val objectMapper = ObjectMapper().apply {
        registerModule(JavaTimeModule())
        disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS)
        disable(DeserializationFeature.ADJUST_DATES_TO_CONTEXT_TIME_ZONE)
        configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false)
        enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS)

        val module = SimpleModule()
        module.addSerializer(ZonedDateTime::class.java, ZonedDateTimeToUtcSerializer())
        registerModule(module)
    }
    private val retrofit: Retrofit = Retrofit.Builder()
        .baseUrl(context.getString(R.string.backend_url))
        .client(provideAccessOkHttpClient(AccessTokenInterceptor(context)))
        .addConverterFactory(JacksonConverterFactory.create(objectMapper))

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

    fun getTodo(id: UUID, callback: (CreateTodoDto?) -> Unit) {
        makeApiCall(
            apiCall = { api.getTodo(id).execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
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

    fun deleteTodo(id: UUID, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.deleteTodo(id).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }
}