package at.avollmaier.tasknest.auth.data

import java.util.Date

data class User(
    private val id: String?,
    val name: String?,
    val nickname: String?,
    val pictureURL: String?,
    val email: String?,
    val isEmailVerified: Boolean?,
    val familyName: String?,
    val createdAt: Date?
)
