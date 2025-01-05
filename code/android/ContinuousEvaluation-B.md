## **Documentation: (B) Secure Coding Work**

### **Chapter: DataStore: Secure and Persistent Preferences**

#### **Objective**

Utilize Jetpack DataStore for modern, secure storage of user preferences, ensuring efficient and
private data handling.

#### **Key Elements**

- **Data Initialization**: Create a `DataStore` instance bound to the app context.

```kotlin
private val Context.dataStore by preferencesDataStore(name = "onboarding_preferences")
```

- **Reading Preferences**:
    - Use `dataStore.data` to access stored preferences reactively.
    - Map the preferences to the required type, such as a boolean for onboarding status.

```kotlin
context.dataStore.data
    .map { preferences -> preferences[ONBOARDING_SHOWN_KEY] ?: false }
    .collect { isShown -> _onboardingCompleted.value = isShown }
```

- **Writing Preferences**:
    - Edit preferences securely using `dataStore.edit` and update the state.

```kotlin
context.dataStore.edit { preferences -> preferences[ONBOARDING_SHOWN_KEY] = true }
```

#### **What It Does**

- Securely stores the `onboarding_shown` flag.
- Ensures reactivity by linking state changes directly to the UI.

#### **Link**

- [OnboardingViewModel.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/onboarding/OnboardingViewModel.kt)

---

### **Chapter: Permission Handling**

#### **Objective**

Handle sensitive permissions securely, ensuring users are well-informed while maintaining app
functionality.

#### **Key Elements**

- **Permission Request**:
    - Use the Accompanist library to check and request permissions dynamically.

```kotlin
val state = rememberPermissionState(android.Manifest.permission.ACCESS_FINE_LOCATION)
```

- **Permission Status Handling**:
    - Check if the permission is granted or denied, and act accordingly.

```kotlin
if (state.status.isGranted) {
    LocationCheckWorker.schedule(LocalContext.current)
    content()
} else {
    PermissionRationale(state)
}
```

- **Permission Rationale**:
    - Provide a clear explanation if the user denies the permission.

```kotlin
Column {
    Text("Navigation permission required", style = MaterialTheme.typography.headlineMedium)
    Button(onClick = { launchSettings(context) }) {
        Text("Go to settings")
    }
}
```

#### **What It Does**

- Dynamically requests the `ACCESS_FINE_LOCATION` permission.
- Displays a rationale and guides users to the settings page if needed.
- Schedules location-based workers only when permissions are granted.

#### **Link**

- [PermissionHandlingScreen.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/PermissionHandlingScreen.kt)

---

### **Chapter: Secure Authentication with Auth0 (OAuth2) SecureCredentialsManager**

#### **Objective**

Leverage Auth0's `SecureCredentialsManager` to securely handle authentication credentials, such as
access tokens and refresh tokens. This ensures secure storage and retrieval of sensitive user data.

---

#### **Key Elements**

- **Initialization**:
    - The `SecureCredentialsManager` integrates with Android's `SharedPreferences` and the Keystore
      to store credentials securely.

```kotlin
val manager = SecureCredentialsManager(
    context,
    CredentialsManager(Auth0(clientId, domain)),
    SharedPreferencesStorage(context)
)
```

- **Storing Credentials**:
    - After a successful login, store the credentials securely.

```kotlin
val credentials = Credentials(
    accessToken = "your-access-token",
    idToken = "your-id-token",
    refreshToken = "your-refresh-token",
    expiresAt = Date(System.currentTimeMillis() + 3600 * 1000)
)

manager.saveCredentials(credentials)
```

- **Retrieving Credentials**:
    - Retrieve stored credentials for API requests or other authenticated actions.

```kotlin
manager.getCredentials(object : Callback<Credentials, CredentialsManagerException> {
    override fun onSuccess(credentials: Credentials) {
        // Use credentials.accessToken or credentials.idToken
    }

    override fun onFailure(error: CredentialsManagerException) {
        // Handle failure
    }
})
```

- **Refreshing Tokens**:
    - Automatically refresh tokens using the `SecureCredentialsManager` if they are expired or about
      to expire.

```kotlin
manager.getCredentials(object : Callback<Credentials, CredentialsManagerException> {
    override fun onSuccess(credentials: Credentials) {
        // Access token refreshed automatically if needed
    }

    override fun onFailure(error: CredentialsManagerException) {
        // Handle failure
    }
})
```

---

#### **What It Does**

1. **Secure Storage**:
    - Credentials are stored using Android’s Keystore, ensuring encryption and secure access.
2. **Automatic Token Management**:
    - Automatically refreshes access tokens using the refresh token when needed.
3. **Simplified Authentication**:
    - Encapsulates the complexity of securely storing and managing sensitive authentication data.

---

#### **Link**

- [AuthProviderService.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/auth/domain/service/AuthProviderService.kt)
