using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [Header("Class References")]
    [SerializeField] private UIManager uIManager;//The UI Manager
    [SerializeField] private GameManager gameManager;//The game manager
    [SerializeField] private MusicHandler musicHandler;//The music handler
    [SerializeField] private VoiceLineManager vLManager;
    [Header("Level Variables")]
    private GameObject player; 
    public GameObject spawn;
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    public int activeLevelNumber;
    public string sceneName;
    [SerializeField] private float sceneLoadTime = 2.0f;//The time it takes to load a scene
    void Start()
    {
        //If the UIManager is null, find the UIManager
        if(uIManager == null)
        {
            uIManager = FindObjectOfType<UIManager>();
        }
        vLManager = FindObjectOfType<VoiceLineManager>();
        player = gameManager.player;
    }
    /// <summary>
    /// Load the specified level based on the sceneName variable
    /// </summary>
    /// <param name="sceneName">The name of the scene to load</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        if(sceneName.Contains("L_"))
        {
            uIManager.UILoadingScreen(uIManager.hUD);
            if(sceneName.Contains("Tutorial"))
            {
                Debug.Log("Setting music to tutorial");
                musicHandler.SwitchAudioTrack("tutorial");
            }
            else if(sceneName.Contains("1"))
            {
                Debug.Log("Setting music to level1");
                musicHandler.SwitchAudioTrack("level1");
            }
            else if(sceneName.Contains("2"))
            {
                Debug.Log("Setting music to level2");
                musicHandler.SwitchAudioTrack("level2");
            }
            else if(sceneName.Contains("3"))
            {
                Debug.Log("Setting music to level3");
                musicHandler.SwitchAudioTrack("level3");
            }
        }
        if(sceneName.Contains("MainMenu"))
        {
            Debug.Log("Setting Ui to menu");
            musicHandler.SwitchAudioTrack("title");
            uIManager.UILoadingScreen(uIManager.mainMenu); 
        }  
        StartCoroutine(WaitForScreenLoad(sceneName));
    }
    /// <summary>
    /// used to load the next level when all puzzles complete
    /// </summary>
    public void LoadNextLevel()
    {
        sceneName = SceneManager.GetActiveScene().name;

        if(sceneName == "L_1" || sceneName == "L_2")
        {
            activeLevelNumber += 1;
        }
        else
        {
            activeLevelNumber = 0;
            activeLevelNumber += 1;
        }
        LoadScene(string.Format("L_{0}", activeLevelNumber));
    }
    /// <summary>
    /// Quit the game function
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneName = SceneManager.GetActiveScene().name;
        spawn = GameObject.FindWithTag("Spawn");
        uIManager.volume = FindObjectOfType<Volume>();
        gameManager.playerCon.isZoomedOut = false;
        gameManager.playerCon.confiner.InvalidateCache();
        if(spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
        else
        {
            Debug.Log("Spawn Not Found!");
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }
        if(scene.name.StartsWith("L"))
        {
            player.SetActive(true);
            vLManager.GetAllSpeakers();
            if(scene.name.Contains("Tutorial"))
            {
                vLManager.PlayVoiceLine(vLManager.voiceLines[5]);
                vLManager.firstDoor = GameObject.FindWithTag("1stDoor");
            }
            else
            {
                vLManager.firstDoor = null;
            }
            if(scene.name.Contains("2"))
            {
                gameManager.playerCon.faceLeft();
            }
            if(scene.name.Contains("3"))
            {
                vLManager.PlayVoiceLine(vLManager.voiceLines[12]);
            }
            gameManager.gameState = GameManager.GameState.Gameplay;
            gameManager.playerCon.boundingBox = GameObject.FindWithTag("BoundingBox").GetComponent<Collider2D>();
            gameManager.playerCon.SetBoundingBox();
        }
        else if (scene.name.StartsWith("Main"))
        {
            gameManager.gameState = GameManager.GameState.MainMenu;
            Time.timeScale = 1;
            player.SetActive(false);
        }
        Debug.Log("SceneLoaded");
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

     /// <summary>
    /// Waits for screen to load before starting operation. 
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        Debug.Log("Loading Scene " + sceneName + " Starting");
        yield return new WaitForSeconds(uIManager.fadeTime);

        yield return new WaitForSeconds(sceneLoadTime);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.completed += OperationCompleted;
        scenesToLoad.Add(operation);
    }
    /// <summary>
    /// Gets average progress for Loading bar. 
    /// </summary>
    /// <returns></returns>
    public float GetLoadingProgress()
    {
        float totalProgress = 0;

        foreach (AsyncOperation operation in scenesToLoad)
        {
            totalProgress += operation.progress;
        }

        return totalProgress / scenesToLoad.Count;
    }
    /// <summary>
    /// Event for when load operation is finished. 
    /// </summary>
    /// <param name="operation"></param>
    private void OperationCompleted(AsyncOperation operation)
    {
        scenesToLoad.Remove(operation);
        operation.completed -= OperationCompleted;
    }

    public void DebugLoadScene(string scenesToLoad)
    {
        if(scenesToLoad == "L_1")
        {
            activeLevelNumber = 1;
        }
        if(scenesToLoad == "L_2")
        {
            activeLevelNumber = 2;
        }
        LoadScene(scenesToLoad);
    }
}
