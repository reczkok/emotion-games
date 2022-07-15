using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    // Start is called before the first frame update
    void Start()
    {
        laughSound = GetComponent<AudioSource>();
        lights = FindObjectsOfType<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (!eventStarted)
        {
            var player = FindObjectOfType<FirstPersonController>();
            player.IsMovementBlocked = true;
            eventStarted = true;
            foreach (var light in lights)
            {
                light.gameObject.SetActive(false);
            }
            monster = GameObject.Instantiate(jumpScareSpawn, player.transform.position - (player.transform.forward * 2f), Quaternion.LookRotation(player.transform.forward));
            player.transform.LookAt(dissapearDoor.transform, Vector3.up);
            dissapearDoor.SetActive(false);
            StartCoroutine(EnableLights());
        }
    }


    private IEnumerator StartMonster()
    {
        laughSound.Play();
        while (laughSound.isPlaying)
        {
            yield return null;
        }
        StartCoroutine(FindObjectOfType<Flashlight>().FlashligthFlicker(4));
        monster.SetActive(false);
    }

    private IEnumerator EnableLights()
    {

        yield return new WaitForSeconds(3);

        var player = FindObjectOfType<FirstPersonController>();
        player.transform.LookAt(monster.transform, Vector3.up);
        StartCoroutine(StartMonster());
        foreach (var light in lights)
        {
            light.gameObject.SetActive(true);
        }

        player.IsMovementBlocked = false;
       
        disableArea.ForEach(x =>
        {
            x.SetActive(false);
        });
        enableArea.ForEach(x => x.SetActive(true));
    }
}
