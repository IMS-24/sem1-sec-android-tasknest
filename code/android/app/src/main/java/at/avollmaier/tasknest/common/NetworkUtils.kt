package at.avollmaier.tasknest.common

import ZonedDateTimeToUtcSerializer
import android.content.Context
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.domain.config.AccessTokenInterceptor
import com.fasterxml.jackson.databind.DeserializationFeature
import com.fasterxml.jackson.databind.MapperFeature
import com.fasterxml.jackson.databind.ObjectMapper
import com.fasterxml.jackson.databind.SerializationFeature
import com.fasterxml.jackson.databind.module.SimpleModule
import com.fasterxml.jackson.datatype.jsr310.JavaTimeModule
import okhttp3.OkHttpClient
import okhttp3.logging.HttpLoggingInterceptor
import retrofit2.Retrofit
import retrofit2.converter.jackson.JacksonConverterFactory
import java.time.ZonedDateTime
import java.util.concurrent.TimeUnit

object NetworkUtils {

    fun provideObjectMapper(): ObjectMapper {
        return ObjectMapper().apply {
            registerModule(JavaTimeModule())
            disable(SerializationFeature.WRITE_DATES_AS_TIMESTAMPS)
            disable(DeserializationFeature.ADJUST_DATES_TO_CONTEXT_TIME_ZONE)
            configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false)
            enable(MapperFeature.ACCEPT_CASE_INSENSITIVE_ENUMS)

            val module = SimpleModule()
            module.addSerializer(ZonedDateTime::class.java, ZonedDateTimeToUtcSerializer())
            registerModule(module)
        }
    }

    fun provideRetrofit(context: Context): Retrofit {
        val objectMapper = provideObjectMapper()
        return Retrofit.Builder()
            .baseUrl(context.getString(R.string.backend_url))
            .client(provideAccessOkHttpClient(AccessTokenInterceptor(context)))
            .addConverterFactory(JacksonConverterFactory.create(objectMapper))
            .build()
    }

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
}