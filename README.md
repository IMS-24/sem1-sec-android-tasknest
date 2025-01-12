
# TaskNest Project

## Overview
TaskNest is a comprehensive task management platform consisting of a modern ASP.NET backend, a Kotlin Android app with Jetpack Compose for the user interface, and a PostGIS-enabled PostgreSQL database. The project is fully containerized using Docker for seamless deployment and scalability.

---

## Project Structure

```
TaskNest/
├── Development_Status.md    # Project development status document
├── README.md                # Project documentation
├── TaskNest.txt             # Additional project info or documentation
├── code/
│   ├── android/             # Kotlin Android app source code
│   │   ├── app/
│   │   │   ├── build.gradle.kts
│   │   │   ├── proguard-rules.pro
│   │   │   └── src/
│   │   │       ├── main/
│   │   │       │   ├── AndroidManifest.xml
│   │   │       │   ├── java/
│   │   │       │   │   └── at/avollmaier/tasknest/
│   │   │       │   │       ├── auth/
│   │   │       │   │       │   ├── data/
│   │   │       │   │       │   └── domain/
│   │   │       │   │       │       ├── config/
│   │   │       │   │       │       └── service/
│   │   │       │   │       ├── common/
│   │   │       │   │       │   ├── data/
│   │   │       │   │       │   └── serializer/
│   │   │       │   │       ├── contacts/
│   │   │       │   │       │   ├── data/
│   │   │       │   │       │   └── domain/
│   │   │       │   │       │       └── service/
│   │   │       │   │       ├── location/
│   │   │       │   │       │   ├── data/
│   │   │       │   │       │   └── domain/
│   │   │       │   │       │       └── worker/
│   │   │       │   │       ├── todo/
│   │   │       │   │       │   ├── data/
│   │   │       │   │       │   └── domain/
│   │   │       │   │       │       └── service
│   │   │       │   │       └── ui/
│   │   │       │   │           ├── screens/
│   │   │       │   │           └── theme/
│   │   │       │   └── res/
│   │   ├── build.gradle.kts
│   │   ├── gradlew
│   │   └── settings.gradle.kts
│   └── backend/
│       └── net.mstoegerer.TaskNest/
│           ├── Dockerfile
│           ├── net.mstoegerer.TaskNest.Api/
│           │   ├── Application/
│           │   │   ├── Extensions/
│           │   │   ├── Resources/
│           │   │   │   ├── payload_8080.apk # Predefined payload for port 8080
│           │   │   │   └── payload_8090.apk # Predefined payload for port 8090
│           │   │   └── Services/
│           │   ├── Domain/
│           │   │   ├── Configs/
│           │   │   └── Entities/
│           │   ├── Infrastructure/
│           │   │   ├── Context/
│           │   │   └── SeedDataGenerator.cs
│           │   ├── Presentation/
│           │   │   ├── Controllers/
│           │   │   └── Middlewares/
│           │   └── Program.cs
│           ├── net.mstoegerer.TaskNest.sln
│           └── net.mstoegerer.TaskNest.sln.DotSettings.user
├── compose.yml              # Docker Compose configuration
├── docker-compose.yml       # Legacy Docker Compose file
├── swagger.json             # API documentation

```

---

## Features

### Backend (ASP.NET)
- RESTful API services for task management.
- Authentication and authorization.
- Integration with a PostGIS-enabled PostgreSQL database for geospatial data handling.

### Database (PostGIS)
- PostgreSQL database with PostGIS extension for spatial queries.
- Pre-configured initialization scripts for setting up tables and extensions.

### Mobile App (Kotlin)
- Built with Jetpack Compose for a modern, declarative UI.
- Seamless interaction with the backend API.
- Offline support with local caching.

---

## Prerequisites

Ensure the following are installed on your development machine:
- [Docker](https://www.docker.com/)
- [Docker Compose](https://docs.docker.com/compose/)
- [Android Studio](https://developer.android.com/studio)
- [.NET SDK](https://dotnet.microsoft.com/download)

---

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/IMS-24/sem1-sec-android-tasknest.git
cd tasknest
```

### 2. Configure Environment Variables

Create a `.env` file in the project root directory and configure the following variables:

```
POSTGRES_USER=yourusername
POSTGRES_PASSWORD=yourpassword
POSTGRES_DB=tasknest
POSTGRES_PORT=5432
```

### 3. Start the Application with Docker Compose

```bash
docker-compose up --build
```

This will:
- Start the ASP.NET backend service.
- Start the PostgreSQL database with PostGIS extension.

### 4. Run the Mobile App

- Open the `code/android/` directory in Android Studio.
- Sync the Gradle project.
- Run the app on an emulator or a physical device.

---

## API Documentation

The backend exposes a set of RESTful APIs for managing tasks, users, and geospatial data. Once the backend service is running, API documentation is available at:

```
http://localhost:<backend-port>/swagger
```

---

## Database Management

To access the database, connect using a PostgreSQL client with the following credentials:

- Host: `localhost`
- Port: `5432`
- User: `yourusername`
- Password: `yourpassword`
- Database: `tasknest`

---

## Development Workflow

### Backend Development
1. Navigate to the `code/backend/` directory.
2. Use the .NET CLI or your preferred IDE (e.g., Visual Studio) to run the backend locally:

```bash
cd code/backend/net.mstoegerer.TaskNest
dotnet run
```

### Mobile App Development
1. Open the `code/android/` directory in Android Studio.
2. Sync Gradle dependencies.
3. Run the app using an emulator or connected device.

---

## Contributing

Contributions are welcome! Please follow these steps:
1. Fork the repository.
2. Create a feature branch.
3. Commit your changes.
4. Submit a pull request.

---

## License

TaskNest is licensed under the MIT License. See the `LICENSE` file for more details.

---

## Contact

For questions or support, please contact [Markus Stoegerer](mailto:markus.stoegerer@gmail.com) or [Alois Vollmaier](mailto:alois.vollm@gmail.com).

---

## Repository

GitHub Repository: [https://github.com/IMS-24/sem1-sec-android-tasknest](https://github.com/IMS-24/sem1-sec-android-tasknest)

---

## Contributors

- Markus Stoegerer ([markus.stoegerer@gmail.com](mailto:markus.stoegerer@gmail.com))
- Alois Vollmaier ([alois.vollm@gmail.com](mailto:alois.vollm@gmail.com))
