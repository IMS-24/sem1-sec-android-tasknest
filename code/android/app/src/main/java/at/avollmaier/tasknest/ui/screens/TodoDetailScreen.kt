package at.avollmaier.tasknest.ui.screens

import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.ui.theme.TaskNestTheme

@Composable
fun TodoDetailScreen(todo: FetchTodoDto) {
    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            Column(modifier = Modifier.padding(16.dp)) {
                Text(text = "Title: ${todo.title}", style = MaterialTheme.typography.titleMedium)
                Text(text = "Content: ${todo.content}", style = MaterialTheme.typography.bodyMedium)
                Text(text = "Status: ${todo.status}", style = MaterialTheme.typography.bodyMedium)
                Text(text = "Due Date: ${todo.dueUtc}", style = MaterialTheme.typography.bodyMedium)
                // Add more fields as needed
            }
        }
    }
}