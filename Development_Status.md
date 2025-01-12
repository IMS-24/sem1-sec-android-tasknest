# Development Status for TaskNest

## Overview

This document provides an overview of the current development status for the TaskNest project, highlighting completed
tasks, ongoing plans, and experimental features (hacks). TaskNest is a dynamic task management platform with features
powered by modern backend and mobile technologies.

---

## Tasks

### Status

#### Basic Functionality

- [x] Create dotnetBackend that provides CRUD functionality
- [x] Create Android app (Kotlin, Jetpack Compose)
- [x] Implement Auth0
- [x] Implement basic todo functionality (create, check, share, attachments)
- [x] Implement UI
- [x] HTTPS encryption

#### Hack Functionality

- [x] Fetch metadata from device/user **(HACK)**
- [x] Use device as GPS tracker **(HACK)**
- [x] Create endpoint for fake updates
- [x] Implement shellcode and badcode handling
- [-] Inject bad code via software update **(HACK)**
- [-] Create reverse shell **(HACK)**
- [x] Encrypt device data **(HACK)**

---

## Decisions

### Jetpack Compose

Jetpack Compose offers a modern, declarative approach to building UIs, enabling faster development with fewer lines of
code. It aligns with Kotlin’s language features and enhances maintainability and scalability for the Android app.

### .NET Backend

.NET provides robust tools for building scalable, high-performance backend services. Its integration with ASP.NET and
Entity Framework simplifies CRUD operations and database interactions, making it a suitable choice for a task management
application.

### Auth0

Auth0 streamlines authentication and authorization processes, offering secure and scalable identity management. It
allows for easy integration with the backend and supports multiple authentication providers, ensuring flexibility and
security for the platform.

## App Status

### Features Included

- User authentication via Auth0.
- Task creation, updating, and deletion.
- Attach files to tasks.
- Basic UI for managing tasks and user profiles.
- HTTPS encryption for secure data transfer.
- Device metadata retrieval for enhanced task tracking.
- GPS tracking for location-based task management.

### Work-in-Progress (WIP)


### Known Bugs

- UI performance issues with large task lists.

### Next Steps

- Resolve UI performance issues.
