using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] AudioClip DestroySound;
    [SerializeField] GameObject OnDeleteParticle;
    [SerializeField] int MaxHits = 3;
    [SerializeField] Sprite[] HitSprites;
    Level level;
    SpriteRenderer spriteRenderer;
    void Start()
    {
        level = FindFirstObjectByType<Level>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        MaxHits = 3;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!CompareTag("Breakable")) return;
        
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

    private void PlayVFX()
    {
        var sparkles = Instantiate(OnDeleteParticle, transform.position,transform.rotation);
        Destroy(sparkles, 1f);
    }
}
