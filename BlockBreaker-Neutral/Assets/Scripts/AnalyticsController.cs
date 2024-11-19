using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Analytics;
using Unity.Services.Core;
using Unity.Services.Core.Analytics;
using Unity.Services.Core.Environments;
using UnityEngine;
using UnityEngine.UI;

/**
 * Ustawianie anaylityki dla projektu https://docs.unity3d.com/Manual/SettingUpProjectServices.html
 */
public class AnalyticsController : MonoBehaviour
{
    public string userId;
    public static AnalyticsController Instance;

    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    public async Task InitializeAnalytics()
    {
        var textInput = FindFirstObjectByType<TMP_InputField>();
        userId = textInput.text;
        var options = new InitializationOptions();
        options.SetEnvironmentName("production");
        UnityServices.ExternalUserId = userId;
        await UnityServices.InitializeAsync(options);
    }

    public async Task SetupAnalytics(string user)
    {
        userId = user;
        await InitializeAnalytics();
    }


    public void SendAnalyticsEvent(Dictionary<string, object> parameters, string eventName, bool enforceFlush = false)
    {
        // AnalyticsService.Instance.CustomData(eventName, parameters);
        if (enforceFlush)
        {
            AnalyticsService.Instance.Flush();
        }
    }
}
