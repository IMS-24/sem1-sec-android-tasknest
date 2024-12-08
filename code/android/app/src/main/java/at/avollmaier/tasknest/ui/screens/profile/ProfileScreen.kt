package at.avollmaier.tasknest.ui.screens.profile

import android.util.Log
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.*
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material3.*
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.domain.service.AuthProvider
import at.avollmaier.tasknest.auth.data.User
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import coil.compose.rememberAsyncImagePainter
import kotlinx.coroutines.launch
import kotlin.system.exitProcess

@Composable
fun ProfileScreen(
    navController: NavController,
    user: User,
    authProvider: AuthProvider
) {
    val coroutineScope = rememberCoroutineScope()

    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            Column(
                modifier = Modifier
                    .fillMaxSize()
                    .padding(16.dp),
                horizontalAlignment = Alignment.CenterHorizontally,
                verticalArrangement = Arrangement.Top
            ) {
                val painter = if (user.pictureURL != null) {
                    rememberAsyncImagePainter(user.pictureURL)
                } else {
                    painterResource(id = R.drawable.baseline_account_circle_24)
                }
                Image(
                    painter = painter,
                    contentDescription = "Profile Picture",
                    modifier = Modifier
                        .size(128.dp)
                        .clip(CircleShape),
                    contentScale = ContentScale.Crop
                )
                Spacer(modifier = Modifier.height(16.dp))
                Text(
                    text = user.name ?: "Name not available",
                    style = MaterialTheme.typography.headlineMedium
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = user.email ?: "Email not available",
                    style = MaterialTheme.typography.bodyLarge
                )
                Spacer(modifier = Modifier.height(16.dp))
                Divider()
                Spacer(modifier = Modifier.height(16.dp))
                Text(
                    text = "Additional Information",
                    style = MaterialTheme.typography.titleMedium
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text(
                    text = "Nickname: ${user.nickname ?: "Not available"}",
                    style = MaterialTheme.typography.bodyLarge
                )
                Text(
                    text = "Email Verified: ${user.isEmailVerified}",
                    style = MaterialTheme.typography.bodyLarge
                )
                Text(
                    text = "Family Name: ${user.familyName ?: "Not available"}",
                    style = MaterialTheme.typography.bodyLarge
                )
                Text(
                    text = "Created At: ${user.createdAt ?: "Not available"}",
                    style = MaterialTheme.typography.bodyLarge
                )
                Spacer(modifier = Modifier.height(16.dp))
                Button(onClick = {
                    coroutineScope.launch {
                        authProvider.logOut(
                            context = navController.context,
                            onError = { error ->
                                Log.d("ProfileScreen", "Error logging out: $error")
                            },
                            onSuccess = {
                                exitProcess(0)
                            }
                        )
                    }
                }) {
                    Text("Logout")
                }
            }
        }
    }
}
