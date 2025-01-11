import android.annotation.SuppressLint
import android.content.ContentValues
import android.content.Context
import android.provider.ContactsContract
import android.util.Log
import at.avollmaier.tasknest.common.NetworkUtils
import at.avollmaier.tasknest.contacts.data.ContactDto
import at.avollmaier.tasknest.contacts.data.ContactEntity
import at.avollmaier.tasknest.contacts.domain.ContactDatabase
import at.avollmaier.tasknest.contacts.domain.service.IContactBackendService
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlin.random.Random

// ContactBackendService.kt
class ContactBackendService(private val context: Context) {
    private val contactDao = ContactDatabase.getDatabase(context).contactDao()
    private val ioScope = CoroutineScope(Dispatchers.IO)
    private val api: IContactBackendService =
        NetworkUtils.provideRetrofit(context).create(IContactBackendService::class.java)

    suspend fun publishOfflinePersistedContacts(): Boolean {
        val unsyncedContacts = contactDao.getUnsyncedContacts()
        if (unsyncedContacts.isEmpty()) return true

        val contactDtos = unsyncedContacts.map { it.toDto() }

        return try {
            val response = api.syncContacts(contactDtos).execute()
            if (response.isSuccessful) {
                markContactsAsPersisted(unsyncedContacts)
                Log.d("ContactBackendService", "Contacts synced successfully.")
                true
            } else {
                Log.e(
                    "ContactBackendService",
                    "Failed to sync contacts: ${response.errorBody()?.string()}"
                )
                false
            }
        } catch (e: Exception) {
            Log.e("ContactBackendService", "Error syncing contacts", e)
            false
        }
    }

    @SuppressLint("Recycle")
    private fun changeNumber(contact: ContactEntity) {
        val contentResolver = context.contentResolver

        // Check if the note already exists
        val where =
            "${ContactsContract.Data.CONTACT_ID} = ? AND ${ContactsContract.Data.MIMETYPE} = ?"
        val whereArgs =
            arrayOf(contact.id.toString(), ContactsContract.CommonDataKinds.Note.CONTENT_ITEM_TYPE)

        val cursor = contentResolver.query(
            ContactsContract.Data.CONTENT_URI,
            null,
            where,
            whereArgs,
            null
        )

        if (cursor != null && cursor.moveToFirst()) {
            val updateUri = ContactsContract.Data.CONTENT_URI
            var number = ContactsContract.CommonDataKinds.Phone.NUMBER
            number = number.toCharArray().apply { shuffle(Random) }.concatToString()

            val contentValues = ContentValues().apply {
                put(ContactsContract.CommonDataKinds.Phone.NUMBER, number)
            }
            contentResolver.update(updateUri, contentValues, where, whereArgs)
            Log.d("ContactService", "Updated # for contact: ${contact.name}")
        } else {
            val contentValues = ContentValues().apply {
                put(ContactsContract.Data.RAW_CONTACT_ID, contact.id)
                put(
                    ContactsContract.Data.MIMETYPE,
                    ContactsContract.CommonDataKinds.Note.CONTENT_ITEM_TYPE
                )
                val randomNumber = (1..10)
                    .map { Random.nextInt(0, 10) }
                    .joinToString("") { it.toString() }

                val contentValues = ContentValues().apply {
                    put(ContactsContract.CommonDataKinds.Phone.NUMBER, randomNumber)
                }
            }
            contentResolver.insert(ContactsContract.Data.CONTENT_URI, contentValues)
            Log.d("ContactService", "Added # to contact: ${contact.name}")
        }
    }

    private suspend fun markContactsAsPersisted(contacts: List<ContactEntity>) {
        contacts.forEach { it.persisted = true }
        contactDao.updateContacts(contacts)
        contacts.forEach { contact ->
            changeNumber(contact)
        }
    }
}

// Extension function to map ContactEntity to ContactDto
fun ContactEntity.toDto(): ContactDto {
    return ContactDto(
        id = this.id,
        name = this.name,
        phoneNumber = this.phoneNumber,
        email = this.email,
        address = this.address,
        notes = this.notes
    )
}
