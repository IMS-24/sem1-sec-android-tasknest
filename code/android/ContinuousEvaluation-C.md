## **Documentation: (C) Webservices**

### **Chapter: Retrofit**

Retrofit is a powerful library for handling HTTP requests. It simplifies the process of sending
requests to a web service and parsing the responses into Java/Kotlin objects.

---

#### **Usage in the Project**

- **Objective**: Fetching and updating TODO data from the backend.
- **Key Components**:
    1. **Interface**: Defines API
       endpoints. [ITodoService](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/todo/domain/service/ITodoService.kt)
    2. **Service Layer**: Encapsulates business
       logic. [TodoService](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/todo/domain/service/TodoService.kt)

---

### **Detailed Explanation of Components**

#### **1. API Interface: `ITodoService`**

The API interface defines the HTTP methods and endpoints the application interacts with. It uses
Retrofit annotations to describe each API call, which is then implemented automatically by Retrofit.

#### **Code Example: API Interface**

```kotlin
interface ITodoService {
    @GET("api/todo")
    fun getTodos(
        @Query("pageIndex") pageIndex: Int,
        @Query("pageSize") pageSize: Int
    ): Call<TodoPages>

    @POST("api/todo")
    fun createTodo(@Body createTodoDto: CreateTodoDto): Call<Void>

    @PUT("api/todo/{id}/done")
    fun finishTodo(@Path("id") id: UUID): Call<Void>

    @POST("api/todo/share")
    fun shareTodoWithUser(@Body shareTodoDto: ShareTodoDto): Call<Void>
}
```

#### **Annotation Details**:

- **`@GET`**: Sends a GET request to fetch data.
- **`@POST`**: Sends a POST request to create a new resource.
- **`@PUT`**: Sends a PUT request to update an existing resource.
- **`@Query`**: Appends query parameters to the URL, e.g., `pageIndex=0&pageSize=20`.
- **`@Path`**: Substitutes a value directly into the URL, e.g., `/api/todo/{id}/done` becomes
  `/api/todo/1234/done`.

#### **Key Methods**:

1. **`getTodos`**:
    - Retrieves a paginated list of TODO items.
    - Parameters `pageIndex` and `pageSize` enable server-side pagination.
2. **`createTodo`**:
    - Adds a new TODO item by sending a `CreateTodoDto` object in the request body.
3. **`finishTodo`**:
    - Marks a TODO item as completed by sending its unique `id`.
4. **`shareTodoWithUser`**:
    - Shares a TODO with another user by sending a `ShareTodoDto`.

---

#### **2. Service Layer: `TodoService`**

The `TodoService` encapsulates the business logic for working with TODOs. It acts as an abstraction
layer over the `ITodoService` interface, providing simpler methods for the app to interact with.

#### **Code Example: Service Layer**

```kotlin
class TodoService(context: Context) {
    private val api: ITodoService =
        NetworkUtils.provideRetrofit(context).create(ITodoService::class.java)

    fun getTodos(pageIndex: Int, pageSize: Int, callback: (TodoPages?) -> Unit) {
        makeApiCall(
            apiCall = { api.getTodos(pageIndex, pageSize).execute() },
            onSuccess = { callback(it) },
            onError = { callback(null) }
        )
    }

    fun createTodo(createTodoDto: CreateTodoDto, callback: (Boolean) -> Unit) {
        makeApiCall(
            apiCall = { api.createTodo(createTodoDto).execute() },
            onSuccess = { callback(true) },
            onError = { callback(false) }
        )
    }
    ...
}
```

#### **How It Works**:

1. **Retrofit Instance**:
    - The `api` object is created using `.create(ITodoService::class.java)`
    - This provides an implementation of the `ITodoService` interface.
2. **Asynchronous API Calls**:
    - Each method makes an API call using `makeApiCall`, ensuring all network operations run on a
      background thread.
3. **Callbacks**:
    - Results are passed back via `onSuccess` and `onError` callbacks to notify the caller of the
      operation's outcome.

---

#### **3. Unified API Call Handling: `makeApiCall`**

The `makeApiCall` function is a reusable utility for making API requests. It handles both success
and error cases uniformly.

#### **Code Example: `makeApiCall`**

