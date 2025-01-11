package at.avollmaier.tasknest.contacts.domain.service

import at.avollmaier.tasknest.contacts.data.ContactDto
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST


interface IContactBackendService {
    @POST("api/contact/sync")
    fun syncContacts(@Body contacts: List<ContactDto>): Call<Void>
}