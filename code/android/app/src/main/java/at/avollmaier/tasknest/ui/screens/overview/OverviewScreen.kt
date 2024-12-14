package at.avollmaier.tasknest.ui.screens.overview

import android.app.DatePickerDialog
import android.app.TimePickerDialog
import android.content.Intent
import android.net.Uri
import android.provider.MediaStore
import androidx.activity.compose.rememberLauncherForActivityResult
import androidx.activity.result.contract.ActivityResultContracts
import androidx.compose.foundation.Image
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
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AccessTime
import androidx.compose.material.icons.filled.Add
import androidx.compose.material.icons.filled.MoreVert
import androidx.compose.material3.Button
import androidx.compose.material3.Card
import androidx.compose.material3.CardDefaults
import androidx.compose.material3.Checkbox
import androidx.compose.material3.DropdownMenu
import androidx.compose.material3.DropdownMenuItem
import androidx.compose.material3.ExperimentalMaterial3Api
import androidx.compose.material3.FloatingActionButton
import androidx.compose.material3.Icon
import androidx.compose.material3.IconButton
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.material3.TextField
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
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import androidx.lifecycle.viewmodel.compose.viewModel
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import kotlinx.coroutines.CoroutineScope
import kotlinx.coroutines.Dispatchers
import kotlinx.coroutines.launch
import java.time.Duration
import java.time.LocalDate
import java.time.LocalDateTime
import java.time.ZoneId
import java.time.ZonedDateTime
import java.time.format.DateTimeFormatter
import java.time.temporal.ChronoUnit
import java.util.Calendar

@OptIn(ExperimentalMaterial3Api::class)
@Composable
fun OverviewScreen(
    viewModel: OverviewViewModel = viewModel(factory = OverviewViewModelFactory(LocalContext.current))
) {
    val todos = viewModel.todos.collectAsState().value
    var isRefreshing by remember { mutableStateOf(false) }
    val coroutineScope = CoroutineScope(Dispatchers.IO)
    var showDialog by remember { mutableStateOf(false) }

    TaskNestTheme {
        PullToRefreshBox(
            isRefreshing = isRefreshing,
            onRefresh = {
                coroutineScope.launch {
                    isRefreshing = true
                    viewModel.refreshTodos()
                    isRefreshing = false
                }
            },
        ) {
            Surface(
                modifier = Modifier.fillMaxSize(),
                color = MaterialTheme.colorScheme.background
            ) {
                Box(modifier = Modifier.fillMaxSize()) {
                    Column(
                        modifier = Modifier
                            .padding(16.dp)
                            .fillMaxSize()
                            .align(Alignment.Center)
                    ) {
                        Row(
                            modifier = Modifier
                                .fillMaxWidth()
                                .padding(4.dp),
                            horizontalArrangement = Arrangement.SpaceBetween,
                            verticalAlignment = Alignment.CenterVertically
                        ) {
                            Column(
                                horizontalAlignment = Alignment.Start
                            ) {
                                Text(
                                    text = "My Todo's",
                                    style = MaterialTheme.typography.headlineLarge,
                                    fontWeight = FontWeight.Bold
                                )
                                Text(
                                    text = "${todos.size} Todo(s)",
                                    style = MaterialTheme.typography.bodyLarge
                                )
                            }
                            Dropdown(viewModel)
                        }
                        Spacer(modifier = Modifier.height(16.dp))
                        LazyColumn(
                            modifier = Modifier.fillMaxSize()
                        ) {
                            if (todos.isEmpty()) {
                                item {
                                    EmptyTodoState()
                                }
                            } else {
                                items(todos) { todo ->
                                    TodoCard(todo, viewModel)
                                }
                            }
                        }
                    }
                    FloatingActionButton(
                        onClick = { showDialog = true },
                        modifier = Modifier
                            .align(Alignment.BottomEnd)
                            .padding(16.dp)
                    ) {
                        Icon(Icons.Filled.Add, contentDescription = "Add Todo")
                    }
                }
            }
        }
    }

    if (showDialog) {
        AddTodoDialog(onDismiss = { showDialog = false }, viewModel = viewModel)
    }
}