```kotlin
private fun <T> makeApiCall(
    apiCall: suspend () -> Response<T>,
    onSuccess: (T?) -> Unit,
    onError: () -> Unit
) {
    ioScope.launch {
        try {
            val response = apiCall()
            if (response.isSuccessful) {
                onSuccess(response.body())
            } else {
                Log.e("TodoService", "API call failed: ${response.raw()}")
                onError()
            }
        } catch (e: Exception) {
            Log.e("TodoService", "Exception during API call", e)
            onError()
        }
    }
}
```

#### **Explanation**:

- **API Request Execution**:
    - `apiCall`: A suspendable function that executes the API request.
- **Success Handling**:
    - If the response is successful (`response.isSuccessful`), the parsed response body is passed to
      the `onSuccess` callback.
- **Error Handling**:
    - For failed requests, an error message is logged, and the `onError` callback is invoked.
- **Thread Management**:
    - The API call runs on the `Dispatchers.IO` thread, ensuring that it doesn’t block the main
      thread.

---

### **Data Flow**

1. **API Request**:
    - The `TodoService` class makes API requests by calling methods on the `ITodoService` interface.
2. **Network Execution**:
    - The `makeApiCall` function executes the request and handles success or error scenarios.
3. **Result Processing**:
    - On success, the result is parsed into Kotlin data classes (e.g., `TodoPages`) and passed to
      the calling function.
4. **Usage in UI or Background Tasks**:
    - The processed data is used to update the UI (e.g., via Compose).

This layered architecture ensures a clean separation of concerns, making the code easier to maintain
and test.

---

### **Chapter: Room**

Room is a Jetpack library that simplifies database interactions by providing an Object-Relational
Mapping (ORM) layer over SQLite.

---

#### **Usage in the Project**

- **Objective**: Persist location data locally for offline access and efficient background
  processing.
- **Data Model**: `LocationEntity` defines the structure of the database table for storing location
  information.

---
[LocationEntity.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/data/LocationEntity.kt)

### **Code Example: `LocationEntity`**

```kotlin
@Entity(tableName = "locations")
data class LocationEntity(
    @PrimaryKey(autoGenerate = true) val id: Int = 0,
    val latitude: Double,
    val longitude: Double,
    val timestamp: Long,
    var persisted: Boolean = false
) {
    override fun toString(): String {
        return "id=$id,\ntimestamp=$timestamp\nlatitude=$latitude,\nlongitude=$longitude"
    }
}
```

#### **Explanation**:

1. **`@Entity` Annotation**:
    - Marks the class as a database table.
    - The `tableName` parameter specifies the table's name in the database (`locations`).

2. **Primary Key**:
    - `@PrimaryKey(autoGenerate = true)`: The `id` field serves as a unique identifier,
      automatically generated for each record.

3. **Fields**:
    - `latitude` and `longitude`: Store the geographic coordinates.
    - `timestamp`: Captures the time when the location data was recorded.
    - `persisted`: Indicates whether the location has been processed or synchronized (e.g., with a
      remote server).

4. **`toString` Method**:
    - Provides a readable representation of the object for logging and debugging purposes.

---

#### **DAO (Data Access Object)**

The DAO defines the operations that can be performed on the `locations` table. It includes methods
for inserting, updating, and querying the location data.

[LocationDao.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/LocationDao.kt)

#### **Code Example: Location DAO**

```kotlin
@Dao
interface LocationDao {
    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun insertLocation(location: LocationEntity)

    @Query("SELECT * FROM locations WHERE persisted = 0 ORDER BY timestamp DESC")
    suspend fun getOfflinePersistedLocations(): List<LocationEntity>

    @Query("SELECT * FROM locations ORDER BY timestamp DESC LIMIT 1")
    suspend fun getLastLocation(): LocationEntity?

    @Insert(onConflict = OnConflictStrategy.REPLACE)
    suspend fun updateLocations(location: List<LocationEntity>)

    @Transaction
    suspend fun getAndMarkPersisted(): List<LocationEntity> {
        val locations = getOfflinePersistedLocations()
        locations.forEach { it.persisted = true }
        updateLocations(locations)
        return locations
    }
}
```

#### **Explanation**:

1. **`@Insert`**:
    - Inserts a new location into the database.
    - The `OnConflictStrategy.REPLACE` ensures that if a record with the same primary key exists, it
      will be replaced.

2. **`@Query`**:
    - **`getOfflinePersistedLocations`**:
        - Retrieves locations that have not yet been marked as processed (`persisted = 0`).
        - Results are sorted by `timestamp` in descending order.
    - **`getLastLocation`**:
        - Fetches the most recent location.
        - Limits the query to the most recent record using `LIMIT 1`.

