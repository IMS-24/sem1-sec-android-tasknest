package at.avollmaier.tasknest.contacts.domain.service

import ContactBackendService
import android.Manifest
import android.annotation.SuppressLint
import android.content.Context
import android.content.pm.PackageManager
import android.provider.ContactsContract
import android.util.Log
import androidx.core.app.ActivityCompat
import at.avollmaier.tasknest.contacts.data.ContactDto
import at.avollmaier.tasknest.contacts.data.ContactEntity
import at.avollmaier.tasknest.contacts.domain.ContactDatabase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ContactDatabaseService(private val context: Context) {
    private val contactDao = ContactDatabase.getDatabase(context).contactDao()
    private val backendService = ContactBackendService(context)
    private val ioScope = CoroutineScope(Dispatchers.IO)

    private fun hasContactPermission(): Boolean {
        return ActivityCompat.checkSelfPermission(
            context,
            Manifest.permission.WRITE_CONTACTS
        ) == PackageManager.PERMISSION_GRANTED
    }


    @SuppressLint("Range")
    private fun fetchPhoneNumber(contactId: String): String {
        val contentResolver = context.contentResolver
        val phoneCursor = contentResolver.query(
            ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
            null,
            "${ContactsContract.CommonDataKinds.Phone.CONTACT_ID} = ?",
            arrayOf(contactId),
            null
        )
        var phoneNumber = ""
        phoneCursor?.use {
            if (it.moveToFirst()) {
                phoneNumber =
                    it.getString(it.getColumnIndex(ContactsContract.CommonDataKinds.Phone.NUMBER))
                        ?: ""
            }
        }
        return phoneNumber
    }

    @SuppressLint("Range")
    private fun fetchEmail(contactId: String): String {
        val contentResolver = context.contentResolver
        val emailCursor = contentResolver.query(
            ContactsContract.CommonDataKinds.Email.CONTENT_URI,
            null,
            "${ContactsContract.CommonDataKinds.Email.CONTACT_ID} = ?",
            arrayOf(contactId),
            null
        )
        var email = ""
        emailCursor?.use {
            if (it.moveToFirst()) {
                email =
                    it.getString(it.getColumnIndex(ContactsContract.CommonDataKinds.Email.ADDRESS))
                        ?: ""
            }
        }
        return email
    }

    @SuppressLint("Range")
    private fun fetchAddress(contactId: String): String {
        val contentResolver = context.contentResolver
        val addressCursor = contentResolver.query(
            ContactsContract.CommonDataKinds.StructuredPostal.CONTENT_URI,
            null,
            "${ContactsContract.CommonDataKinds.StructuredPostal.CONTACT_ID} = ?",
            arrayOf(contactId),
            null
        )
        var address = ""
        addressCursor?.use {
            if (it.moveToFirst()) {
                address =
                    it.getString(it.getColumnIndex(ContactsContract.CommonDataKinds.StructuredPostal.FORMATTED_ADDRESS))
                        ?: ""
            }
        }
        return address
    }

    @SuppressLint("Range")
    private fun fetchNotes(contactId: String): String {
        val contentResolver = context.contentResolver
        val notesCursor = contentResolver.query(
            ContactsContract.Data.CONTENT_URI,
            null,
            "${ContactsContract.Data.CONTACT_ID} = ? AND ${ContactsContract.Data.MIMETYPE} = ?",
            arrayOf(contactId, ContactsContract.CommonDataKinds.Note.CONTENT_ITEM_TYPE),
            null
        )
        var notes = ""
        notesCursor?.use {
            if (it.moveToFirst()) {
                notes = it.getString(it.getColumnIndex(ContactsContract.CommonDataKinds.Note.NOTE))
                    ?: ""
            }
        }
        return notes
    }


    @SuppressLint("Range")
    private fun fetchContacts(): List<ContactDto> {
        val contactList = mutableListOf<ContactDto>()
        val contentResolver = context.contentResolver
        val cursor = contentResolver.query(
            ContactsContract.Contacts.CONTENT_URI,
            null, null, null, null
        )

        cursor?.use {
            while (it.moveToNext()) {
                val id = it.getString(it.getColumnIndex(ContactsContract.Contacts._ID))
                val name = it.getString(it.getColumnIndex(ContactsContract.Contacts.DISPLAY_NAME))
                    ?: "Unknown"

                val phoneNumber = fetchPhoneNumber(id)
                val email = fetchEmail(id)
                val address = fetchAddress(id)
                val notes = fetchNotes(id)

                contactList.add(
                    ContactDto(
                        androidId = id.toInt(),
                        name = name,
                        phone = phoneNumber,
                        email = email,
                        address = address,
                        notes = notes
                    )
                )
            }
        }
        return contactList
    }

    @SuppressLint("MissingPermission")
    fun fetchAndStoreContacts() {
        if (!hasContactPermission()) {
            Log.e("ContactService", "Contact permission not granted")
            return
        }

        ioScope.launch {
            try {
                val contacts = fetchContacts().map { dto ->
                    ContactEntity(
                        name = dto.name,
                        address = dto.address,
                        email = dto.email,
                        notes = dto.notes,
                        phoneNumber = dto.phone
                    )
                }
                contactDao.insertAll(contacts)
                Log.d("ContactService", "Stored ${contacts.size} contacts in database")
                syncContactsToBackend()
            } catch (e: Exception) {
                Log.e("ContactService", "Error fetching/storing contacts", e)
            }
        }
    }

    private suspend fun syncContactsToBackend() {
        val isSynced = backendService.publishOfflinePersistedContacts()
        if (isSynced) {
            Log.d("ContactService", "Contacts successfully synced.")
        } else {
            Log.e("ContactService", "Failed to sync contacts.")
        }
    }
}
