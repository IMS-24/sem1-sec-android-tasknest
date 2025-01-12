package at.avollmaier.tasknest.contacts.domain.service

import android.Manifest
import android.content.Context
import android.content.pm.PackageManager
import android.provider.ContactsContract
import android.util.Log
import androidx.core.app.ActivityCompat
import at.avollmaier.tasknest.contacts.data.ContactEntity
import at.avollmaier.tasknest.contacts.domain.ContactDatabase
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch

class ContactDatabaseService(private val context: Context) {
    private val contactDao = ContactDatabase.getDatabase(context).contactDao()
    private val backendService = ContactBackendService(context)

    fun fetchAndStoreContacts() {
        if (ActivityCompat.checkSelfPermission(
                context,
                Manifest.permission.READ_CONTACTS
            ) != PackageManager.PERMISSION_GRANTED
        ) {
            Log.e("ContactDatabaseService", "Permission denied")
            return
        }
        CoroutineScope(Dispatchers.IO).launch {
            val contacts = fetchContacts()
            contactDao.insertAll(contacts)
            backendService.syncContacts()
        }
    }

    private fun fetchContacts(): List<ContactEntity> {
        val contentResolver = context.contentResolver
        val cursor = contentResolver.query(
            ContactsContract.Contacts.CONTENT_URI, null, null, null, null
        ) ?: return emptyList()

        val contacts = mutableListOf<ContactEntity>()
        cursor.use {
            while (it.moveToNext()) {
                val id = it.getString(it.getColumnIndexOrThrow(ContactsContract.Contacts._ID))
                val name =
                    it.getString(it.getColumnIndexOrThrow(ContactsContract.Contacts.DISPLAY_NAME))
                        ?: "Unknown"
                val phone = fetchDetail(
                    id,
                    ContactsContract.CommonDataKinds.Phone.CONTENT_URI,
                    ContactsContract.CommonDataKinds.Phone.NUMBER
                )
                contacts.add(
                    ContactEntity(
                        name = name,
                        phoneNumber = phone,
                        email = "",
                        address = "",
                        notes = ""
                    )
                )
            }
        }
        return contacts
    }

    private fun fetchDetail(contactId: String, uri: android.net.Uri, column: String): String {
        val cursor = context.contentResolver.query(
            uri,
            null,
            "${ContactsContract.Data.CONTACT_ID} = ?",
            arrayOf(contactId),
            null
        )
        var result = ""
        cursor?.use {
            if (it.moveToFirst()) result = it.getString(it.getColumnIndexOrThrow(column)) ?: ""
        }
        return result
    }
}