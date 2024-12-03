package net.mstoegerer.tasknest

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import net.mstoegerer.tasknest.databinding.ActivityLoginBinding
import net.mstoegerer.tasknest.service.AuthService

class LoginActivity : AppCompatActivity() {
    private lateinit var binding: ActivityLoginBinding
    private lateinit var authService: AuthService

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)

        // Initialize AuthService
        authService = AuthService(this)

        // Bind the button click with the login action
        binding = ActivityLoginBinding.inflate(layoutInflater)
        setContentView(binding.root)
        binding.buttonGetStarted.setOnClickListener { authService.login(this) }
    }
}