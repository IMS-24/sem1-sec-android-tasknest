plugins {
    alias(libs.plugins.android.application)
    alias(libs.plugins.kotlin.android)
    kotlin("kapt")
    id("com.google.android.libraries.mapsplatform.secrets-gradle-plugin")
}

android {
    namespace = "net.mstoegerer.tasknest"
    compileSdk = 34

    defaultConfig {
        applicationId = "net.mstoegerer.tasknest"
        minSdk = 24
        targetSdk = 35
        versionCode = 1
        versionName = "1.0"

        manifestPlaceholders.clear()
        manifestPlaceholders["auth0Domain"] = "@string/com_auth0_domain"
        manifestPlaceholders["auth0Scheme"] = "@string/com_auth0_scheme"
        testInstrumentationRunner = "androidx.test.runner.AndroidJUnitRunner"
    }

    buildTypes {
        release {
            isMinifyEnabled = false
            proguardFiles(
                getDefaultProguardFile("proguard-android-optimize.txt"),
                "proguard-rules.pro"
            )
        }
    }
    compileOptions {
        sourceCompatibility = JavaVersion.VERSION_17
        targetCompatibility = JavaVersion.VERSION_17
    }
    kotlinOptions {
        jvmTarget = "17"
    }
    buildFeatures {
        viewBinding = true
    }
    buildToolsVersion = "35.0.0"
}

android {
    buildTypes {
        getByName("release") {
            // Enables code shrinking, obfuscation, and optimization for only
            // your project's release build type. Make sure to use a build
            // variant with `isDebuggable=false`.
            isMinifyEnabled = true

            // Enables resource shrinking, which is performed by the
            // Android Gradle plugin.
            isShrinkResources = true

            proguardFiles(
                // Includes the default ProGuard rules files that are packaged with
                // the Android Gradle plugin. To learn more, go to the section about
                // R8 configuration files.
                getDefaultProguardFile("proguard-android-optimize.txt"),

                // Includes a local, custom Proguard rules file
                "proguard-rules.pro"
            )
        }
    }

}
dependencies {
    implementation(libs.androidx.core.ktx)
    implementation(libs.androidx.appcompat)
    implementation(libs.material)
    implementation(libs.androidx.constraintlayout)
    implementation(libs.androidx.lifecycle.livedata.ktx)
    implementation(libs.androidx.lifecycle.viewmodel.ktx)
    implementation(libs.androidx.navigation.fragment.ktx)
    implementation(libs.androidx.navigation.ui.ktx)
    implementation(libs.androidx.legacy.support.v4)
    implementation(libs.androidx.fragment.ktx)
    implementation(libs.play.services.maps)
    implementation(libs.retrofit)
    implementation(libs.converter.gson)
    implementation(libs.androidx.work.runtime)
    implementation(libs.androidx.work.runtime.ktx)
    implementation(libs.androidx.work.rxjava2)
    implementation(libs.androidx.work.gcm)
    implementation(libs.androidx.annotation)
    implementation(libs.androidx.activity)
    androidTestImplementation(libs.androidx.work.testing)
    implementation(libs.androidx.work.multiprocess)
    implementation(libs.androidx.room.runtime)
    annotationProcessor(libs.androidx.room.compiler)
    implementation(libs.auth0)


    kapt("androidx.room:room-compiler:2.6.1")

    implementation(libs.androidx.room.ktx)
    implementation(libs.play.services.location)
    testImplementation(libs.junit)
    androidTestImplementation(libs.androidx.junit)
    androidTestImplementation(libs.androidx.espresso.core)
}
secrets {
    propertiesFileName = "secrets.properties"
    defaultPropertiesFileName = "local.defaults.properties"
}
