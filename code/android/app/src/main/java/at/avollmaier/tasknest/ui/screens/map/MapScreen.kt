package at.avollmaier.tasknest.ui.screens.map

import android.annotation.SuppressLint
import android.os.Bundle
import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.width
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.CheckCircle
import androidx.compose.material.icons.filled.Pending
import androidx.compose.material3.Button
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.DisposableEffect
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.toArgb
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.unit.dp
import androidx.compose.ui.viewinterop.AndroidView
import androidx.compose.ui.window.Dialog
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.todo.domain.service.TodoService
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.MapView
import com.google.android.gms.maps.model.BitmapDescriptorFactory
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions
import java.time.LocalDate
import java.time.temporal.ChronoUnit

@Composable
fun MapScreen() {
    val context = LocalContext.current
    var todos by remember { mutableStateOf<List<FetchTodoDto>>(emptyList()) }
    var mapSelectedTodo by remember { mutableStateOf<FetchTodoDto?>(null) }

    LaunchedEffect(Unit) {
        TodoService(context).getTodos { fetchedTodos ->
            if (fetchedTodos != null) {
                todos = fetchedTodos
            }
        }
    }

    val locationPoints = remember(todos) {
        todos.map { todo ->
            val point = LatLng(todo.location.x, todo.location.y)
            Pair(point, todo)
        }
    }

    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            GoogleMapView(locationPoints) { selectedTodo ->
                mapSelectedTodo = selectedTodo
            }

            mapSelectedTodo?.let { todo ->
                TodoDetailDialog(todo = todo) {
                    mapSelectedTodo = null // Dismiss dialog on close
                }
            }
        }
    }
}

@SuppressLint("PotentialBehaviorOverride")
@Composable
fun GoogleMapView(
    locationPoints: List<Pair<LatLng, FetchTodoDto>>,
    onMarkerClick: (FetchTodoDto) -> Unit
) {
    val mapView = rememberMapViewWithLifecycle()
    val markersAdded =
        remember { mutableStateOf(false) }

    AndroidView({ mapView }) { mapView ->
        mapView.getMapAsync { googleMap ->
            googleMap.uiSettings.isZoomControlsEnabled = true

            if (locationPoints.isNotEmpty() && !markersAdded.value) {
                locationPoints.firstOrNull { value -> value.second.status == TodoStatus.NEW }?.let {
                    googleMap.moveCamera(
                        CameraUpdateFactory.newLatLngZoom(
                            it.first,
                            15f
                        )
                    )
                }


                locationPoints.forEach { (point, todo) ->
                    val markerOptions = MarkerOptions()
                        .position(point)
                        .alpha(if (todo.status == TodoStatus.DONE) 0.3f else 1.0f) // Transparency for Done
                        .icon(
                            if (todo.status == TodoStatus.DONE) {
                                BitmapDescriptorFactory.defaultMarker(BitmapDescriptorFactory.HUE_GREEN)
                            } else {
                                BitmapDescriptorFactory.defaultMarker(BitmapDescriptorFactory.HUE_RED)
                            }
                        )

                    googleMap.addMarker(markerOptions)?.tag = todo

                    if (todo.status != TodoStatus.DONE) {
                        googleMap.addCircle(
                            com.google.android.gms.maps.model.CircleOptions()
                                .center(point)
                                .radius(250.0)
                                .strokeWidth(2f)
                                .strokeColor(android.graphics.Color.RED)
                                .fillColor(Color.Red.copy(alpha = 0.3f).toArgb())
                        )
                    }
                }
                markersAdded.value = true
            }


            googleMap.setOnMarkerClickListener { marker ->
                val selectedTodo = marker.tag as? FetchTodoDto
                selectedTodo?.let { onMarkerClick(it) }
                true
            }
        }
    }
}


@Composable
fun rememberMapViewWithLifecycle(): MapView {
    val context = LocalContext.current
    val mapView = remember { MapView(context) }

    DisposableEffect(mapView) {
        mapView.onCreate(Bundle())
        mapView.onResume()

        onDispose {
            mapView.onPause()
            mapView.onDestroy()
        }
    }

    return mapView
}

@Composable
fun TodoDetailDialog(
    todo: FetchTodoDto,
    onDismiss: () -> Unit
) {
    Dialog(onDismissRequest = onDismiss) {
        Surface(
            shape = RoundedCornerShape(20.dp),
            tonalElevation = 4.dp,
            color = MaterialTheme.colorScheme.surface,
            modifier = Modifier.padding(20.dp)
        ) {
            Column(
                modifier = Modifier
                    .fillMaxWidth()
                    .padding(24.dp),
                verticalArrangement = Arrangement.spacedBy(16.dp)
            ) {
                Text(
                    text = todo.title,
                    style = MaterialTheme.typography.headlineSmall,
                    fontWeight = FontWeight.Bold,
                    modifier = Modifier.fillMaxWidth(),
                    color = MaterialTheme.colorScheme.primary
                )

                Surface(
                    shape = RoundedCornerShape(12.dp),
                    color = MaterialTheme.colorScheme.surfaceVariant,
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(vertical = 8.dp)
                ) {
                    Text(
                        text = todo.content,
                        style = MaterialTheme.typography.bodyMedium,
                        modifier = Modifier.padding(16.dp)
                    )
                }

                Row(verticalAlignment = Alignment.CenterVertically) {
                    Icon(
                        imageVector = if (todo.status == TodoStatus.DONE) Icons.Default.CheckCircle else Icons.Default.Pending,
                        contentDescription = null,
                        tint = if (todo.status == TodoStatus.DONE) MaterialTheme.colorScheme.primary else MaterialTheme.colorScheme.error
                    )
                    Spacer(modifier = Modifier.width(8.dp))
                    Text(
                        text = if (todo.status == TodoStatus.DONE) "Completed!" else "Uncompleted...",
                        style = MaterialTheme.typography.bodyMedium,
                        fontWeight = FontWeight.SemiBold
                    )
                }

                val remainingDays = ChronoUnit.DAYS.between(LocalDate.now(), todo.dueUtc)
                val dueText = when {
                    remainingDays < 0 -> "Overdue by ${-remainingDays} day(s)"
                    remainingDays == 0L -> "Due today!"
                    else -> "Due in $remainingDays day(s)"
                }
                if (todo.status != TodoStatus.DONE) {

                    Text(
                        text = dueText,
                        style = MaterialTheme.typography.bodyMedium,
                        color = when {
                            remainingDays < 0 -> MaterialTheme.colorScheme.error
                            remainingDays == 0L -> MaterialTheme.colorScheme.secondary
                            else -> MaterialTheme.colorScheme.primary
                        }
                    )

                }

                Button(
                    onClick = { onDismiss() },
                    modifier = Modifier.fillMaxWidth(),
                    shape = RoundedCornerShape(12.dp)
                ) {
                    Text(
                        text = "Close",
                        style = MaterialTheme.typography.labelLarge
                    )
                }
            }
        }
    }
}
