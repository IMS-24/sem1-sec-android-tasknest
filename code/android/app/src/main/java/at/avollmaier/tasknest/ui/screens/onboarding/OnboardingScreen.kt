package com.compose.practical.ui.onboardingScreen

import androidx.compose.foundation.Image
import androidx.compose.foundation.background
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.layout.size
import androidx.compose.foundation.shape.RoundedCornerShape
import androidx.compose.material3.Button
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Surface
import androidx.compose.material3.Tab
import androidx.compose.material3.TabRow
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.mutableIntStateOf
import androidx.compose.runtime.remember
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.graphics.graphicsLayer
import androidx.compose.ui.layout.ContentScale
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.platform.testTag
import androidx.compose.ui.res.painterResource
import androidx.compose.ui.text.style.TextAlign
import androidx.compose.ui.tooling.preview.Preview
import androidx.compose.ui.unit.dp
import at.avollmaier.tasknest.R
import at.avollmaier.tasknest.ui.screens.onboarding.OnboardScreenViewModel
import com.compose.practical.ui.onboardingScreen.Tags.TAG_ONBOARD_SCREEN_IMAGE_VIEW
import com.compose.practical.ui.onboardingScreen.Tags.TAG_ONBOARD_SCREEN_NAV_BUTTON
import com.compose.practical.ui.onboardingScreen.Tags.TAG_ONBOARD_TAG_ROW

val onboardPagesList = listOf(
    OnboardPage(
        imageRes = R.drawable.todo,
        title = "Welcome to TaskNest",
        description = "Organize your tasks efficiently and boost your productivity with TaskNest."
    ),
    OnboardPage(
        imageRes = R.drawable.team,
        title = "Discover Powerful Features",
        description = "Collaborate with your team, manage projects, and stay on top of your goals."
    ),
    OnboardPage(
        imageRes = R.drawable.map,
        title = "Get Started Today",
        description = "Join TaskNest now and take the first step towards a more organized life."
    )
)

object Tags {
    const val TAG_ONBOARD_SCREEN = "onboard_screen"
    const val TAG_ONBOARD_SCREEN_IMAGE_VIEW = "onboard_screen_image"
    const val TAG_ONBOARD_SCREEN_NAV_BUTTON = "nav_button"
    const val TAG_ONBOARD_TAG_ROW = "tag_row"
}

@Composable
fun OnboardScreen(viewModel: OnboardScreenViewModel) {
    val onboardPages = onboardPagesList
    val currentPage = remember { mutableIntStateOf(0) }
    val context = LocalContext.current
    viewModel.checkOnboardingShown(context)
    Column(
        modifier = Modifier
            .fillMaxSize()
            .testTag(Tags.TAG_ONBOARD_SCREEN)
    ) {
        Spacer(modifier = Modifier.height(64.dp))
        OnBoardImageView(
            modifier = Modifier
                .weight(1f)
                .fillMaxWidth(),
            currentPage = onboardPages[currentPage.intValue]
        )

        OnBoardDetails(
            modifier = Modifier
                .weight(1f)
                .padding(16.dp),
            currentPage = onboardPages[currentPage.intValue]
        )

        OnBoardNavButton(
            modifier = Modifier
                .align(Alignment.CenterHorizontally)
                .padding(top = 16.dp),
            currentPage = currentPage.intValue,
            noOfPages = onboardPages.size,
            onNextClicked = {
                if (currentPage.intValue < onboardPages.size - 1) {
                    currentPage.intValue++
                }
            },
            onLastButtonClicked = {
                viewModel.setOnboardingShown(context)
            }
        )

        Spacer(modifier = Modifier.height(64.dp))

        TabSelector(
            onboardPages = onboardPages,
            currentPage = currentPage.intValue
        ) { index ->
            currentPage.intValue = index
        }
    }
}

@Composable
fun OnBoardDetails(
    modifier: Modifier = Modifier, currentPage: OnboardPage
) {
    Column(
        modifier = modifier
    ) {
        Text(
            text = currentPage.title,
            style = MaterialTheme.typography.displaySmall,
            textAlign = TextAlign.Center,
            modifier = Modifier.fillMaxWidth()
        )
        Spacer(modifier = Modifier.height(16.dp))
        Text(
            text = currentPage.description,
            style = MaterialTheme.typography.bodyMedium,
            textAlign = TextAlign.Center,
            modifier = Modifier.fillMaxWidth()
        )
    }
}

@Composable
fun OnBoardNavButton(
    modifier: Modifier = Modifier, currentPage: Int, noOfPages: Int, onNextClicked: () -> Unit, onLastButtonClicked: () -> Unit
) {
    Button(
        onClick = {
            if (currentPage < noOfPages - 1) {
                onNextClicked()
            } else {
                onLastButtonClicked()
            }
        }, modifier = modifier.testTag(TAG_ONBOARD_SCREEN_NAV_BUTTON)
    ) {
        Text(text = if (currentPage < noOfPages - 1) "Next" else "Get Started")
    }
}


@Composable
fun OnBoardImageView(modifier: Modifier = Modifier, currentPage: OnboardPage) {
    val imageRes = currentPage.imageRes
    Box(
        modifier = modifier
            .testTag(TAG_ONBOARD_SCREEN_IMAGE_VIEW + currentPage.title)
    ) {
        Image(
            painter = painterResource(id = imageRes),
            contentDescription = null,
            modifier = Modifier.fillMaxSize(),
            contentScale = ContentScale.FillWidth
        )
        Box(modifier = Modifier
            .background(Color.Transparent)
            .fillMaxSize()
            .align(Alignment.BottomCenter)
            .graphicsLayer {
                alpha = 0.6f
            }
            )
    }
}

@Composable
fun TabSelector(onboardPages: List<OnboardPage>, currentPage: Int, onTabSelected: (Int) -> Unit) {
    TabRow(
        selectedTabIndex = currentPage,
        modifier = Modifier
            .fillMaxWidth()
            .background(MaterialTheme.colorScheme.primary)
            .testTag(TAG_ONBOARD_TAG_ROW)

    ) {
        onboardPages.forEachIndexed { index, _ ->
            Tab(selected = index == currentPage, onClick = {
                onTabSelected(index)
            }, modifier = Modifier.padding(16.dp), content = {
                Box(
                    modifier = Modifier
                        .testTag("$TAG_ONBOARD_TAG_ROW$index")
                        .size(8.dp)
                        .background(
                            color = if (index == currentPage) MaterialTheme.colorScheme.onPrimary
                            else Color.LightGray, shape = RoundedCornerShape(4.dp)
                        )
                )
            })
        }
    }
}

data class OnboardPage(
    val imageRes: Int,
    val title: String,
    val description: String
)

@Preview
@Composable
fun PreviewOnboardScreen() {
    MaterialTheme {
        Surface {
            OnboardScreen(OnboardScreenViewModel())
        }
    }

}