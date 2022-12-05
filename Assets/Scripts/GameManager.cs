using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // instance
    public static GameManager instance;

    // save data
    [System.Serializable]
    private class SaveData
    {
        public bool isLinear;
    }
    private SaveData data;

    // Audio
    [SerializeField] private AudioSource voiceAudioSource;
    [SerializeField] private AudioClip voice1;
    [SerializeField] private AudioClip voice2;
    [SerializeField] private AudioClip voice3;
    [SerializeField] private AudioClip voice4;
    [SerializeField] private AudioClip voice5;

    #region UNITY FUNCTIONS

    public void Awake() // called each time a scene is laoded/reloaded
    {
        // set up GameManager singleton class
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);

            data = new SaveData();
            string path = Application.persistentDataPath + "/savedata.json";
            if (File.Exists(path))
            {
                // read json file into data object
                string json = File.ReadAllText(path);
                data = JsonUtility.FromJson<SaveData>(json);
            }
            else // default save file configuration
            {
                // initialize save data to default values (used when no save data file is found)
                data.isLinear = true;
            }
        }
        else
        {
            Destroy(gameObject); // destroy duplicate instance of singleton object
        }
    }

    // Start is called before the first frame update
    void Start() // called only once at program boot-up (since duplicate GameManagers are destroyed in Awake())
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnApplicationQuit()
    {
        // save SaveData to json file
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    #endregion

    #region DATA FUNCTIONS

    public bool getIsLinear()
    {
        return data.isLinear;
    }

    public void setIsLinear(bool state)
    {
        data.isLinear = state;
    }

    #endregion

    #region SCENE MANAGEMENT
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        LoadScene("GameScene");
    }

    public void LoadInfoScene()
    {
        LoadScene("InfoScene");
    }

    public void LoadMenuScene()
    {
        LoadScene("MenuScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    #endregion

    #region AUDIO FUNCTIONS

    public void PlayRandomVoiceAudio()
    {
        if(!voiceAudioSource.isPlaying)
        {
            int rand = Random.Range(0, 5);
            switch (rand)
            {
                case 0:
                    voiceAudioSource.PlayOneShot(voice1);
                    break;
                case 1:
                    voiceAudioSource.PlayOneShot(voice2);
                    break;
                case 2:
                    voiceAudioSource.PlayOneShot(voice3);
                    break;
                case 3:
                    voiceAudioSource.PlayOneShot(voice4);
                    break;
                case 4:
                    voiceAudioSource.PlayOneShot(voice5);
                    break;
            }
        }
    }

    #endregion
}
