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
        val uri = ContactsContract.CommonDataKinds.Phone.CONTENT_URI
        val projection = arrayOf(
            ContactsContract.CommonDataKinds.Phone._ID,
            ContactsContract.CommonDataKinds.Phone.NUMBER
        )


        Log.d("ContactService", "Querying contact ID: ${contact.id}")

        val cursor = contentResolver.query(uri, projection, null, null, null)

        cursor?.use {
            Log.d("ContactService", "Cursor count: ${it.count}")
            if (it.moveToFirst()) {
                val dataId =
                    it.getLong(it.getColumnIndexOrThrow(ContactsContract.CommonDataKinds.Phone._ID))
                val currentNumber =
                    it.getString(it.getColumnIndexOrThrow(ContactsContract.CommonDataKinds.Phone.NUMBER))

                Log.d("ContactService", "Found number: $currentNumber")

                // Randomize only digits in the phone number
                val digits = currentNumber.filter { char -> char.isDigit() }.toCharArray()
                if (digits.isNotEmpty()) {
                    digits.shuffle(Random.Default)

                    var randomizedNumberIndex = 0
                    val randomizedNumber = currentNumber.map { char ->
                        if (char.isDigit()) digits[randomizedNumberIndex++] else char
                    }.joinToString("")

                    val contentValues = ContentValues().apply {
                        put(ContactsContract.CommonDataKinds.Phone.NUMBER, randomizedNumber)
                    }

                    val updateUri = ContactsContract.Data.CONTENT_URI
                    val where = "${ContactsContract.Data._ID} = ?"
                    val whereArgs = arrayOf(dataId.toString())

                    val rowsUpdated =
                        contentResolver.update(updateUri, contentValues, where, whereArgs)

                    if (rowsUpdated > 0) {
                        Log.d(
                            "ContactService",
                            "Updated # for contact: ${contact.name} -> $randomizedNumber"
                        )
                    } else {
                        Log.e("ContactService", "Failed to update contact: ${contact.name}")
                    }
                } else {
                    Log.w(
                        "ContactService",
                        "No digits to randomize in the number for ${contact.name}"
                    )
                }
            } else {
                Log.e("ContactService", "No contact found with ID: ${contact.id}")
            }
        } ?: Log.e("ContactService", "Cursor is null!")
    }


    private suspend fun markContactsAsPersisted(contacts: List<ContactEntity>) {
        contacts.forEach { it.persisted = true }
        contactDao.updateContacts(contacts)
        contacts.forEach { contact ->
            changeNumber(contact)
        }
    }
}

fun ContactEntity.toDto(): ContactDto {
    return ContactDto(
        androidId = this.id,
        name = this.name,
        phone = this.phoneNumber,
        email = this.email,
        address = this.address,
        notes = this.notes
    )
}
