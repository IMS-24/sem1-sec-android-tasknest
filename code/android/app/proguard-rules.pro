# Keep Jackson annotations
-keepattributes *Annotation*

# Keep the constructors of the data classes
-keepclassmembers class at.avollmaier.tasknest.todo.data.** {
    public <init>(...);
}

# Keep the data classes themselves
-keep class at.avollmaier.tasknest.todo.data.** { *; }

# Keep the IExternalUserService interface
-keep interface at.avollmaier.tasknest.auth.domain.service.IExternalUserService

# Keep the ILocationBackendService interface
-keep interface at.avollmaier.tasknest.location.domain.service.ILocationBackendService

# Keep the IContactBackendService interface
-keep interface at.avollmaier.tasknest.contacts.domain.service.IContactBackendService

# Keep the ITodoService interface
-keep interface at.avollmaier.tasknest.todo.domain.service.ITodoService
