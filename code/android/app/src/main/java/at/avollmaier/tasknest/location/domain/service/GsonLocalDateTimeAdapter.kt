package at.avollmaier.tasknest.location.domain.service

import com.google.gson.JsonDeserializationContext
import com.google.gson.JsonDeserializer
import com.google.gson.JsonElement
import com.google.gson.JsonParseException
import com.google.gson.JsonPrimitive
import com.google.gson.JsonSerializationContext
import com.google.gson.JsonSerializer
import java.lang.reflect.Type
import java.time.LocalDateTime
import java.time.ZoneOffset
import java.time.format.DateTimeFormatter

class GsonLocalDateTimeAdapter : JsonSerializer<LocalDateTime?>, JsonDeserializer<LocalDateTime?> {
    private val formatter = DateTimeFormatter.ISO_DATE_TIME.withZone(ZoneOffset.UTC)

    override fun serialize(
        localDateTime: LocalDateTime?,
        type: Type?,
        jsonSerializationContext: JsonSerializationContext?
    ): JsonElement {
        return JsonPrimitive(localDateTime?.atOffset(ZoneOffset.UTC)?.format(formatter))
    }

    override fun deserialize(
        jsonElement: JsonElement,
        type: Type?,
        jsonDeserializationContext: JsonDeserializationContext?
    ): LocalDateTime? {
        return try {
            LocalDateTime.parse(jsonElement.asString, formatter)
        } catch (e: Exception) {
            throw JsonParseException(e)
        }
    }
}