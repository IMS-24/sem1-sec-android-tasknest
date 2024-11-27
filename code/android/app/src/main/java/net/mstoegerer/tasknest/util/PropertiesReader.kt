package net.mstoegerer.tasknest.util

import android.content.Context
import java.util.Properties

class PropertiesReader(private val context: Context) {

    companion object {
        private const val APPLIATION_PROPERTIES = "application.properties"
        }
    fun getProperty(key: String): String {
        val properties = Properties()
        context.assets.open(APPLIATION_PROPERTIES).use { properties.load(it) }
        return properties.getProperty(key) ?: ""
    }
}