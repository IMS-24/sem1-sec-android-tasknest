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
import androidx.compose.runtime.collectAsState
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
import androidx.lifecycle.viewmodel.compose.viewModel
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.data.TodoStatus
import at.avollmaier.tasknest.ui.screens.overview.TodoDueState
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.MapView
import com.google.android.gms.maps.model.BitmapDescriptorFactory
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions

@Composable
fun MapScreen(
    viewModel: MapViewModel = viewModel(factory = MapViewModelFactory(LocalContext.current))
) {
    var mapSelectedTodo by remember { mutableStateOf<FetchTodoDto?>(null) }
    val todos by viewModel.todos.collectAsState()
    val markersAdded by viewModel.markersAdded.collectAsState()

    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {

            GoogleMapView(viewModel, viewModel.getLatLngPair(todos), markersAdded) { selectedTodo ->
                mapSelectedTodo = selectedTodo
            }

            mapSelectedTodo?.let { todo ->
                TodoDetailDialog(todo = todo) {
                    mapSelectedTodo = null
                }
            }
        }
    }
}

@SuppressLint("PotentialBehaviorOverride")
@Composable
fun GoogleMapView(
    viewModel: MapViewModel,
    locationPoints: List<Pair<LatLng, FetchTodoDto>>,
    markersAdded: Boolean,
    onMarkerClick: (FetchTodoDto) -> Unit
) {
    val mapView = rememberMapViewWithLifecycle()
    val context = LocalContext.current


    LaunchedEffect(mapView) {
        viewModel.fetchTodos()
    }

    AndroidView({ mapView }) { mapView ->
        mapView.getMapAsync { googleMap ->
            googleMap.uiSettings.isZoomControlsEnabled = true

            if (locationPoints.isNotEmpty() && !markersAdded) {
                googleMap.clear()
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
                        .alpha(if (todo.status == TodoStatus.DONE) 0.3f else 1.0f)
                        .icon(
                            if (todo.status == TodoStatus.DONE) {
                                BitmapDescriptorFactory.defaultMarker(BitmapDescriptorFactory.HUE_GREEN)
                            } else {
                                BitmapDescriptorFactory.defaultMarker(BitmapDescriptorFactory.HUE_RED)
                            }
                        )

                    googleMap.addMarker(markerOptions)?.tag = todo

                    if (todo.status == TodoStatus.NEW) {
                        googleMap.addCircle(
                            com.google.android.gms.maps.model.CircleOptions()
                                .center(point)
                                .radius(
                                    context.resources.getString(R.string.todo_location_radius)
                                        .toDouble()
                                )
                                .strokeWidth(2f)
                                .strokeColor(android.graphics.Color.RED)
                                .fillColor(Color.Red.copy(alpha = 0.3f).toArgb())
                        )
                    }
                }

                viewModel.setMarkersAdded(true)

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

                TodoDueStateText(todo = todo)
                TodoDueState(todo = todo)

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

@Composable
fun TodoDueStateText(todo: FetchTodoDto) {
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
}
