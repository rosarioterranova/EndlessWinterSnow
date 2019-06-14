using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchController : MonoBehaviour
{
    [SerializeField] PlayerMovement m_PlayerMovement;
    [SerializeField] ButtonCallback m_CrouchButton;
    [SerializeField] ButtonCallback m_JumpButton;

    void Start()
    {
        m_CrouchButton.OnPressed += OnCrouchPressed;
        m_CrouchButton.OnReleased += OnCrouchReleased;
        m_JumpButton.OnPressed += OnJumpPressed;
        m_JumpButton.OnReleased += OnJumpReleased;
    }

    public void OnCrouchPressed()
    {
        m_PlayerMovement.Crouch(true);
        GameManager.Instance.OnCrouchPressed();
    }

    public void OnCrouchReleased()
    {
        m_PlayerMovement.Crouch(false);
    }

    public void OnJumpPressed()
    {
        m_PlayerMovement.Jump(true);
        GameManager.Instance.OnJumpPressed();
    }

    public void OnJumpReleased()
    {
        m_PlayerMovement.Jump(false);
    }
}
