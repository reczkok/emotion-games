using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public float fallingspeed;
    private Rigidbody eagle;
    private bool Dead;

    float timer;
    public float deathtimer = 1;
    public GameObject OnDestroyFuncTarget;
    public string OnDestroyFuncMessage;
    // Start is called before the first frame update
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            eagle = GetComponent<Rigidbody>();
            eagle.useGravity = true;
        }
    }
    void Update()
    {
        eagle = GetComponent<Rigidbody>();
        if (eagle.useGravity == true)
        {
            transform.Translate(Vector3.forward * fallingspeed);
            Dead = true;
        }
        if (Dead == true)
        {
            timer += Time.deltaTime;

            if (timer >= deathtimer)
            {
                if (OnDestroyFuncTarget != null)
                    OnDestroyFuncTarget.SendMessage(OnDestroyFuncMessage);
                Destroy(gameObject);
            }
        }
    }
}
