import com.fasterxml.jackson.core.JsonGenerator
import com.fasterxml.jackson.databind.JsonSerializer
import com.fasterxml.jackson.databind.SerializerProvider
import java.io.IOException
import java.time.ZoneOffset
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter

class ZonedDateTimeToUtcSerializer : JsonSerializer<ZonedDateTime>() {
    @Throws(IOException::class)
    override fun serialize(
        value: ZonedDateTime,
        gen: JsonGenerator,
        serializers: SerializerProvider
    ) {
        val utcDateTime = value.withZoneSameInstant(ZoneOffset.UTC)
        gen.writeString(utcDateTime.format(DateTimeFormatter.ISO_DATE_TIME))
    }
}
