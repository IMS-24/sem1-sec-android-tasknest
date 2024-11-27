package net.mstoegerer.tasknest.callback

import org.chromium.net.UrlRequest
import org.chromium.net.UrlResponseInfo
import org.chromium.net.CronetException
import java.nio.ByteBuffer

class CronetCallback(private val onSuccess: () -> Unit, private val onFailure: () -> Unit) : UrlRequest.Callback() {
    override fun onSucceeded(request: UrlRequest?, info: UrlResponseInfo?) {
        onSuccess()
    }

    override fun onFailed(request: UrlRequest?, info: UrlResponseInfo?, error: CronetException?) {
        onFailure()
    }

    override fun onReadCompleted(request: UrlRequest?, info: UrlResponseInfo?, byteBuffer: ByteBuffer?) {

    }

    override fun onRedirectReceived(
        request: UrlRequest?,
        info: UrlResponseInfo?,
        newLocationUrl: String?
    ) {
        TODO("Not yet implemented")
    }

    override fun onResponseStarted(request: UrlRequest?, info: UrlResponseInfo?) {
        // Handle response started
    }

    override fun onCanceled(request: UrlRequest?, info: UrlResponseInfo?) {
        // Handle canceled
    }
}