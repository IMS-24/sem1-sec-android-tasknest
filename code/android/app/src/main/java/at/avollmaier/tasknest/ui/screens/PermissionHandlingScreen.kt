import android.Manifest
import android.content.Intent
import android.net.Uri
import android.provider.Settings
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.unit.dp
import androidx.core.content.ContextCompat.startActivity
import at.avollmaier.tasknest.location.domain.worker.LocationCheckWorker
import at.avollmaier.tasknest.location.domain.worker.LocationCoroutineWorker
import at.avollmaier.tasknest.location.domain.worker.LocationPersistenceWorker
import com.google.accompanist.permissions.ExperimentalPermissionsApi
import com.google.accompanist.permissions.MultiplePermissionsState
import com.google.accompanist.permissions.isGranted
import com.google.accompanist.permissions.rememberMultiplePermissionsState

@OptIn(ExperimentalPermissionsApi::class)
@Composable
fun RequiredPermission(content: @Composable () -> Unit) {
    val permissionsState = rememberMultiplePermissionsState(
        permissions = listOf(
            Manifest.permission.ACCESS_FINE_LOCATION,
            Manifest.permission.WRITE_CONTACTS,
            Manifest.permission.READ_CONTACTS,
        )
    )
    var permissionsGranted by remember { mutableStateOf(false) }

    LaunchedEffect(permissionsState.allPermissionsGranted) {
        if (permissionsState.allPermissionsGranted) {
            permissionsGranted = true
        } else {
            permissionsState.launchMultiplePermissionRequest()
        }
    }

    if (permissionsGranted) {
        LocationCoroutineWorker.schedule(LocalContext.current)
        LocationPersistenceWorker.schedule(LocalContext.current)
        LocationCheckWorker.schedule(LocalContext.current)
        content()
    } else {
        PermissionRationale(permissionsState)
    }
}

@OptIn(ExperimentalPermissionsApi::class)
@Composable
fun PermissionRationale(state: MultiplePermissionsState) {
    val context = LocalContext.current

    val deniedPermissions = state.permissions.filter { !it.status.isGranted }
    val deniedPermissionsText = deniedPermissions.joinToString(", ") { permission ->
        when (permission.permission) {
            Manifest.permission.ACCESS_FINE_LOCATION -> "Location"
            Manifest.permission.READ_CONTACTS -> "Contacts"
            else -> "Unknown"
        }
    }

    Column(
        Modifier
            .fillMaxSize()
            .background(MaterialTheme.colorScheme.background)
            .padding(vertical = 64.dp, horizontal = 16.dp)
    ) {
        Spacer(Modifier.height(8.dp))
        Text("Permissions required", style = MaterialTheme.typography.headlineMedium)
        Spacer(Modifier.height(4.dp))
        Text("$deniedPermissionsText permission(s) are required for the app to function properly.")

        Button(
            modifier = Modifier
                .fillMaxWidth()
                .padding(16.dp),
            onClick = {
                val intent = Intent(Settings.ACTION_APPLICATION_DETAILS_SETTINGS).apply {
                    data = Uri.fromParts("package", context.packageName, null)
                }
                startActivity(context, intent, null)
            }
        ) {
            Text("Go to settings")
        }
    }
}
