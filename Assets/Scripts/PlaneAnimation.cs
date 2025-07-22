using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneAnimation : MonoBehaviour
{
    public Sprite[] sprites;

    private SpriteRenderer spriteRenderer;

    private float timer;
    private int currentFrame;

    public float superBulletDuration = 5;
    private float superBulletTimer;
    public GameObject[] Guns = new GameObject[3];

    public int hp = 5;
    public int invincibleTime = 3;
    public bool isInvincible = false;
    public float invincibleTimer = 0;

    public float blinkInterval = 0.1f;

    public Sprite[] deathSprites;

    public AudioSource getBombAudio;
    public AudioSource getBulletAudio;

    private void Start()
    {
        UIManager.Instance.UpdateLiftNum(hp);
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (hp > 0)
        {
            AnimationUpdate();
            MoveUpdate();
            SuperGunUpdate();
        }
        else
        {
            DeathAnimationUpdate();
        }
    }

    private void AnimationUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f / Constants.FrameRate)
        {
            currentFrame = (currentFrame + 1) % sprites.Length;
            timer -= 1.0f / Constants.FrameRate;
            spriteRenderer.sprite = sprites[currentFrame];
        }
    }

    private void DeathAnimationUpdate()
    {
        timer += Time.deltaTime;
        if (timer >= 1.0f / Constants.FrameRate)
        {
            currentFrame++;
            timer -= 1.0f / Constants.FrameRate;
        }

        if (currentFrame >= deathSprites.Length)
        {
            GameManager.Instance.GameOver();
        }
        else
        {
            spriteRenderer.sprite = deathSprites[currentFrame];
        }
    }

    private void MoveUpdate()
    {
        float horizontal = 0f;
        float vertical = 0f;
        if (Gamepad.current != null)
        {
            Vector2 input = Gamepad.current.leftStick.ReadValue();
            horizontal = input.x;
            vertical = input.y;
        }
        if (Keyboard.current.anyKey.isPressed)
        {
            if (Keyboard.current.wKey.isPressed)
            {
                vertical = 1f;
            }
            else if (Keyboard.current.sKey.isPressed)
            {
                vertical = -1f;
            }
            else if (Keyboard.current.aKey.isPressed)
            {
                horizontal = -1f;
            }
            else if (Keyboard.current.dKey.isPressed)
            {
                horizontal = 1f;
            }
        }

        Vector3 movement = new Vector3(horizontal, vertical, 0f) * (Constants.PlayerSpeed * Time.deltaTime);
        transform.position += movement;
        CheckPosition();
    }

    private void CheckPosition()
    {
        Vector3 position = transform.position;
        if (position.x < -Constants.PlayerXLimit)
        {
            position.x = -Constants.PlayerXLimit;
        }
        else if (position.x > Constants.PlayerXLimit)
        {
            position.x = Constants.PlayerXLimit;
        }

        if (position.y < -Constants.PlayerYLimit)
        {
            position.y = -Constants.PlayerYLimit;
        }
        else if (position.y > Constants.PlayerYLimit)
        {
            position.y = Constants.PlayerYLimit;
        }
        transform.position = position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Award"))
        {
            Debug.Log("Get Award");
            if (collision.GetComponent<Award>().awardType == AwardType.Bullet)
            {
                SwitchGun(true);
                superBulletTimer = superBulletDuration;
                getBulletAudio.Play();
            }
            else
            {
                GameManager.Instance.UpdateBomb(1);
                getBombAudio.Play();
            }
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("Enemy") && !isInvincible)
        {
            Debug.Log("Invincible");
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.TakeDamage(enemy.hp);
            this.hp--;
            UIManager.Instance.UpdateLiftNum(hp);
            if (hp <= 0)
            {
                // 死亡
                TransformToDeath();
            }
            else
            {
                TransformToInvincible();
            }
        }
    }

    private void TransformToInvincible()
    {
        isInvincible = true;
        invincibleTimer = 0;
        StartCoroutine(BlinkEffect());
    }

    private void TransformToDeath()
    {
        timer = 0;
        currentFrame = 0;
        for (int i = 0; i < Guns.Length; i++)
        {
            Guns[i].SetActive(false);
        }
    }

    private void SuperGunUpdate()
    {
        if (superBulletTimer > 0)
        {
            superBulletTimer -= Time.deltaTime;
        }
        else
        {
            SwitchGun(false);
        }
    }

    private void SwitchGun(bool enable)
    {
        Guns[1].SetActive(enable);
        Guns[2].SetActive(enable);
    }

    private IEnumerator BlinkEffect()
    {
        while (invincibleTimer <= invincibleTime)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
            invincibleTimer += blinkInterval;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }
}
