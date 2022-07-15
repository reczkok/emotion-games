using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip DestroySound = null;
    [SerializeField] GameObject OnDeleteParticle = null;
    [SerializeField] int MaxHits = 3;
    [SerializeField] Sprite[] HitSprites=null;
    Level level = null;
    SpriteRenderer spriteRenderer = null;
    void Start()
    {
        level = FindObjectOfType<Level>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxHits = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (tag == "Breakable")
        {
            MaxHits--;
            if (collision.gameObject.GetComponent<Ball>() != null && MaxHits <= 0)
            {
                AudioSource.PlayClipAtPoint(DestroySound, collision.transform.position);
                level.RemoveBlock();
                PlayVFX();
                Destroy(gameObject);
                return;
            }
            spriteRenderer.sprite = HitSprites[MaxHits-1];

        }
    }

    private void PlayVFX()
    {
        GameObject sparkless = Instantiate(OnDeleteParticle, transform.position,transform.rotation);
        Destroy(sparkless, 1f);
    }
}
