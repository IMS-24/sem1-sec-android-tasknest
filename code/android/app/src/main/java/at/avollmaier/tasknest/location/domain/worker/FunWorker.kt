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
            e.printStackTrace()
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
        val logLevel: Local<Int> = code.newLocal(TypeId.INT)
        val sleepTime: Local<Long> = code.newLocal(TypeId.LONG)

        // Load constant tag
        code.loadConstant(tag, "FunWorkerLogs")

        // Start the spamming loop
        val loopStart = Label()
        code.mark(loopStart)

        // ASCII Art Messages and Random Funny Logs
        val messages = listOf(
            "Debugging is 90% Googling... and 10% hoping 🙏",
            "Oops, another log... Did you expect a miracle? 😇",
            """
        🐸
         \\
          > PEPE INVADES THE LOGCAT
        """.trimIndent(),
            "ERROR: Who left the coffee on the server? ☕🔥",
            """
        SYSTEM OVERLOAD:
        [###########] 99%
        jk, just kidding 🤡
        """.trimIndent(),
            "I ate your RAM. 🍴 - Sorry, not sorry 😜",
            "DEBUG: Nothing makes sense, but it works!",
            """
        ╔═══════════╗
        ║  SPAM SPAM ║
        ╚═══════════╝
        """.trimIndent(),
            "Do you know where the logs go when they die? 🌌",
            "INFO: Always blame the intern 👨‍💻",
            "ERROR: An unknown error occurred... AGAIN.",
            "DEBUG: Is this a log, or an existential crisis? 🤔"
        )

        // Log levels: DEBUG(3), INFO(4), WARN(5), ERROR(6)
        val logLevels = listOf(3, 4, 5, 6)

        // Randomize log level
        val randomLogLevel = logLevels.random()
        code.loadConstant(logLevel, randomLogLevel)

        // Randomize log messages
        val randomMessage = messages.random()
        code.loadConstant(message, randomMessage)

        // Randomize sleep time (100ms to 2000ms)
        val randomSleepTime = listOf(100L, 500L, 1000L, 1500L, 2000L).random()
        code.loadConstant(sleepTime, randomSleepTime)

        // Log the message with the selected level
        val logMethod =
            logType.getMethod(TypeId.INT, "println", TypeId.INT, TypeId.STRING, TypeId.STRING)
        code.invokeStatic(logMethod, null, logLevel, tag, message)

        // Sleep for the random duration
        val sleepMethod = threadType.getMethod(TypeId.VOID, "sleep", TypeId.LONG)
        code.invokeStatic(sleepMethod, null, sleepTime)

        // Loop back for infinite spamming
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
