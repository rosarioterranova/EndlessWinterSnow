using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float m_Runspeed = 6;
    [SerializeField] AudioClip[] m_FootstepsAudios;

    CharacterController2D m_Controller;
    Rigidbody2D m_Rigidbody2D;
    CharacterController2D m_CharacterController2D;
    Animator m_Animator;
    AudioSource m_AudioSource;
    SpriteRenderer m_SpriteRenderer;

    float m_HorizontalMove = 1f; //move forever
    bool m_IsJumping = false;
    bool m_IsDoubleJumping = false;
    bool m_IsCrouching = false;
    bool m_IsLanded = false;
    bool m_IsHitted = false;
    int m_Life = 5;
    bool m_IsOnGround = false;

    void Start()
    {
        m_Controller = GetComponent<CharacterController2D>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        m_CharacterController2D = GetComponent<CharacterController2D>();
        m_Animator = GetComponent<Animator>();
        m_AudioSource = GetComponent<AudioSource>();
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        m_Controller.Move(m_HorizontalMove * m_Runspeed * Time.fixedDeltaTime, m_IsCrouching, m_IsJumping);
    }

    public void Jump(bool value)
    {
        m_IsJumping = value;
        m_Animator.SetBool("Jump", value);
        if (!m_IsLanded && !m_IsDoubleJumping && m_IsJumping)
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Rigidbody2D.AddForce(new Vector2(0, m_CharacterController2D.JumpForce));
            m_IsDoubleJumping = true;
            m_IsOnGround = false;
        }
        else
        {
            m_IsLanded = false;            
        }
    }

    public void Crouch(bool value)
    {
        m_IsCrouching = value;
        m_Animator.SetBool("Duck", value);
    }

    public void Land()
    {
        Jump(false);
        m_Animator.SetTrigger("Land");
        m_IsLanded = true;
        m_IsDoubleJumping = false;
        m_Rigidbody2D.velocity = Vector2.zero;
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.layer.Equals(8))
        {
            m_IsOnGround = true;
        }
    }

    void PlayRandomFootstepsAudio()
    {
        m_AudioSource.PlayOneShot(m_FootstepsAudios[Random.Range(0, m_FootstepsAudios.Length)]);
    }

    public void Hit()
    {
        StartCoroutine(HitEffects());
    }

    IEnumerator HitEffects()
    {
        m_Animator.SetTrigger("Roll");
        m_IsHitted = true;
        m_HorizontalMove = 0;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(-800, 0));

        yield return new WaitForSeconds(.5f);
        m_Life -= 1;
        LifeEffect();
        m_IsHitted = false;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_HorizontalMove = 1;
        LifeCheck();
    }

    public void Star()
    {
        StartCoroutine(StarEffects());
    }

    IEnumerator StarEffects()
    {
        m_Animator.SetTrigger("RollMirror");
        m_IsHitted = true;
        m_HorizontalMove = 0;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_Rigidbody2D.AddForce(new Vector2(+800, 0));

        yield return new WaitForSeconds(.5f);
        if(!m_Life.Equals(5))
            m_Life += 1;
        LifeEffect();
        m_IsHitted = false;
        m_Rigidbody2D.velocity = Vector2.zero;
        m_HorizontalMove = 1;
    }

    void LifeEffect()
    {
        switch (m_Life)
        {
            case (5):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, 1f);
                m_AudioSource.volume = 1f;
                break;
            case (4):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, .75f);
                m_AudioSource.volume = .75f;
                break;
            case (3):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, .5f);
                m_AudioSource.volume = .50f;
                break;
            case (2):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, .25f);
                m_AudioSource.volume = .25f;
                break;
            case (1):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, .1f);
                m_AudioSource.volume = .10f;
                break;
            case (0):
                m_SpriteRenderer.color = SetAlpha(m_SpriteRenderer, 0f);
                m_AudioSource.volume = .0f;
                break;
        }
    }

    void LifeCheck()
    {
        if(m_Life<1)
        {
            GameManager.Instance.GameOver();
        }
    }

    Color SetAlpha(SpriteRenderer spriteRenderer, float alpha)
    {
        return new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
    }
}
