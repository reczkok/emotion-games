using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Turning;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class LightFlicker : MonoBehaviour
{
    public GameObject dissapearDoor;
    public GameObject jumpScareSpawn;
    public List<GameObject> disableArea;
    public List<GameObject> enableArea;
    private AudioSource laughSound;
    private Light[] lights;
    private bool eventStarted = false;
    private GameObject monster;

    void Start()
    {
        laughSound = GetComponent<AudioSource>();
        lights = FindObjectsByType<Light>(FindObjectsInactive.Include, FindObjectsSortMode.None);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (eventStarted) return;
        var xrOrigin = FindFirstObjectByType<XROrigin>();
        var moveProvide = FindFirstObjectByType<DynamicMoveProvider>();
        var turnProvider = FindFirstObjectByType<ContinuousTurnProvider>();
        turnProvider.turnSpeed = 0;
        moveProvide.moveSpeed = 0;
        eventStarted = true;
        foreach (var sceneLight in lights)
        {
            sceneLight.gameObject.SetActive(false);
        }
        monster = GameObject.Instantiate(jumpScareSpawn, xrOrigin.transform.position - (xrOrigin.transform.forward * 2f), Quaternion.LookRotation(xrOrigin.transform.forward));
        xrOrigin.transform.LookAt(dissapearDoor.transform, Vector3.up);
        dissapearDoor.SetActive(false);
        StartCoroutine(EnableLights());
    }


    private IEnumerator StartMonster()
    {
        laughSound.Play();
        while (laughSound.isPlaying)
        {
            yield return null;
        }
        StartCoroutine(FindFirstObjectByType<Flashlight>().FlashlightFlicker(4));
        monster.SetActive(false);
    }

    private IEnumerator EnableLights()
    {

        yield return new WaitForSeconds(3);

        var xrOrigin = FindFirstObjectByType<XROrigin>();
        var moveProvide = FindFirstObjectByType<DynamicMoveProvider>();
        var turnProvider = FindFirstObjectByType<ContinuousTurnProvider>();
        
        xrOrigin.transform.LookAt(monster.transform, Vector3.up);
        StartCoroutine(StartMonster());
        foreach (var sceneLight in lights)
        {
            sceneLight.gameObject.SetActive(true);
        }

        turnProvider.turnSpeed = 60f;
        moveProvide.moveSpeed = 2.5f;
       
        disableArea.ForEach(x =>
        {
            x.SetActive(false);
        });
        enableArea.ForEach(x => x.SetActive(true));
    }
}
