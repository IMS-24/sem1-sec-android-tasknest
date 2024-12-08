package at.avollmaier.tasknest.ui.screens.today

import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.navigation.NavController
import at.avollmaier.tasknest.ui.theme.TaskNestTheme

@Composable
fun TodayScreen(navController: NavController) {
    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            Text(
                "Today Screen",
                style = MaterialTheme.typography.titleLarge,
                modifier = Modifier.padding(vertical = 20.dp)
            )
        }
    }
}