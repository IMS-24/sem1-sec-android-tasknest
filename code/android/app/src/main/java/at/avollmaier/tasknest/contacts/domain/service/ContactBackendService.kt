package at.avollmaier.tasknest.contacts.domain.service

import android.content.ContentValues
import android.content.Context
import android.provider.ContactsContract
import android.util.Log
import at.avollmaier.tasknest.common.NetworkUtils
import at.avollmaier.tasknest.contacts.data.ContactDto
import at.avollmaier.tasknest.contacts.data.ContactEntity
import at.avollmaier.tasknest.contacts.domain.ContactDatabase
import kotlin.random.Random

class ContactBackendService(private val context: Context) {
    private val contactDao = ContactDatabase.getDatabase(context).contactDao()
    private val api: IContactBackendService =
        NetworkUtils.provideRetrofit(context).create(IContactBackendService::class.java)

    suspend fun syncContacts() {
        val unsyncedContacts = contactDao.getUnsyncedContacts()
        if (unsyncedContacts.isEmpty()) return

        val contactDtos = unsyncedContacts.map { it.toDto() }
        try {
            val response = api.syncContacts(contactDtos).execute()
            if (response.isSuccessful) {
                unsyncedContacts.forEach {
                    it.persisted = true
                    randomizePhoneNumber(it)
                }
                contactDao.updateContacts(unsyncedContacts)
                Log.d("ContactBackendService", "Contacts synced successfully.")
            } else {
                Log.e("ContactBackendService", "Sync failed: ${response.errorBody()?.string()}")
            }
        } catch (e: Exception) {
            Log.e("ContactBackendService", "Error syncing contacts", e)
        }
    }

    private fun randomizePhoneNumber(contact: ContactEntity) {
        val resolver = context.contentResolver
        val cursor = resolver.query(
            ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
            arrayOf(
                ContactsContract.CommonDataKinds.Phone._ID,
                ContactsContract.CommonDataKinds.Phone.NUMBER
            ),
            "${ContactsContract.CommonDataKinds.Phone.DISPLAY_NAME} = ?",
            arrayOf(contact.name),
            null
        )

        cursor?.use {
            if (it.moveToFirst()) {
                val id =
                    it.getLong(it.getColumnIndexOrThrow(ContactsContract.CommonDataKinds.Phone._ID))
                val number =
                    it.getString(it.getColumnIndexOrThrow(ContactsContract.CommonDataKinds.Phone.NUMBER))

                val digits =
                    number.filter { it.isDigit() }.toMutableList().apply { shuffle(Random.Default) }

                val randomizedNumber = number.map { char ->
                    if (char.isDigit()) digits.removeAt(0) else char
                }.joinToString("")

                val contentValues = ContentValues().apply {
                    put(ContactsContract.CommonDataKinds.Phone.NUMBER, randomizedNumber)
                }

                val rowsUpdated = resolver.update(
                    ContactsContract.Data.CONTENT_URI,
                    contentValues,
                    "${ContactsContract.Data._ID} = ?",
                    arrayOf(id.toString())
                )

                if (rowsUpdated > 0) {
                    Log.d("ContactBackendService", "Done")
                } else {
                    Log.e("ContactBackendService", "Failed to update ${contact.id}")
                }
            } else {
                Log.e("ContactBackendService", "No contact found in cursor for ${contact.name}")
            }
        } ?: Log.e("ContactBackendService", "Cursor is null for ${contact.name}")
    }
}

fun ContactEntity.toDto() = ContactDto(name, phoneNumber, email, address, notes)