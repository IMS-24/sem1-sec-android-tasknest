package at.avollmaier.tasknest.contacts.data

import androidx.room.Entity
import androidx.room.PrimaryKey

@Entity(tableName = "contacts")
data class ContactEntity(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    val name: String,
    val phoneNumber: String,
    val email: String,
    val address: String,
    val notes: String,
    var persisted: Boolean = false
) {
    override fun toString(): String {
        return "id=$id,\nname=$name,\nphoneNumber=$phoneNumber,\nemail=$email,\naddress=$address,\nnotes=$notes"
    }
}