using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    [HeaderAttribute("Menu Elements")]
    [SerializeField] GameObject m_VirtualCamMenu;
    [SerializeField] GameObject m_LogoBeasties;
    [SerializeField] GameObject m_MainMenuCanvas;

    [HeaderAttribute("Game Elements")]
    [SerializeField] GameObject m_Character;
    [SerializeField] GameObject m_Shadow;
    [SerializeField] GameObject m_VirtualCamGame;
    [SerializeField] GameObject m_GameCanvas;
    [SerializeField] GameObject m_TutorialCanvas;
    [SerializeField] Text m_TutorialText;
    [SerializeField] GameObject m_LeftHandTutorial;
    [SerializeField] GameObject m_RightHandTutorial;
    [SerializeField] List<GameObject> m_Enemies = new List<GameObject>();
    [SerializeField] GameObject m_Star;

    [HeaderAttribute("Score Elements")]
    [SerializeField] GameObject m_ScoreCanvas;
    [SerializeField] GameObject m_House;
    [SerializeField] Text m_DistanceText;

    [HeaderAttribute("Managers Elements")]
    [SerializeField] Animator m_FadeWhite;

    int m_Level = -1;
    bool m_IsCountingDistance = false;
    float m_Distance = 0;
    int m_EnemiesSpawned = 0;
    float m_EnemyPositionLow = -1.4f;
    float m_EnemyPositionMedium = -.5f;
    float m_EnemyPositionHigh = 2f;
    bool m_IsPlayerDead = false;
    bool m_IsTutorial = false;
    int m_TutorialState = 0;

    void Update()
    {
        if(m_IsCountingDistance)
        {
            m_Distance += Time.deltaTime;
        }
    }

    public void StartGame()
    {
        m_MainMenuCanvas.SetActive(false);
        m_Character.SetActive(true);
        m_Shadow.SetActive(true);
        m_VirtualCamGame.SetActive(true);
        m_GameCanvas.SetActive(true);
        StartCoroutine(StartTutorial());
    }

    IEnumerator StartTutorial()
    {
        m_IsTutorial = true;
        yield return new WaitForSeconds(3);
        m_TutorialText.text = "Hold this side to crouch";
        m_TutorialCanvas.SetActive(true);
        m_LeftHandTutorial.SetActive(true);
        m_RightHandTutorial.SetActive(false);
    }

    public void OnCrouchPressed()
    {
        if (!m_IsTutorial) { return; }
        if(m_TutorialState.Equals(0))
        {
            m_TutorialState = 1;
            m_LeftHandTutorial.SetActive(false);
            m_TutorialText.text = "Hold this side to jump";
            m_RightHandTutorial.SetActive(true);
        }
    }

    public void OnJumpPressed()
    {
        if (!m_IsTutorial) { return; }
        if (m_TutorialState.Equals(2))
        {
            m_TutorialState = 3;
            StartCoroutine(StartingGame());
            m_IsTutorial = false;
            m_TutorialCanvas.SetActive(false);
        }
        if (m_TutorialState.Equals(1))
        {
            m_TutorialState = 2;
            m_LeftHandTutorial.SetActive(false);
            m_TutorialText.text = "Press twice to jump higher";
            m_RightHandTutorial.SetActive(true);
        }
    }

    IEnumerator StartingGame()
    {
        m_IsCountingDistance = true;
        m_Level = 1;
        int EnemyToNextLevel = 5;
        yield return new WaitForSeconds(2); //first play
        while (!m_IsPlayerDead)
        {
            while (!m_EnemiesSpawned.Equals(EnemyToNextLevel) && !m_IsPlayerDead)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(2);
            }
            if (!m_Level.Equals(5))
            {
                m_Level += 1;
                EnemyToNextLevel += 5;
            }
        }
    }

    void SpawnEnemy()
    {

        //Random Spawn Star
        if (Random.Range(1, 6).Equals(1))
        {
            GameObject star = Instantiate(m_Star);
            float xStarPosition = m_Character.transform.position.x + 15;
            float yStarPosition = 0;
            switch (Random.Range(0, 2))
            {
                case 0:
                    yStarPosition = m_EnemyPositionLow;
                    break;
                case 1:
                    yStarPosition = m_EnemyPositionMedium;
                    break;
                case 2:
                    yStarPosition = m_EnemyPositionHigh;
                    break;
            }
            star.transform.position = new Vector2(xStarPosition, yStarPosition);

        }
        else
        {
            int index = Random.Range(0, m_Level);
            GameObject enemy = Instantiate(m_Enemies[index]);
            float xEnemyPosition = m_Character.transform.position.x + 15;
            float yEnemyPosition = 0;
            switch (index)
            {
                case 0: //Snowball
                    yEnemyPosition = m_EnemyPositionLow;
                    break;
                case 1: //Penguin
                    yEnemyPosition = m_EnemyPositionMedium;
                    break;
                case 2: //Friend
                    yEnemyPosition = m_EnemyPositionMedium;
                    break;
                case 3: //Snowman
                    xEnemyPosition += Random.Range(-4, 2);
                    yEnemyPosition = m_EnemyPositionLow;
                    break;
                case 4: //Polar Bear
                    yEnemyPosition = m_EnemyPositionMedium;
                    break;
                case 5: //Reindeer Storm
                    yEnemyPosition = m_EnemyPositionHigh;
                    break;
            }
            enemy.transform.position = new Vector2(xEnemyPosition, yEnemyPosition);
            m_EnemiesSpawned += 1;
        }
    }

    public void OnEnemyHitPlayer()
    {
        m_Character.GetComponent<PlayerMovement>().Hit();
    }

    public void OnStarHitPlayer()
    {
        m_Character.GetComponent<PlayerMovement>().Star();
    }

    public void GameOver()
    {
        if(m_IsPlayerDead) { return; }
        m_IsPlayerDead = true;
        m_DistanceText.text = ((int)m_Distance).ToString();
        StartCoroutine(GameOverAnimation());
    }

    IEnumerator GameOverAnimation()
    {
        m_FadeWhite.SetBool("FadeIN", true);
        yield return new WaitForSeconds(2f);
        m_LogoBeasties.SetActive(false);
        m_ScoreCanvas.SetActive(true);
        m_House.SetActive(true);
        m_Character.SetActive(false);
        m_Shadow.SetActive(false);
        m_VirtualCamGame.SetActive(false);
        m_GameCanvas.SetActive(false);
        yield return new WaitForSeconds(2f);
        m_FadeWhite.SetBool("FadeIN", false);
    }

    public void OnRestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
