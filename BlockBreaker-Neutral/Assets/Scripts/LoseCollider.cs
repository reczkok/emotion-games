using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseCollider : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindFirstObjectByType<SceneLoader>().ReloadCurrentLevel(); ;
        var parameters = new Dictionary<string, object>();
    }
}
