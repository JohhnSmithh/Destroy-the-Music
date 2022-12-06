using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    // constants
    private float MAX_MUSIC_VOLUME = 0.5f;

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
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioClip music;

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
        // make music loop
        if (!musicAudioSource.isPlaying)
            musicAudioSource.PlayOneShot(music);

        // set volume back to max in main menu
        if(SceneManager.GetActiveScene().name == "MenuScene")
            musicAudioSource.volume = MAX_MUSIC_VOLUME;
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

    public bool IsTalking()
    {
        return voiceAudioSource.isPlaying;
    }

    // input volume is on a scale of 0 to 1, which is remapped to the scale being used
    public void setMusicVolume(float volume)
    {
        musicAudioSource.volume = mapRange(volume, 0, 1, 0, MAX_MUSIC_VOLUME);
    }

    // maps one range to another; see https://forum.unity.com/threads/re-map-a-number-from-one-range-to-another.119437/
    private float mapRange(float val, float from1, float to1, float from2, float to2)
    {
        return (val - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    #endregion
}
