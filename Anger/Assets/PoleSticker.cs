using UnityEngine;

public class PoleSticker : MonoBehaviour
{
    [SerializeField] Transform attachment;
    [SerializeField] Transform stickPoint;
    [SerializeField] float stickAngleTreshold;
    private float currentAngleX;
    private float currentAngleZ;
    private Rigidbody rb;
    private bool stickEnabled = true;

    private Vector3 forceDirection;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!stickEnabled)
        {
            //return;
        }

        currentAngleX = transform.eulerAngles.x;
        if (currentAngleX > 360 - stickAngleTreshold)
        {
            currentAngleX = 360 - currentAngleX;
        }
        currentAngleZ = transform.eulerAngles.z;

        if (currentAngleZ > 360 - stickAngleTreshold)
        {
            currentAngleZ = 360 - currentAngleZ;
        }

        if (Mathf.Abs(currentAngleX) < stickAngleTreshold &&
            Mathf.Abs(currentAngleZ) < stickAngleTreshold &&
            (transform.position.x != attachment.position.x || transform.position.z != attachment.position.z))
        {
            Vector3 newPosition = new Vector3(
                transform.position.x - (stickPoint.position.x - attachment.position.x),
                transform.position.y,
                transform.position.z - (stickPoint.position.z - attachment.position.z));
            transform.position = newPosition;
        }
        else
        {
            stickEnabled = false;
        }
    }
    void OnTriggerEnter(Collider other)
    {   
        
        if (other.gameObject.tag == "enemy")
        {
            audioSource.Play();
            forceDirection = new Vector3(
                           Random.Range(-45, 45),
                           0,
                           Random.Range(-45, 45));
            forceDirection.Normalize();
            Vector3 force = forceDirection * Random.Range(100, 275);
            rb.AddForce(force);
        }
        if (other.gameObject.tag == "Finish")
        {
            Debug.Log(FindObjectOfType<Score>().score);
            //FindObjectOfType<SCOREBOARD>().UpdateScoreboard();
            FindObjectOfType<GameManager>().EndGame();
        }
    }
}
