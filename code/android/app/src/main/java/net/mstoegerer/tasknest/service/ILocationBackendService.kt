import net.mstoegerer.tasknest.entity.LocationEntity
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST

interface ILocationBackendService {
    @POST("api/locations")
    fun postLocations(@Body locations: List<LocationEntity>): Call<Void>
}