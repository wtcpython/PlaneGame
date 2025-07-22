using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3;
    public int hp = 1;
    public int score = 10;

    private EnemyState state = EnemyState.Idle;
    
    public Sprite[] sprites;
    public Sprite[] damagedSprite;
    public Sprite[] deadSprite;

    private SpriteRenderer spriteRenderer;
    
    private float timer = 0;
    private int currentFrame = 0;
    
    public AudioSource audioSource;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    void Update()
    {
        MoveUpdate();
        
        AnimationUpdate();
        
        PlayDamageAnimation();
        PlayDeadAnimation();
    }

    private void MoveUpdate()
    {
        if (state == EnemyState.Dead) return;
        transform.Translate(Vector3.down * (speed * Time.deltaTime));
        
        if (transform.position.y < -Constants.DestroyYLimit)
        {
            Destroy(this.gameObject);
        }
    }

    private void ResetFrame()
    {
        timer = 0;
        currentFrame = 0;
    }
    
    private void AnimationUpdate()
    {
        if (state != EnemyState.Idle) return;
        timer += Time.deltaTime;
        if (timer >= 1.0f / Constants.FrameRate)
        {
            currentFrame = (currentFrame + 1) % sprites.Length;
            timer -= 1.0f / Constants.FrameRate;
            spriteRenderer.sprite = sprites[currentFrame];
        }
    }

    void PlayDamageAnimation()
    {
        if (state != EnemyState.Damage) return;
        
        timer += Time.deltaTime;
        if (timer >= 1.0f / Constants.FrameRate)
        {
            currentFrame++;
            timer -= 1.0f / Constants.FrameRate;
        }

        if (currentFrame >= damagedSprite.Length)
        {
            // 回归正常动画
            state = EnemyState.Idle;
            ResetFrame();
        }
        else
        {
            spriteRenderer.sprite = damagedSprite[currentFrame];
        }
    }

    void PlayDeadAnimation()
    {
        if (state != EnemyState.Dead) return;
        
        timer += Time.deltaTime;
        if (timer >= 1.0f / Constants.FrameRate)
        {
            currentFrame++;
            timer -= 1.0f / Constants.FrameRate;
        }

        if (currentFrame >= deadSprite.Length)
        {
            Destroy(this.gameObject);
        }
        else
        {
            spriteRenderer.sprite = deadSprite[currentFrame];
        }
    }

    public void TakeDamage(int damage = 1)
    {
        if (hp <= 0) return;
        hp -= damage;
        if (hp <= 0)
        {
            // 死亡处理
            Dead();
        }
        else
        {
            // 受伤处理
            Debug.Log("HIT");
            state = EnemyState.Damage;
        }

        ResetFrame();
    }

    private void Dead()
    {
        Debug.Log("DEAD");
        state = EnemyState.Dead;
        GameManager.Instance.AddScore(score);
        audioSource.Play();
    }
}
