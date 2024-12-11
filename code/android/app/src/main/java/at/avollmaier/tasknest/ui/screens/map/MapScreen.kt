package at.avollmaier.tasknest.ui.screens.map

import android.annotation.SuppressLint
import android.os.Bundle
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.runtime.DisposableEffect
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.getValue
import androidx.compose.runtime.mutableStateOf
import androidx.compose.runtime.remember
import androidx.compose.runtime.setValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.viewinterop.AndroidView
import androidx.navigation.NavController
import at.avollmaier.tasknest.todo.data.FetchTodoDto
import at.avollmaier.tasknest.todo.domain.service.TodoService
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import com.google.android.gms.maps.CameraUpdateFactory
import com.google.android.gms.maps.MapView
import com.google.android.gms.maps.model.LatLng
import com.google.android.gms.maps.model.MarkerOptions

@Composable
fun MapScreen(navController: NavController) {
    val context = LocalContext.current
    var todos by remember { mutableStateOf<List<FetchTodoDto>>(emptyList()) }
    val locationPoints = mutableListOf<Pair<LatLng, FetchTodoDto>>()
    todos.map { todo ->
        val point = LatLng(todo.location.x, todo.location.y)
        locationPoints.plus(Pair(point, todo))
    }

    LaunchedEffect(Unit) {
        TodoService(context).getTodos { fetchedTodos ->
            if (fetchedTodos != null) {
                todos = fetchedTodos
            }
        }
    }

    TaskNestTheme {
        Surface(
            modifier = Modifier.fillMaxSize(),
            color = MaterialTheme.colorScheme.background
        ) {
            GoogleMapView(locationPoints) { selectedTodo ->
                navController.navigate("todoDetail/${selectedTodo.id}")
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

    AndroidView({ mapView }) { mapView ->
        mapView.getMapAsync { googleMap ->
            googleMap.uiSettings.isZoomControlsEnabled = true
            googleMap.moveCamera(
                CameraUpdateFactory.newLatLngZoom(
                    locationPoints.firstOrNull()?.first ?: LatLng(0.0, 0.0), 10f
                )
            )
            locationPoints.forEach { (point, todo) ->
                googleMap.addMarker(MarkerOptions().position(point))?.tag = todo
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