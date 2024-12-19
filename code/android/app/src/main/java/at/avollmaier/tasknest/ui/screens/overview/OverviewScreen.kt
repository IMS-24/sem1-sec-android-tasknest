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
import androidx.compose.foundation.clickable
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
import androidx.compose.ui.text.input.TextFieldValue
import androidx.compose.ui.text.style.TextOverflow
import androidx.compose.ui.unit.dp
import androidx.compose.ui.window.Dialog
import androidx.lifecycle.viewmodel.compose.viewModel
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.PointDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import com.google.android.libraries.places.api.model.AutocompleteSessionToken
import com.google.android.libraries.places.api.model.Place
import com.google.android.libraries.places.api.net.FetchPlaceRequest
import com.google.android.libraries.places.api.net.FindAutocompletePredictionsRequest
import com.google.android.libraries.places.api.net.PlacesClient
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
    val hasNextPage by viewModel.hasNextPage.collectAsState()

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
                modifier = Modifier.fillMaxSize(), color = MaterialTheme.colorScheme.background
            ) {
                Box(modifier = Modifier.fillMaxSize()) {
                    Column(
                        modifier = Modifier
                            .padding(16.dp)
                            .fillMaxSize()
                            .align(Alignment.Center),
                    ) {
                        OverviewHeader(viewModel, viewModel.getTodosCount())
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
                                item {
                                    if (hasNextPage) {
                                        Box(
                                            modifier = Modifier.fillMaxWidth(),
                                            contentAlignment = Alignment.Center
                                        ) {
                                            Button(onClick = { viewModel.loadMoreTodos() }) {
                                                Text("Load More")
                                            }
                                        }
                                    }
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
fun OverviewHeader(viewModel: OverviewViewModel, count: Int) {
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
                text = "$count Todo(s)",
                style = MaterialTheme.typography.bodyLarge
            )
        }
        Dropdown(viewModel)
    }
}

@Composable
fun Dropdown(viewModel: OverviewViewModel) {
    var expanded by remember { mutableStateOf(false) }
    var showDialog by remember { mutableStateOf(false) }
    val coroutineScope = CoroutineScope(Dispatchers.IO)

    Box {
        IconButton(onClick = { expanded = true }) {
            Icon(Icons.Filled.MoreVert, contentDescription = "Dropdown Menu")


        }
        DropdownMenu(expanded = expanded, onDismissRequest = { expanded = false }) {
            DropdownMenuItem(text = { Text(text = "Add Todo") }, onClick = {
                expanded = false
                showDialog = true
            })
            DropdownMenuItem(text = { Text(text = "Refresh") }, onClick = {
                expanded = false
                coroutineScope.launch {
                    viewModel.refreshTodos()
                }
            })
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
    var searchedLocation by remember { mutableStateOf<PointDto?>(null) }

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
                TextField(value = title, onValueChange = {
                    if (it.length <= 50) title = it
                }, label = { Text("Title") }, modifier = Modifier.fillMaxWidth())
                Spacer(modifier = Modifier.height(8.dp))
                TextField(value = content, onValueChange = {
                    if (it.length <= 50) content = it
                }, label = { Text("Content") }, modifier = Modifier.fillMaxWidth())
                Spacer(modifier = Modifier.height(8.dp))
                TextField(value = dueDate.format(DateTimeFormatter.ofPattern("yyyy-MM-dd HH:mm")),
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
                                                    year, month + 1, dayOfMonth, hour, minute
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
                    })
                Spacer(modifier = Modifier.height(8.dp))
                SearchBar(
                    placesClient = viewModel.getPlaceClient(),
                    onPlaceSelected = { lat, lng ->
                        searchedLocation = PointDto(x = lat, y = lng)
                    })
                Spacer(modifier = Modifier.height(8.dp))
                Button(onClick = {
                    val intent = Intent(
                        Intent.ACTION_OPEN_DOCUMENT, MediaStore.Images.Media.EXTERNAL_CONTENT_URI
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
                    horizontalArrangement = Arrangement.End, modifier = Modifier.fillMaxWidth()
                ) {
                    Button(onClick = { onDismiss() }) {
                        Text("Cancel")
                    }
                    Spacer(modifier = Modifier.width(8.dp))
                    Button(
                        onClick = {
                            searchedLocation?.let {
                                viewModel.addTodo(
                                    title,
                                    content,
                                    dueDate,
                                    context = context,
                                    attachments = attachments,
                                    location = it
                                )
                            }
                            onDismiss()
                        },
                        enabled = title.isNotBlank() && content.isNotBlank() && searchedLocation != null
                    ) {
                        Text("Add")
                    }
                }
            }
        }
    }
}

@Composable
fun SearchBar(
    placesClient: PlacesClient,
    onPlaceSelected: (Double, Double) -> Unit
) {
    var text by remember { mutableStateOf(TextFieldValue("")) }
    var suggestions by remember { mutableStateOf(listOf<String>()) }

    val sessionToken = remember { AutocompleteSessionToken.newInstance() }

    Column(modifier = Modifier.fillMaxWidth()) {
        TextField(
            value = text,
            singleLine = true,
            onValueChange = { newText ->
                text = newText
                if (newText.text.isNotEmpty()) {
                    val request = FindAutocompletePredictionsRequest.builder()
                        .setSessionToken(sessionToken)
                        .setQuery(newText.text)
                        .build()

                    placesClient.findAutocompletePredictions(request)
                        .addOnSuccessListener { response ->
                            suggestions = response.autocompletePredictions.map {
                                it.getFullText(null).toString()
                            }
                        }
                        .addOnFailureListener { exception ->
                            exception.printStackTrace()
                        }
                } else {
                    suggestions = emptyList()
                }
            },
            modifier = Modifier
                .fillMaxWidth(),
            placeholder = {
                Text("Search for a place", style = MaterialTheme.typography.bodyMedium)
            }
        )

        Spacer(modifier = Modifier.height(8.dp))

        suggestions.take(5).forEach { suggestion ->
            Text(
                maxLines = 1,
                overflow = TextOverflow.Ellipsis,
                text = suggestion,
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(vertical = 4.dp)
                    .clickable {
                        text = TextFieldValue(suggestion)
                        suggestions = emptyList()

                        // Fetch the selected place details
                        val request = FindAutocompletePredictionsRequest
                            .builder()
                            .setSessionToken(sessionToken)
                            .setQuery(suggestion)
                            .build()

                        placesClient
                            .findAutocompletePredictions(request)
                            .addOnSuccessListener { response ->
                                val placeId =
                                    response.autocompletePredictions.firstOrNull()?.placeId
                                if (placeId != null) {
                                    val placeRequest = FetchPlaceRequest.newInstance(
                                        placeId,
                                        listOf(Place.Field.LOCATION)
                                    )

                                    placesClient
                                        .fetchPlace(placeRequest)
                                        .addOnSuccessListener { placeResponse ->
                                            val latLng = placeResponse.place.location
                                            if (latLng != null) {
                                                onPlaceSelected(latLng.latitude, latLng.longitude)
                                            }
                                        }
                                        .addOnFailureListener { fetchException ->
                                            fetchException.printStackTrace()
                                        }
                                }
                            }
                            .addOnFailureListener { exception ->
                                exception.printStackTrace()
                            }
                    }
            )
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
            Checkbox(checked = isChecked, onCheckedChange = {
                isChecked = it
                viewModel.finishTodo(todo)
            })
            Spacer(modifier = Modifier.width(4.dp))
            Column {
                Text(
                    overflow = TextOverflow.Ellipsis,
                    text = todo.title,
                    style = MaterialTheme.typography.titleMedium,
                    fontWeight = FontWeight.Bold,
                    maxLines = 1
                )
                Row(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(4.dp),
                    verticalAlignment = Alignment.CenterVertically,
                    horizontalArrangement = Arrangement.SpaceBetween,
                ) {
                    Text(
                        maxLines = 1,
                        overflow = TextOverflow.Ellipsis,
                        text = todo.content,
                        style = MaterialTheme.typography.bodySmall
                    )

                    TodoDueState(todo = todo)
                }

            }
        }
    }
}

@Composable
fun EmptyTodoState() {
    Box(
        modifier = Modifier.fillMaxSize(), contentAlignment = Alignment.Center
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

@Composable
fun TodoDueState(todo: FetchTodoDto) {
    val remainingDays = ChronoUnit.DAYS.between(LocalDate.now(), todo.dueUtc)
    val dueText = when {
        remainingDays < 0 -> "Overdue by ${-remainingDays} day(s)"
        remainingDays == 0L -> "Due today!"
        else -> "Due in $remainingDays day(s)"
    }
    if (todo.status == TodoStatus.NEW) {
        Text(
            maxLines = 1,
            text = dueText,
            style = MaterialTheme.typography.bodySmall,
            color = when {
                remainingDays < 0 -> MaterialTheme.colorScheme.error
                remainingDays == 0L -> MaterialTheme.colorScheme.secondary
                else -> MaterialTheme.colorScheme.primary
            }
        )
    }
}