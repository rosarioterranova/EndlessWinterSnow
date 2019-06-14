using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeAnimController : MonoBehaviour
{

    [SerializeField] Animator m_Pupil, m_Blink;

    void Start()
    {
        StartCoroutine(Blink());
        StartCoroutine(LookingAround());
    }

    IEnumerator LookingAround()
    {
        while(gameObject)
        {
            yield return new WaitForSecondsRealtime(Random.Range(1, 5));
            m_Pupil.SetBool("LookAround", true);
            yield return new WaitForSecondsRealtime(Random.Range(1, 5));
            m_Pupil.SetBool("LookAround", false);
        }
    }

    IEnumerator Blink()
    {
        while (gameObject)
        {
            yield return new WaitForSecondsRealtime(Random.Range(1, 5));
            m_Blink.SetTrigger("Blink");
        }
    }
}
