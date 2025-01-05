package at.avollmaier.tasknest.ui.screens.onboarding

import android.content.Context
import androidx.datastore.preferences.core.booleanPreferencesKey
import androidx.datastore.preferences.core.edit
import androidx.datastore.preferences.preferencesDataStore
import androidx.lifecycle.ViewModel
import androidx.lifecycle.viewModelScope
import kotlinx.coroutines.flow.MutableStateFlow
import kotlinx.coroutines.flow.StateFlow
import kotlinx.coroutines.flow.map
import kotlinx.coroutines.launch

private val Context.dataStore by preferencesDataStore(name = "onboarding_preferences")

class OnboardScreenViewModel : ViewModel() {

    companion object {
        private val ONBOARDING_SHOWN_KEY = booleanPreferencesKey("onboarding_shown")
    }

    private val _onboardingCompleted = MutableStateFlow(false)
    val onboardingCompleted: StateFlow<Boolean> = _onboardingCompleted

    fun checkOnboardingShown(context: Context) {
        viewModelScope.launch {
            context.dataStore.data
                .map { preferences ->
                    preferences[ONBOARDING_SHOWN_KEY] ?: false
                }
                .collect { isShown ->
                    _onboardingCompleted.value = isShown
                }
        }
    }

    fun setOnboardingShown(context: Context) {
        viewModelScope.launch {
            context.dataStore.edit { preferences ->
                preferences[ONBOARDING_SHOWN_KEY] = true
            }
            _onboardingCompleted.value = true
        }
    }
}
