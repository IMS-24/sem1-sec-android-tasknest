package at.avollmaier.tasknest.contacts.data

data class ContactDto(
    val address: String,
    val email: String,
    val id: Int,
    val name: String,
    val notes: String,
    val phoneNumber: String
)