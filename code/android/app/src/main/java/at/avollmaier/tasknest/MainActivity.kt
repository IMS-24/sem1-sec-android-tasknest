package at.avollmaier.tasknest

import RequiredPermission
import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.activity.viewModels
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.SnackbarHostState
import androidx.compose.material3.Surface
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.runtime.rememberCoroutineScope
import androidx.compose.ui.Modifier
import at.avollmaier.tasknest.auth.domain.service.AuthProviderService
import at.avollmaier.tasknest.ui.screens.nav.BottomNavigationBar
import at.avollmaier.tasknest.ui.screens.onboarding.OnboardScreenViewModel
import at.avollmaier.tasknest.ui.theme.TaskNestTheme
import com.compose.practical.ui.onboardingScreen.OnboardScreen
import kotlinx.coroutines.launch

class MainActivity : ComponentActivity() {
    private val authProvider = AuthProviderService()
    private val onboardViewModel: OnboardScreenViewModel by viewModels()
    private val mainViewModel: MainViewModel by viewModels()

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        authProvider.setUp(this)
        setContent {
            TaskNestTheme {
                Surface(
                    modifier = Modifier.fillMaxSize(),
                    color = MaterialTheme.colorScheme.background
                ) {
                    MainView()
                }
            }
        }
    }

    @Composable
    fun MainView() {
        val onboardingCompleted by onboardViewModel.onboardingCompleted.collectAsState()
        val userIsAuthenticated by mainViewModel.userIsAuthenticated.collectAsState()

        if (!onboardingCompleted) {
            OnboardScreen(viewModel = onboardViewModel)
        } else if (userIsAuthenticated) {
            RequiredPermission(content = {
                BottomNavigationBar(authProvider, mainViewModel.getUser())
            })
        } else {
            mainViewModel.loginUser(
                context = this,
                authProvider = authProvider
            )
        }
    }
}