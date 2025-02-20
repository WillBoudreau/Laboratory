using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [Header("Level Settings")]
    [SerializeField] private UIManager uIManager;//The UI Manager
    [SerializeField] private GameManager gameManager;//The game manager
    [SerializeField] 
    private GameObject player; 
    public GameObject spawn;
    public List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    void Start()
    {
        //If the UIManager is null, find the UIManager
        if(uIManager == null)
        {
            uIManager = FindObjectOfType<UIManager>();
        }
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
        }
        if(sceneName.Contains("MainMenu"))
        {
            Debug.Log("Setting Ui to menu");
            uIManager.UILoadingScreen(uIManager.mainMenu); 
        }  
        StartCoroutine(WaitForScreenLoad(sceneName));
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
        spawn = GameObject.FindWithTag("Spawn");
        if(spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
        else
        {
            Debug.Log("Spawn Not Found!")
            SceneManager.sceneLoaded -= OnSceneLoaded;
            return;
        }
        if(scene.name.StartsWith("L_"))
        {
            player.SetActive(true);
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
}
