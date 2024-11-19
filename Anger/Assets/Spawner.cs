using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class deployAsteroids : MonoBehaviour
{
    public GameObject Eagle_ElitePrefab;
    public GameObject Eagle_NormalPrefab;
    public int respawnTime = 3;
    float timer;
    float oldtime = 8;
    public AudioSource audioSource1;
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    int audioselector;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(asteroidWave());
    }
    private void spawnElite()
    {
        GameObject a = Instantiate(Eagle_ElitePrefab) as GameObject;
        a.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(2, 18), Random.Range(-50, 50));
    }
    private void spawnNormal()
    {
        GameObject a = Instantiate(Eagle_NormalPrefab) as GameObject;
        a.transform.position = new Vector3(Random.Range(-50, 50), Random.Range(2, 18), Random.Range(-50, 50));
    }
    IEnumerator asteroidWave()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (timer > oldtime + respawnTime)
            {
                oldtime = timer;
                audioselector = Random.Range(1, 4);
                if (audioselector == 1)
                {
                    audioSource1.Play();
                }
                if (audioselector == 2)
                {
                    audioSource2.Play();
                }
                if (audioselector == 3)
                {
                    audioSource3.Play();
                }
                if (timer >= 10 && timer <= 50)
                {
                    yield return new WaitForSeconds(respawnTime);
                    spawnNormal();
                    timer += respawnTime;
                }
                if (timer >= 30 && timer <= 50)
                {
                    yield return new WaitForSeconds(respawnTime);
                    spawnElite();
                    timer += respawnTime;
                }
                if (timer >= 50)
                {
                    yield return new WaitForSeconds(respawnTime);
                    spawnNormal();
                    spawnElite();
                    timer += respawnTime;
                }
                if (timer >= 65)
                {
                    respawnTime = 2;
                }
                if (timer >= 75)
                {
                    respawnTime = 1;
                }
            }
        }
    }
}