package net.mstoegerer.tasknest.ui.team

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import androidx.fragment.app.Fragment
import androidx.fragment.app.viewModels
import net.mstoegerer.tasknest.R

class TeamFragment : Fragment() {

    companion object {
        fun newInstance() = TeamFragment()
    }

    private val viewModel: TeamViewModel by viewModels()

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View {
        return inflater.inflate(R.layout.fragment_team, container, false)
    }
}