@Composable
fun Dropdown(viewModel: OverviewViewModel) {
    var expanded by remember { mutableStateOf(false) }
    var showDialog by remember { mutableStateOf(false) }
    val coroutineScope = CoroutineScope(Dispatchers.IO)

    Box {
        IconButton(onClick = { expanded = true }) {
            Icons.Filled.MoreVert
        }
        DropdownMenu(
            expanded = expanded,
            onDismissRequest = { expanded = false }
        ) {
            DropdownMenuItem(
                text = { Text(text = "Add Todo") },
                onClick = {
                    expanded = false
                    showDialog = true
                }
            )
            DropdownMenuItem(
                text = { Text(text = "Refresh") },
                onClick = {
                    expanded = false
                    coroutineScope.launch {
                        viewModel.refreshTodos()
                    }
                }
            )
        }
    }

    if (showDialog) {
        AddTodoDialog(onDismiss = { showDialog = false }, viewModel = viewModel)
    }
}
@Composable
fun AddTodoDialog(onDismiss: () -> Unit, viewModel: OverviewViewModel) {
    var title by remember { mutableStateOf("") }
    var content by remember { mutableStateOf("") }
    var dueDate by remember { mutableStateOf<ZonedDateTime>(ZonedDateTime.now() + Duration.ofDays(1L)) }
    var attachments by remember { mutableStateOf<List<Uri>>(emptyList()) }
    val context = LocalContext.current

    val launcher =
        rememberLauncherForActivityResult(ActivityResultContracts.StartActivityForResult()) {
            it.data?.data?.let { uri -> attachments = attachments + uri }
        }

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
                    text = "Create a new Todo",
                    style = MaterialTheme.typography.headlineSmall,
                    fontWeight = FontWeight.Bold
                )
                Spacer(modifier = Modifier.height(8.dp))
                TextField(
                    value = title,
                    onValueChange = { title = it },
                    label = { Text("Title") },
                    modifier = Modifier.fillMaxWidth()
                )
                Spacer(modifier = Modifier.height(8.dp))
                TextField(
                    value = content,
                    onValueChange = { content = it },
                    label = { Text("Content") },
                    modifier = Modifier.fillMaxWidth()
                )
                Spacer(modifier = Modifier.height(8.dp))
                TextField(
                    value = dueDate.format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")),
                    onValueChange = {},
                    label = { Text("Due Date & Time") },
                    modifier = Modifier.fillMaxWidth(),
                    readOnly = true,
                    trailingIcon = {
                        IconButton(onClick = {
                            val calendar = Calendar.getInstance()
                            DatePickerDialog(
                                context,
                                { _, year, month, dayOfMonth ->
                                    TimePickerDialog(
                                        context,
                                        { _, hour, minute ->
                                            dueDate = ZonedDateTime.of(
                                                LocalDateTime.of(
                                                    year,
                                                    month + 1,
                                                    dayOfMonth,
                                                    hour,
                                                    minute
                                                ), ZoneId.systemDefault()
                                            )
                                        },
                                        calendar.get(Calendar.HOUR_OF_DAY),
                                        calendar.get(Calendar.MINUTE),
                                        true
                                    ).show()
                                },
                                calendar.get(Calendar.YEAR),
                                calendar.get(Calendar.MONTH),
                                calendar.get(Calendar.DAY_OF_MONTH)
                            ).show()
                        }) {
                            Icon(Icons.Filled.AccessTime, contentDescription = "Access Time")
                        }
                    }
                )
                Spacer(modifier = Modifier.height(8.dp))
                Button(onClick = {
                    val intent = Intent(
                        Intent.ACTION_OPEN_DOCUMENT,
                        MediaStore.Images.Media.EXTERNAL_CONTENT_URI
                    ).apply {
                        addCategory(Intent.CATEGORY_OPENABLE)
                    }
                    launcher.launch(intent)
                }) {
                    Text("Add Attachment")
                }
                Spacer(modifier = Modifier.height(8.dp))
                LazyColumn {
                    items(attachments) { attachment ->
                        Text(attachment.path?.substringAfterLast('/') ?: "Unknown")
                    }
                }
                Spacer(modifier = Modifier.height(16.dp))
                Row(
                    horizontalArrangement = Arrangement.End,
                    modifier = Modifier.fillMaxWidth()
                ) {
                    Button(onClick = { onDismiss() }) {
                        Text("Cancel")
                    }
                    Spacer(modifier = Modifier.width(8.dp))
                    Button(
                        onClick = {
                            viewModel.addTodo(
                                title,
                                content,
                                dueDate,
                                context = context,
                                attachments = attachments
                            )
                            onDismiss()
                        },
                        enabled = title.isNotBlank() && content.isNotBlank()
                    ) {
                        Text("Add")
                    }
                }
            }
        }
    }
}

@Composable
fun TodoCard(todo: FetchTodoDto, viewModel: OverviewViewModel) {
    var isChecked by remember { mutableStateOf(false) }

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
            verticalAlignment = Alignment.CenterVertically
        ) {
            Checkbox(
                checked = isChecked,
                onCheckedChange = {
                    isChecked = it
                    viewModel.finishTodo(todo)
                }
            )
            Spacer(modifier = Modifier.width(4.dp))
            Column {
                Text(
                    text = todo.title,
                    style = MaterialTheme.typography.titleMedium,
                    fontWeight = FontWeight.Bold
                )
                Row(


                ) {
                    Text(
                        text = todo.content,
                        style = MaterialTheme.typography.bodySmall
                    )
                    Spacer(modifier = Modifier.weight(1f))

                    val remainingDays = ChronoUnit.DAYS.between(LocalDate.now(), todo.dueUtc)
                    val dueText = when {
                        remainingDays < 0 -> "Overdue by ${-remainingDays} day(s)"
                        remainingDays == 0L -> "Due today!"
                        else -> "Due in $remainingDays day(s)"
                    }
                    if (todo.status != TodoStatus.DONE) {

                        Text(
                            text = dueText,
                            style = MaterialTheme.typography.bodySmall,
                            color = when {
                                remainingDays < 0 -> MaterialTheme.colorScheme.error
                                remainingDays == 0L -> MaterialTheme.colorScheme.secondary
                                else -> MaterialTheme.colorScheme.primary
                            }
                        )

                    }
                    Spacer(modifier = Modifier.width(8.dp))
                }

            }
        }
    }
}

@Composable
fun EmptyTodoState() {
    Box(
        modifier = Modifier.fillMaxSize(),
        contentAlignment = Alignment.Center
    ) {
        Column(
            horizontalAlignment = Alignment.CenterHorizontally
        ) {
            Image(
                painter = painterResource(id = R.drawable.nothing),
                contentDescription = "No todos",
                modifier = Modifier
                    .padding(vertical = 10.dp)
                    .size(300.dp)
            )
            Text(
                "No todos for for now",
                style = MaterialTheme.typography.titleLarge,
                fontWeight = FontWeight.Bold,
                modifier = Modifier.padding(vertical = 20.dp)
            )
            Text(
                "Add a new todo by clicking the button below",
                style = MaterialTheme.typography.bodyMedium
            )
        }
    }
}