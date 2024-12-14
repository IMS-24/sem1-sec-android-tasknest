package at.avollmaier.tasknest.ui.screens

sealed class Screens(val route: String) {
    object Overview : Screens("overview_screen")
    object Map : Screens("map_screen")
    object Team : Screens("team_screen")
    object Profile : Screens("profile_screen")
}