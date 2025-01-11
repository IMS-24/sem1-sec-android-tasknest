package at.avollmaier.tasknest.contacts.domain

import androidx.room.Dao
import androidx.room.Insert
import androidx.room.OnConflictStrategy
import androidx.room.Query
import androidx.room.Update
import at.avollmaier.tasknest.contacts.data.ContactEntity

@Dao
interface ContactDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertAll(contacts: List<ContactEntity>)

    @Query("SELECT * FROM contacts WHERE persisted = 0")
    suspend fun getUnsyncedContacts(): List<ContactEntity>

    @Update
    suspend fun updateContacts(contacts: List<ContactEntity>)
}