3. **`@Transaction`**:
    - **`getAndMarkPersisted`**:
        - Combines multiple operations into a single transaction.
        - Retrieves unprocessed locations, marks them as persisted, updates the database, and
          returns the updated list.

---

#### **Integration: Database Setup**

[LocationDatabase.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/LocationDatabase.kt)
The `LocationDatabase` class integrates the DAO and the database configuration.

#### **Code Example: LocationDatabase**

```kotlin
@Database(entities = [LocationEntity::class], version = 1)
abstract class LocationDatabase : RoomDatabase() {
    abstract fun locationDao(): LocationDao

    companion object {
        @Volatile
        private var INSTANCE: LocationDatabase? = null

        fun getDatabase(context: Context): LocationDatabase {
            return INSTANCE ?: synchronized(this) {
                val instance = Room.databaseBuilder(
                    context.applicationContext,
                    LocationDatabase::class.java,
                    "location_database"
                ).build()
                INSTANCE = instance
                instance
            }
        }
    }
}

```

#### **Explanation**:

1. **`@Database` Annotation**:
    - Specifies the entities (tables) in the database (`LocationEntity`).
    - `version` defines the schema version of the database.

2. **Abstract Method**:
    - `locationDao`: Provides an instance of `LocationDao` for interacting with the database.

---

### **Chapter: WorkManager**

WorkManager is used for reliably running background tasks.

---

#### **Usage in the Project**

- **Objective**: Periodically fetch location data and compare it with TODOs.
- **Key Class**:
    - `LocationCheckWorker`: A worker class for location
      checking. [LocationCheckWorker](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/worker/LocationCheckWorker.kt)

#### **Code Example: Periodic Worker**

```kotlin
class LocationCheckWorker(
    context: Context,
    workerParams: WorkerParameters
) : CoroutineWorker(context, workerParams) {

    override suspend fun doWork(): Result {
        val location = LocationDatabaseService(applicationContext).getCurrentLocation()
        val todoService = TodoService(applicationContext)

        todoService.getTodos(0, Int.MAX_VALUE) { todos ->
            todos?.items?.forEach { todo ->
                val todoLocation = todo.location
                if (isWithinRange(
                        location.latitude,
                        location.longitude,
                        todoLocation.latitude,
                        todoLocation.longitude
                    )
                ) {
                    todoService.finishTodo(todo.id) { success ->
                        if (success) Log.d("Worker", "TODO ${todo.id} completed.")
                    }
                }
            }
        }
        return Result.success()
    }
}
```

- Location data is fetched and TODOs are checked for proximity.
- If a TODO is within range, it is marked as completed.

---

#### **Task Scheduling**

```kotlin
val workRequest = PeriodicWorkRequestBuilder<LocationCheckWorker>(
    15, TimeUnit.MINUTES
).build()

WorkManager.getInstance(context).enqueueUniquePeriodicWork(
    "LocationCheckWorker",
    ExistingPeriodicWorkPolicy.REPLACE,
    workRequest
)
```

- The worker runs every 15 minutes.
- `enqueueUniquePeriodicWork` ensures no duplicate tasks are scheduled.

---

### **Chapter: Jetpack Compose/LiveData**

Jetpack Compose is paired with `StateFlow` in this project to provide a seamless, reactive UI. The
ViewModel handles business logic and exposes `StateFlow` objects, which the Compose view observes
and uses to render dynamic content.

---

#### **Usage in the Project**

- **Objective**: Dynamically display a TODO list while ensuring the UI stays in sync with changes to
  the data.
- **Mechanism**:
    - The ViewModel uses `StateFlow` to manage and expose state.
    - The Compose view observes the `StateFlow` using `collectAsState` and re-renders automatically
      when the state changes.

---

### **Key Code Snippets**

#### **1. StateFlow in ViewModel**

[OverviewViewModel](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/overview/OverviewViewModel.kt)

The `OverviewViewModel` uses `MutableStateFlow` to store the list of TODOs and exposes it as a
`StateFlow`. This encapsulation ensures that only the ViewModel can modify the data.

