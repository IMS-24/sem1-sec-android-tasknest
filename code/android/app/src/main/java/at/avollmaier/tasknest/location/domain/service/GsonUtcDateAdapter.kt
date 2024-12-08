package at.avollmaier.tasknest.location.domain.service

import com.google.gson.JsonDeserializationContext
import com.google.gson.JsonDeserializer
import com.google.gson.JsonElement
import com.google.gson.JsonParseException
import com.google.gson.JsonPrimitive
import com.google.gson.JsonSerializationContext
import com.google.gson.JsonSerializer
import java.lang.reflect.Type;
import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;
import java.util.TimeZone;

class GsonUTCDateAdapter : JsonSerializer<Date?>,
    JsonDeserializer<Date?> {
    override fun serialize(
        date: Date?,
        type: Type?,
        jsonSerializationContext: JsonSerializationContext?
    ): JsonElement {
        return JsonPrimitive(date?.let { newDateFormat.format(it) })
    }

    override fun deserialize(
        jsonElement: JsonElement,
        type: Type?,
        jsonDeserializationContext: JsonDeserializationContext?
    ): Date? {
        try {
            return newDateFormat.parse(jsonElement.asString)
        } catch (e: ParseException) {
            throw JsonParseException(e)
        }
    }

    private val newDateFormat: DateFormat
        get() {
            val dateFormat: DateFormat =
                SimpleDateFormat("yyyy-MM-dd'T'HH:mm:ss'Z'", Locale.getDefault())
            dateFormat.timeZone = TimeZone.getTimeZone("UTC")
            return dateFormat
        }
}