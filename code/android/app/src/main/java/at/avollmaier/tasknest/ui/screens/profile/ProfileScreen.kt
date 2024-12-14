package at.avollmaier.tasknest.ui.screens.profile

import android.util.Log
import androidx.compose.foundation.Image
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.CircleShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AccountCircle
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Event
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.HorizontalDivider
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.draw.clip
import androidx.compose.ui.graphics.vector.ImageVector
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.auth.data.User
import at.avollmaier.tasknest.auth.domain.service.AuthProvider
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
                    .padding(24.dp),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                val painter = user.pictureURL?.let { rememberAsyncImagePainter(it) }

                Box(
                    modifier = Modifier
                        .size(140.dp)
                        .clip(CircleShape),
                    contentAlignment = Alignment.Center
                ) {
                    Image(
                        painter = painter
                            ?: painterResource(id = R.drawable.baseline_account_circle_24),
                        contentDescription = "Profile Picture",
                        modifier = Modifier.size(130.dp),
                        contentScale = ContentScale.Crop
                    )
                }

                Spacer(modifier = Modifier.height(20.dp))

                Text(
                    text = user.name ?: "Anonymous User",
                    style = MaterialTheme.typography.headlineSmall,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.padding(horizontal = 16.dp)
                )
                Text(
                    text = user.email ?: "Email not available",
                    style = MaterialTheme.typography.bodyMedium,
                    modifier = Modifier.padding(horizontal = 16.dp)
                )

                Spacer(modifier = Modifier.height(32.dp))

                HorizontalDivider(thickness = 1.dp)
                Spacer(modifier = Modifier.height(16.dp))

                Column(
                    modifier = Modifier.fillMaxWidth(),
                    verticalArrangement = Arrangement.spacedBy(12.dp)
                ) {
                    InfoRow(
                        icon = Icons.Default.AccountCircle,
                        label = "Nickname",
                        value = user.nickname
                    )
                    InfoRow(
                        icon = Icons.Default.CheckCircle,
                        label = "Email Verified",
                        value = if (user.isEmailVerified == true) "Yes" else "No"
                    )
                    InfoRow(
                        icon = Icons.Default.AccountCircle,
                        label = "Family Name",
                        value = user.familyName
                    )
                    InfoRow(
                        icon = Icons.Default.Event,
                        label = "Joined",
                        value = user.createdAt?.toString()
                    )
                }

                Spacer(modifier = Modifier.height(32.dp))

                Button(
                    onClick = {
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
                    },
                    modifier = Modifier
                        .fillMaxWidth()
                        .height(48.dp),
                    colors = ButtonDefaults.buttonColors(
                        containerColor = MaterialTheme.colorScheme.error,
                        contentColor = MaterialTheme.colorScheme.onError
                    ),
                    shape = MaterialTheme.shapes.medium
                ) {
                    Text("Logout", style = MaterialTheme.typography.labelLarge)
                }
            }
        }
    }
}

@Composable
fun InfoRow(icon: ImageVector, label: String, value: String?) {
    Row(
        modifier = Modifier
            .fillMaxWidth()
            .padding(horizontal = 16.dp),
        verticalAlignment = Alignment.CenterVertically
    ) {
        Icon(
            imageVector = icon,
            contentDescription = null,
            modifier = Modifier.size(24.dp)
        )
        Spacer(modifier = Modifier.width(12.dp))
        Column {
            Text(
                text = label,
                style = MaterialTheme.typography.bodyMedium,
            )
            Text(
                text = value ?: "Not available",
                style = MaterialTheme.typography.bodyLarge,
            )
        }
    }
}
