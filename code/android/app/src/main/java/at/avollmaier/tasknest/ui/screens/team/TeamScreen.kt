package at.avollmaier.tasknest.ui.screens.team

import androidx.compose.foundation.layout.*
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import at.avollmaier.tasknest.ui.theme.TaskNestTheme

@Composable
fun TeamScreen(navController: NavController) {
    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            Text(
                "Team Screen",
                style = MaterialTheme.typography.titleLarge,
                modifier = Modifier.padding(vertical = 20.dp)
            )
        }
    }
}