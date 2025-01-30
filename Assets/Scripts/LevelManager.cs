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
        SceneManager.LoadScene(sceneName);
        uIManager.LoadUI(sceneName);
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
        if(scene.name.Contains("L_"))
        {
            gameManager.gameState = GameManager.GameState.Gameplay;
            gameManager.playerCon.boundingBox = GameObject.FindWithTag("BoundingBox").GetComponent<Collider2D>();
            gameManager.playerCon.SetBoundingBox();
        }
        else
        {
            gameManager.gameState = GameManager.GameState.MainMenu;
        }
        Debug.Log("SceneLoaded");
        spawn = GameObject.FindWithTag("Spawn");
        if(spawn != null)
        {
            player.transform.position = spawn.transform.position;
        }
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

     /// <summary>
    /// Waits for screen to load before starting operation. 
    /// </summary>
    /// <param name="sceneName"></param>
    /// <returns></returns>
    private IEnumerator WaitForScreenLoad(string sceneName)
    {
        yield return new WaitForSeconds(uIManager.fadeTime);
        //Debug.Log("Loading Scene Starting");

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
        float totalprogress = 0;

        foreach (AsyncOperation operation in scenesToLoad)
        {
            totalprogress += operation.progress * Time.deltaTime;
        }

        return totalprogress / scenesToLoad.Count;
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
