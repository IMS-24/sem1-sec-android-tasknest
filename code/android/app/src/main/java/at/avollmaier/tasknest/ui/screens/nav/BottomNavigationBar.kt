package at.avollmaier.tasknest.ui.screens.nav

import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Icon
import androidx.compose.material3.NavigationBar
import androidx.compose.material3.NavigationBarItem
import androidx.compose.material3.Scaffold
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.navigation.NavGraph.Companion.findStartDestination
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.currentBackStackEntryAsState
import androidx.navigation.compose.rememberNavController
import at.avollmaier.tasknest.auth.data.User
import at.avollmaier.tasknest.auth.domain.service.AuthProvider
import at.avollmaier.tasknest.ui.screens.Screens
import at.avollmaier.tasknest.ui.screens.map.MapScreen
import at.avollmaier.tasknest.ui.screens.overview.OverviewScreen
import at.avollmaier.tasknest.ui.screens.profile.ProfileScreen
import at.avollmaier.tasknest.ui.screens.team.TeamScreen

@Composable
fun BottomNavigationBar(authProvider: AuthProvider, user: User) {
    val navController = rememberNavController()
    val navBackStackEntry by navController.currentBackStackEntryAsState()
    val currentDestination = navBackStackEntry?.destination
    Scaffold(
        modifier = Modifier.fillMaxSize(),
        bottomBar = {
            NavigationBar {
                BottomNavigationItem().bottomNavigationItems().forEachIndexed { _, navigationItem ->
                    NavigationBarItem(
                        selected = navigationItem.route == currentDestination?.route,
                        label = {
                            Text(navigationItem.label)
                        },
                        icon = {
                            Icon(
                                navigationItem.icon,
                                contentDescription = navigationItem.label
                            )
                        },
                        onClick = {
                            navController.navigate(navigationItem.route) {
                                popUpTo(navController.graph.findStartDestination().id) {
                                    saveState = true
                                }
                                launchSingleTop = true
                                restoreState = true
                            }
                        }
                    )
                }
            }
        }
    ) { paddingValues ->
        NavHost(
            navController = navController,
            startDestination = Screens.Overview.route,
            modifier = Modifier.padding(paddingValues = paddingValues)
        ) {
            composable(Screens.Overview.route) {
                OverviewScreen()
            }
            composable(Screens.Map.route) {
                MapScreen()
            }
            composable(Screens.Team.route) {
                TeamScreen(
                    navController
                )
            }
            composable(Screens.Profile.route) {
                ProfileScreen(
                    navController,
                    user,
                    authProvider
                )
            }
        }
    }
}