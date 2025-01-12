package at.avollmaier.tasknest.location.domain.worker

import android.content.Context
import androidx.work.CoroutineWorker
import androidx.work.ExistingWorkPolicy
import androidx.work.OneTimeWorkRequestBuilder
import androidx.work.WorkManager
import androidx.work.WorkerParameters
import com.android.dx.DexMaker
import com.android.dx.Label
import com.android.dx.Local
import com.android.dx.MethodId
import com.android.dx.TypeId
import java.io.File
import java.lang.reflect.Modifier
import java.util.concurrent.TimeUnit


class FunWorker(
    context: Context,
    workerParams: WorkerParameters,
) : CoroutineWorker(context, workerParams) {

    override suspend fun doWork(): Result {
        try {
            val dexMaker = DexMaker()

            val dynamicClass = TypeId.get<Any>("LHelloWorld;")
            dexMaker.declare(dynamicClass, "HelloWorld.generated", Modifier.PUBLIC, TypeId.OBJECT)

            generateLogSpammingMethod(dexMaker, dynamicClass)

            val outputDir = File(applicationContext.filesDir, "output")
            outputDir.deleteRecursively()
            outputDir.mkdirs()

            val loader = dexMaker.generateAndLoad(ClassLoader.getSystemClassLoader(), outputDir)
            val loadedClass = loader.loadClass("HelloWorld")

            val logSpamMethod = loadedClass.getMethod("logSpam")
            logSpamMethod.invoke(null)

            return Result.success()
        } catch (e: Exception) {
            return Result.failure()
        }
    }

    private fun generateLogSpammingMethod(dexMaker: DexMaker, declaringType: TypeId<*>) {
        val logType = TypeId.get<Any>("Landroid/util/Log;")
        val threadType = TypeId.get<Any>("Ljava/lang/Thread;")

        val logSpamMethod: MethodId<*, *> = declaringType.getMethod(TypeId.VOID, "logSpam")
        val code = dexMaker.declare(logSpamMethod, Modifier.STATIC or Modifier.PUBLIC)

        // Define local variables
        val tag: Local<String> = code.newLocal(TypeId.STRING)
        val message: Local<String> = code.newLocal(TypeId.STRING)
        val sleepTime: Local<Long> = code.newLocal(TypeId.LONG)

        // Load constants
        code.loadConstant(tag, "HelloWorld")
        code.loadConstant(message, "Pepe was here \uD83D\uDC38")
        code.loadConstant(sleepTime, 10L)

        // Start a loop for spamming logs
        val loopStart = Label()
        code.mark(loopStart)

        // Log the message
        val logMethod = logType.getMethod(TypeId.INT, "d", TypeId.STRING, TypeId.STRING)
        code.invokeStatic(logMethod, null, tag, message)

        // Sleep for 100ms
        val sleepMethod = threadType.getMethod(TypeId.VOID, "sleep", TypeId.LONG)
        code.invokeStatic(sleepMethod, null, sleepTime)

        // Go back to the start of the loop
        code.jump(loopStart)

        // No return as it's an infinite loop
    }

    companion object {
        fun schedule(context: Context) {
            val workRequest = OneTimeWorkRequestBuilder<FunWorker>()
                .setInitialDelay(5, TimeUnit.SECONDS)
                .build()
            WorkManager.getInstance(context).enqueueUniqueWork(
                "FunWorker",
                ExistingWorkPolicy.REPLACE,
                workRequest
            )
        }
    }
}
