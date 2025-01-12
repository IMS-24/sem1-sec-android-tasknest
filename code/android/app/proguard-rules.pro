# Keep Jackson annotations
-keepattributes *Annotation*

# Keep the constructors of the data classes
-keepclassmembers class at.avollmaier.tasknest.todo.data.** {
    public <init>(...);
}

# Keep the data classes themselves
-keep class at.avollmaier.tasknest.todo.data.** { *; }