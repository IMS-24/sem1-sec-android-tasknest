package at.avollmaier.tasknest.auth.domain.service

import android.content.Context
import android.util.Log
import at.avollmaier.tasknest.auth.data.ExternalUserDto
import at.avollmaier.tasknest.common.NetworkUtils
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import retrofit2.Response
import java.util.UUID

class ExternalUserService(context: Context) {
    private val ioScope = CoroutineScope(Dispatchers.IO)


    private val api: IExternalUserService =
        NetworkUtils.provideRetrofit(context).create(IExternalUserService::class.java)


    private fun <T> makeApiCall(
        apiCall: suspend () -> Response<T>,
        onSuccess: (T?) -> Unit,
        onError: () -> Unit
    ) {
        ioScope.launch {
            try {
                val response = apiCall()
                if (response.isSuccessful) {
                    onSuccess(response.body())
                } else {
                    Log.e("ExternalUserService", "API call failed: ${response.raw()}")
                    onError()
                }
            } catch (e: Exception) {
                Log.e("ExternalUserService", "Exception during API call", e)
                onError()
            }
        }
    }

    fun getUser(id: UUID, callback: (ExternalUserDto?) -> Unit) {
        makeApiCall(
            apiCall = { api.getUser(id).execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
    }
}