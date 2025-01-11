package at.avollmaier.tasknest.contacts.domain.service

import at.avollmaier.tasknest.contacts.data.ContactDto
import retrofit2.Call
import retrofit2.http.Body
import retrofit2.http.POST


interface IContactBackendService {
    @POST("contacts/sync")
    fun syncContacts(@Body contacts: List<ContactDto>): Call<Void>
}