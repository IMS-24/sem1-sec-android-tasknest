package at.avollmaier.tasknest.ui.screens.onboarding
import android.content.Context
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.launch

class OnboardScreenViewModel() : ViewModel() {
    private val localState = MutableStateFlow(false)
    val onboardingCompleted: StateFlow<Boolean> = localState

    fun checkOnboardingShown(context: Context) {
        val sharedPref = context.getSharedPreferences("onboarding", Context.MODE_PRIVATE)
        localState.value = sharedPref.getBoolean("onboarding_shown", false)
    }

    fun setOnboardingShown(context: Context) {
        viewModelScope.launch {
            val sharedPref = context.getSharedPreferences("onboarding", Context.MODE_PRIVATE)
            with(sharedPref.edit()) {
                putBoolean("onboarding_shown", true)
                apply()
            }
            localState.value = true
        }
    }
}