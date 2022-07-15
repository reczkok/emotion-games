using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimeManager : MonoBehaviour
{
    TextMeshProUGUI text;
    float timeValue = 0;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        timeValue = 5 * 60;
    }

    // Update is called once per frame
    void Update()
    {
        timeValue = timeValue - Time.deltaTime;
        var min = (int)timeValue / 60;
        var s = timeValue % 60;
        if (min <= 0 && s <= 0)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>()
            {
            };
            // The ‘myEvent’ event will get queued up and sent every minute
            Events.CustomData("outOfTime", parameters);
            Events.Flush();
            SceneManager.LoadScene(1);
        }

        text.text = "" + Mathf.Round(min) + ":" + (Mathf.Round(s) < 10 ? "0" + Mathf.Round(s) : "" + Mathf.Round(s));

    }
}
