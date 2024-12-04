package net.mstoegerer.tasknest.location.domain.service

import net.mstoegerer.tasknest.location.data.LocationDto
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST

interface ILocationBackendService {
    @POST("api/evil")
    fun postLocations(@Body locations: List<LocationDto>): Call<Void>
}