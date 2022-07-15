using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using Unity.Services.Core.Environments;
using UnityEngine;

public class AnalyticsController : MonoBehaviour
{
    string consentIdentifier;
    bool consentHasBeenChecked;
    private const string ADS_PERSONALIZATION_CONSENT = "Ads";
    // Start is called before the first frame update
    async void Start()
    {
        Debug.Log("Terms of Service has been accepted: " + SimpleGDPR.IsTermsOfServiceAccepted);
        Debug.Log("Ads personalization consent state: " + SimpleGDPR.GetConsentState(ADS_PERSONALIZATION_CONSENT));
        Debug.Log("Is user possibly located in the EEA: " + SimpleGDPR.IsGDPRApplicable);

    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0))
        //{
        //    // Don't attempt to show a dialog if another dialog is already visible
        //    if (SimpleGDPR.IsDialogVisible)
        //        return;

        //        // Show a dialog that prompts the user to accept the Terms of Service and Privacy Policy
        //        SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
        //            SetPrivacyPolicyLink(Events.PrivacyUrl),
        //            TermsOfServiceDialogClosed);


        //}
    }

    public async void InitializeAnalytics()
    {
        var textInput = FindObjectOfType<TMP_InputField>();
        var userId = textInput.text;
        var options = new InitializationOptions();
        options.SetEnvironmentName("production");
        options.SetAnalyticsUserId(userId);
        try
        {
            await UnityServices.InitializeAsync(options);
            List<string> consentIdentifiers = await Events.CheckForRequiredConsents();
            if (consentIdentifiers.Count > 0)
            {
                consentIdentifier = consentIdentifiers[0];
                SimpleGDPR.ShowDialog(new TermsOfServiceDialog().
                    SetPrivacyPolicyLink(Events.PrivacyUrl),
                    TermsOfServiceDialogClosed);
            }

        }
        catch (ConsentCheckException e)
        {
            // Something went wrong when checking the GeoIP, check the e.Reason and handle appropriately
            Debug.Log(e.Reason);
            throw;
        }
    }

    public void OptOut()
    {
        try
        {
            if (!consentHasBeenChecked)
            {
                // Show a GDPR/COPPA/other opt-out consent flow
                // If a user opts out
                Events.OptOut();
            }
            // Record that we have checked a user's consent, so we don't repeat the flow unnecessarily. 
            // In a real game, use PlayerPrefs or an equivalent to persist this state between sessions
            consentHasBeenChecked = true;
        }
        catch (ConsentCheckException e)
        {
            Debug.LogError(e.Reason);
            // Handle the exception by checking e.Reason
        }
    }

    public void onShowPrivacyPageButtonPressed()
    {
        // Open the Privacy Policy in the system's default browser
        Application.OpenURL(Events.PrivacyUrl);
    }

    private void TermsOfServiceDialogClosed()
    {
        // We can assume that user has accepted the terms because
        // TermsOfServiceDialog dialog can only be closed via the 'Accept' button
        Events.ProvideOptInConsent(consentIdentifier, true);
        Debug.Log("Accepted Terms of Service");
    }

}
