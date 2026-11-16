
using UnityEngine;

// Opens the Forward playtest feedback survey in a web browser.
public class FeedbackLink : MonoBehaviour
{
    private const string SurveyUrl =
        "https://forms.cloud.microsoft/Pages/ResponsePage.aspx?id=OcL4BRfLVUejRQSHUgY8O1c-82x4-A1NrmYP2RFfsSpURVJDNk5QQzZUTjFPUzM2VzE3T00zMTg3Si4u";

    // Opens the survey when the Feedback button is pressed.
    public void OpenFeedback()
    {
        Application.OpenURL(SurveyUrl);
    }
}
