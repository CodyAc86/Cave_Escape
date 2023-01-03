using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Rocket : MonoBehaviour {

    [SerializeField] float destroyDelay = 1f;
    [SerializeField] float levelLoadDelay = .8f;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip crash;
    [SerializeField] ParticleSystem crashParticles;
    
    public Image levelCompleteImage;
    public Image gameCompleteImage;
    public Button nextLevel;
    public Button mainMenu;
    public TextMeshProUGUI youWon;
    public TextMeshProUGUI levelComplete;

    AudioSource audioSource;

    bool isTransitioning = false;
    
    void Start()
    {
        DisableLevelCompleteWindow();
        DisableGameOverWindow();

        audioSource = GetComponent<AudioSource>();
    }

    void DisableGameOverWindow()
    {
        youWon.gameObject.SetActive(false);
        gameCompleteImage.gameObject.SetActive(false);
        mainMenu.gameObject.SetActive(false);
    }

    void DisableLevelCompleteWindow()
    {
        levelCompleteImage.gameObject.SetActive(false);
        nextLevel.gameObject.SetActive(false);
        levelComplete.gameObject.SetActive(false);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isTransitioning)
        {
            return;
        }
        switch (collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                Debug.Log("OK"); //todo remove
                break;
            
            case "Finish":
                StopMovement();
                audioSource.PlayOneShot(success);
                ShowWinWindow();
                break;
            default:
                StartCrashSequence();
                break;
        }
    }
    

    void ShowWinWindow()
    {
        levelCompleteImage.gameObject.SetActive(true);
        nextLevel.gameObject.SetActive(true);
        levelComplete.gameObject.SetActive(true);
    }
    void ShowFinishWIndow()
    {
        gameCompleteImage.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);
        youWon.gameObject.SetActive(true);
    }


    void ReloadLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    void LoadNextLevel()
    {       
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {            
            nextSceneIndex = 0;            
        }
        
        SceneManager.LoadScene(nextSceneIndex);

    }
    void LoadMainMenu()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        SceneManager.LoadScene(nextSceneIndex);
    }
    void StartCrashSequence()
    {
        StopMovement();
        audioSource.PlayOneShot(crash);
        crashParticles.Play();
        Invoke("ReloadLevel", destroyDelay);
    }
    public void GoToMainMenu()
    {
        Invoke("LoadMainMenu", levelLoadDelay);
    }
    public void StartNextLevel()
    {                
        Invoke("LoadNextLevel", levelLoadDelay);
    }

    void StopMovement()
    {
        isTransitioning = true;
        audioSource.Stop();
        GetComponent<Movement>().enabled = false;
    }
    
}
