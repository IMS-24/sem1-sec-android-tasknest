package at.avollmaier.tasknest.ui.screens.nav

import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.filled.AccountCircle
import androidx.compose.material.icons.filled.DateRange
import androidx.compose.material.icons.filled.Face
import androidx.compose.material.icons.filled.LocationOn
import androidx.compose.ui.graphics.vector.ImageVector
import at.avollmaier.tasknest.ui.screens.Screens

data class BottomNavigationItem(
    val label: String = "",
    val icon: ImageVector = Icons.Filled.DateRange,
    val route: String = ""
) {
    fun bottomNavigationItems(): List<BottomNavigationItem> {
        return listOf(
            BottomNavigationItem(
                label = "Today",
                icon = Icons.Filled.DateRange,
                route = Screens.Today.route
            ),
            BottomNavigationItem(
                label = "Map",
                icon = Icons.Filled.LocationOn,
                route = Screens.Map.route
            ),
            BottomNavigationItem(
                label = "Team",
                icon = Icons.Filled.Face,
                route = Screens.Team.route
            ),
            BottomNavigationItem(
                label = "Profile",
                icon = Icons.Filled.AccountCircle,
                route = Screens.Profile.route
            ),
        )
    }
}