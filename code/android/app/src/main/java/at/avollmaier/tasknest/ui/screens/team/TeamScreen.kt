package at.avollmaier.tasknest.ui.screens.team

import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.foundation.text.KeyboardActions
import androidx.compose.foundation.text.KeyboardOptions
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.Share
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.OutlinedTextField
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.pulltorefresh.PullToRefreshBox
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.input.ImeAction
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import androidx.lifecycle.viewmodel.compose.viewModel
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.util.UUID

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun TeamScreen(
    teamViewModel: TeamViewModel = viewModel(factory = TeamViewModelFactory(LocalContext.current))

) {
    val todos by teamViewModel.todos.collectAsState()
    val coroutineScope = CoroutineScope(Dispatchers.IO)
    var isRefreshing by remember { mutableStateOf(false) }
    val hasNextPage by teamViewModel.hasNextPage.collectAsState()

    TaskNestTheme {
        PullToRefreshBox(
            isRefreshing = isRefreshing,
            onRefresh = {
                coroutineScope.launch {
                    isRefreshing = true
                    teamViewModel.refreshTodos()
                    isRefreshing = false
                }
            },
        ) {
            Surface(
                modifier = Modifier.fillMaxSize(), color = MaterialTheme.colorScheme.background
            ) {
                Box(modifier = Modifier.fillMaxSize()) {
                    Column(
                        modifier = Modifier
                            .padding(16.dp)
                            .fillMaxSize()
                            .align(Alignment.Center)
                    ) {

                        TeamTodos(todos, teamViewModel, hasNextPage)
                    }
                }
            }
        }


    }
}

@Composable
fun TeamTodos(
    todos: List<FetchTodoDto>,
    teamViewModel: TeamViewModel,
    hasNextPage: Boolean
) {
    Text(
        text = "Team Todo's",
        style = MaterialTheme.typography.headlineLarge,
        fontWeight = FontWeight.Bold
    )

    Spacer(modifier = Modifier.height(8.dp))

    LazyColumn(
        modifier = Modifier.fillMaxWidth(),
        verticalArrangement = Arrangement.spacedBy(8.dp)
    ) {
        items(todos) { todo ->
            ShareTodoCard(todo, teamViewModel)
        }
        item {
            if (hasNextPage) {
                Box(
                    modifier = Modifier.fillMaxWidth(),
                    contentAlignment = Alignment.Center
                ) {
                    Button(onClick = { teamViewModel.loadMoreTodos() }) {
                        Text("Load More")
                    }
                }
            }
        }
    }
}

@Composable
fun ShareTodoCard(todo: FetchTodoDto, teamViewModel: TeamViewModel) {
    var selectedTodoId by remember { mutableStateOf<UUID?>(null) }
    var showDialog by remember { mutableStateOf(false) }

    Card(
        shape = RoundedCornerShape(4.dp),
        colors = CardDefaults.cardColors(
            containerColor = Color.LightGray.copy(alpha = 0.2f)
        ),
        modifier = Modifier
            .fillMaxWidth()
            .background(MaterialTheme.colorScheme.surface)
            .padding(8.dp)
    ) {
        Row(
            modifier = Modifier
                .fillMaxWidth()
                .padding(4.dp),
            verticalAlignment = Alignment.CenterVertically,
            horizontalArrangement = Arrangement.SpaceBetween
        ) {
            Column {
                Text(
                    overflow = TextOverflow.Ellipsis,
                    text = todo.title,
                    style = MaterialTheme.typography.titleMedium,
                    fontWeight = FontWeight.Bold,
                    maxLines = 1
                )
                Text(
                    maxLines = 1,
                    overflow = TextOverflow.Ellipsis,
                    text = todo.content,
                    style = MaterialTheme.typography.bodySmall
                )
            }

            IconButton(onClick = {
                selectedTodoId = todo.id
                showDialog = true
            }) {
                Icon(Icons.Filled.Share, contentDescription = "Share")
            }
        }
    }

    if (showDialog && selectedTodoId != null) {
        ShareTodoDialog(
            onDismiss = { showDialog = false },
            onShare = { userId ->
                teamViewModel.shareTodoWithUser(selectedTodoId!!, UUID.fromString(userId))
                showDialog = false
            }
        )
    }
}

@Composable
fun ShareTodoDialog(
    onDismiss: () -> Unit,
    onShare: (String) -> Unit
) {
    var userIdToShare by remember { mutableStateOf("") }

    Dialog(onDismissRequest = onDismiss) {
        Surface(
            shape = RoundedCornerShape(16.dp),
            color = MaterialTheme.colorScheme.background,
            modifier = Modifier.padding(16.dp)
        ) {
            Column(
                modifier = Modifier.padding(16.dp),
                horizontalAlignment = Alignment.CenterHorizontally
            ) {
                Text(
                    text = "Share TODO",
                    style = MaterialTheme.typography.headlineSmall,
                    fontWeight = FontWeight.Bold
                )
                Spacer(modifier = Modifier.height(8.dp))
                Text("Enter the User ID to share the TODO with:")
                Spacer(modifier = Modifier.height(8.dp))
                OutlinedTextField(
                    value = userIdToShare,
                    onValueChange = { userIdToShare = it },
                    label = { Text("User ID") },
                    singleLine = true,
                    keyboardOptions = KeyboardOptions(imeAction = ImeAction.Done),
                    keyboardActions = KeyboardActions(onDone = { onDismiss() })
                )
                Spacer(modifier = Modifier.height(16.dp))
                Row(
                    horizontalArrangement = Arrangement.End,
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Button(onClick = onDismiss) {
                        Text("Cancel")
                    }
                    Spacer(modifier = Modifier.width(8.dp))
                    Button(onClick = {
                        if (userIdToShare.isNotBlank()) {
                            onShare(userIdToShare)
                        }
                    }, enabled = userIdToShare.isNotBlank() && isValidUUID(userIdToShare)) {
                        Text("Share")
                    }
                }
            }
        }
    }
}

fun isValidUUID(userIdToShare: String): Boolean {
    return try {
        UUID.fromString(userIdToShare)
        true
    } catch (e: IllegalArgumentException) {
        false
    }
}