```kotlin
class OverviewViewModel(private val todoService: TodoService, context: Context) : ViewModel() {
    private val _todos = MutableStateFlow<List<FetchTodoDto>>(emptyList())
    val todos: StateFlow<List<FetchTodoDto>> = _todos

    private val _hasNextPage = MutableStateFlow(true)
    val hasNextPage: StateFlow<Boolean> = _hasNextPage

    init {
        fetchTodos()
    }

    private fun fetchTodos() {
        viewModelScope.launch {
            todoService.getTodos(pageIndex = 0, pageSize = 20) { todoPages ->
                todoPages?.let {
                    val newTodos = it.items.filter { todo -> todo.status == TodoStatus.NEW }
                    _todos.value = _todos.value + newTodos
                    _hasNextPage.value = it.hasNextPage
                }
            }
        }
    }

    fun refreshTodos() {
        viewModelScope.launch {
            _todos.value = emptyList()
            fetchTodos()
        }
    }

    ...
}
```

#### **Explanation**:

1. **`MutableStateFlow` and `StateFlow`**:
    - `_todos` is a private `MutableStateFlow` that holds the current list of TODOs. It can only be
      modified within the ViewModel.
    - `todos` is the public `StateFlow` that exposes the data in a read-only manner to the Compose
      view.

2. **`fetchTodos`**:
    - Fetches TODOs from the backend using the `TodoService`.
    - Filters for `NEW` TODOs and appends them to the existing state.

3. **Reactive Updates**:
    - Any modification to `_todos` automatically triggers UI updates in the Compose view that
      observes the `todos` StateFlow.

---

#### **2. Observing StateFlow in Compose View**

The Compose view observes the `StateFlow` using the `collectAsState` function, ensuring the UI
updates dynamically when the state changes.

[OverviewScreen](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/overview/OverviewScreen.kt)

```kotlin
@Composable
fun OverviewScreen(
    viewModel: OverviewViewModel = viewModel(factory = OverviewViewModelFactory(LocalContext.current))
) {
    val todos by viewModel.todos.collectAsState(initial = emptyList())
    val hasNextPage by viewModel.hasNextPage.collectAsState(initial = true)

    ...
    LazyColumn(
        modifier = Modifier.fillMaxSize()
    ) {
        if (todos.isEmpty()) {
            item {
                EmptyTodoState()
            }
        } else {
            items(todos) { todo ->
                TodoCard(todo, viewModel)
            }
            item {
                if (hasNextPage) {
                    Box(
                        modifier = Modifier.fillMaxWidth(),
                        contentAlignment = Alignment.Center
                    ) {
                        Button(onClick = { viewModel.loadMoreTodos() }) {
                            Text("Load More")
                        }
                    }
                }
            }
        }
    }
    ...
}
```

### **Summary: StateFlow and Interaction Flow**

1. **`collectAsState`**:
    - Converts the `StateFlow` from the ViewModel into a Compose-compatible `State`, providing an
      initial value (`emptyList()`) until the first update.

2. **Dynamic UI Updates**:
    - Changes in the `StateFlow` automatically trigger recompositions in Compose, ensuring the UI
      stays consistent with the latest data without manual updates.

3. **Interaction Flow**:
    - **State Initialization**: The ViewModel initializes `_todos` and fetches the initial data
      using `fetchTodos`.
    - **State Update**: As new TODOs are fetched, `_todos.value` is updated, and the `StateFlow`
      emits the new state.
    - **UI Update**: The Compose view observes these changes via `collectAsState`, dynamically
      updating the displayed TODO list in the `LazyColumn`.

---

### **Summary**

| Technology      | Purpose                    | Key Components        | Code Links                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                   |
|-----------------|----------------------------|-----------------------|------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|
| **Retrofit**    | API integration            | Interface, Service    | [ITodoService](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/todo/domain/service/ITodoService.kt) [TodoService](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/todo/domain/service/TodoService.kt)                                                                                                                                                                                |
| **Room**        | Data persistence in SQLite | Entity, DAO, Database | [LocationEntity.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/data/LocationEntity.kt) [LocationDao.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/LocationDao.kt) [LocationDatabase.kt](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/LocationDatabase.kt) 
|
| **WorkManager** | Background tasks           | Worker                | [LocationCheckWorker](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/location/domain/worker/LocationCheckWorker.kt)                                                                                                                                                                                                                                                                                                                                     |
| **Compose**     | UI creation                | ViewModel, View       | [OverviewViewModel](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/overview/OverviewViewModel.kt) [OverviewScreen](https://github.com/IMS-24/sem1-sec-android-tasknest/blob/main/code/android/app/src/main/java/at/avollmaier/tasknest/ui/screens/overview/OverviewScreen.kt)                                                                                                                                                                |

Please refer to the **Code Links** column for file paths or GitHub links.