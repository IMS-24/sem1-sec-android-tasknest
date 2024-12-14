package at.avollmaier.tasknest.auth.domain.service

import at.avollmaier.tasknest.auth.data.ExternalUserDto
import retrofit2.Call
import retrofit2.http.GET
import retrofit2.http.Path
import java.util.UUID

interface IExternalUserService {
    @GET("api/user/{id}")
    fun getUser(@Path("id") id: UUID): Call<ExternalUserDto>
}