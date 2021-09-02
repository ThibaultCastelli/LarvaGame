using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveManager : MonoBehaviour
{
    #region Variables
    [SerializeField] EventInt loadSceneEvent;

    GameObject _collectible;

    public Save currentSave { get; private set; }

    string _savePath;
    bool _isLarvaAlreadyCollected;
    #endregion

    #region Starts & Updates
    private void Awake()
    {
        _savePath = Application.persistentDataPath + "/gamesave.save";
        currentSave = GetSave();

        // Set the start position of player and cam
        if (currentSave != null && SceneController.GetActiveScene() != 0 && !currentSave.isNewLevel)
        {
            Camera.main.GetComponent<Cam>().Initialize(currentSave.xCamPos, currentSave.yCamPos, currentSave.xOffset, currentSave.yOffset, currentSave.orthographicSize, currentSave.leftBound, currentSave.rightBound, currentSave.topBound, currentSave.bottomBound);
            GameObject player = GameObject.Find("Player");
            if (player != null)
                player.transform.position = new Vector2(currentSave.xStartPos, currentSave.yStartPos);
        }
    }

    private void Start()
    {
        // Find the collectible and change sprite if already collected
        _collectible = GameObject.Find("Collectible");
        if (_collectible != null && currentSave.collectedLarva[SceneController.GetActiveScene()])
        {
            _collectible.GetComponent<SpriteRenderer>().color = Color.black;
            _isLarvaAlreadyCollected = true;
        }
    }
    #endregion

    #region Functions
    public void NewGame()
    {
        File.Delete(_savePath);
        loadSceneEvent.Raise(1);
    }

    public void LoadSave() => loadSceneEvent.Raise(currentSave.currentLevel);

    public void SaveLevel(int sceneIndex)
    {
        // Don't save if at main menu
        if (sceneIndex == 0)
            return;

        // Create a new save file if don't exist
        if (!File.Exists(_savePath))
            currentSave = new Save(sceneIndex, SceneController.GetCountScene());
        // Else rewrite only the level index
        else if (sceneIndex != currentSave.currentLevel)
        {
            currentSave.currentLevel = sceneIndex;
            currentSave.isNewLevel = true;
        }
        else
            return;

        SaveData(currentSave);
    }

    public void SaveCheckpoint() => StartCoroutine(SaveCheckpointCoroutine());

    public void SavePosition(Vector2 pos)
    {
        if (!File.Exists(_savePath))
            return;

        currentSave.xStartPos = pos.x;
        currentSave.yStartPos = pos.y;

        currentSave.isNewLevel = false;
    }

    public void SaveCamPosition(Vector2 pos)
    {
        if (!File.Exists(_savePath))
            return;

        currentSave.xCamPos = pos.x;
        currentSave.yCamPos = pos.y;
    }

    public void SaveOffset(Vector2 pos)
    {
        if (!File.Exists(_savePath))
            return;

        currentSave.xOffset = pos.x;
        currentSave.yOffset = pos.y;
    }

    public void SaveOrthographicSize(float orthographicSize)
    {
        if (!File.Exists(_savePath))
            return;

        currentSave.orthographicSize = orthographicSize;
    }

    public void SaveBoundaries(float left, float right, float top, float bot)
    {
        if (!File.Exists(_savePath))
            return;

        currentSave.leftBound = left;
        currentSave.rightBound = right;
        currentSave.topBound = top;
        currentSave.bottomBound = bot;
    }

    public void SaveLarva()
    {
        if (_isLarvaAlreadyCollected || !File.Exists(_savePath))
            return;

        // Set the larva of the current level to collected state
        if (!currentSave.collectedLarva[SceneController.GetActiveScene()])
            currentSave.collectedLarva[SceneController.GetActiveScene()] = true;
        else
            _isLarvaAlreadyCollected = true;

        // Add 1 to the total amount of larva if the larva is not already collected
        if (!_isLarvaAlreadyCollected)
            currentSave.totalLarva++;

        SaveData(currentSave);
    }

    void SaveData(Save save)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(_savePath);

        bf.Serialize(file, save);
        file.Close();
    }

    Save GetSave()
    {
        if (!File.Exists(Application.persistentDataPath + "/gamesave.save"))
            return null;

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.OpenRead(_savePath);
        Save save = (Save)bf.Deserialize(file);
        file.Close();

        return save;
    }

    public static bool SaveExist() => File.Exists(Application.persistentDataPath + "/gamesave.save");
    #endregion

    #region Coroutines
    IEnumerator SaveCheckpointCoroutine()
    {
        yield return null;
        SaveData(currentSave);
    }
    #endregion
}
