using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<SceneLoader>().ReloadCurrentLevel(); ;
        Dictionary<string, object> parameters = new Dictionary<string, object>()
        {
        };
        // The ‘myEvent’ event will get queued up and sent every minute
        Events.CustomData("playerDeath", parameters);
        Events.Flush();
    }
}
