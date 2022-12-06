using System.Collections.Generic;
using UnityEngine;

public class HashMapTester : MonoBehaviour
{
    //HashMap baby
    private HashMap hashMap;

    //Values for how many to add and remove
    [SerializeField] int addAmt = 50000;
    [SerializeField] int removeAmt = 100;

    // Start is called before the first frame update
    void Start()
    {
        hashMap = new();
        hashMap.Initialize("quadratic");
    }

    //Prints it
    public void PrintHashMap()
    {
        hashMap.DebugPrint();
    }

    //Add 1 Element
    public void AddSome()
    {
        hashMap.AddX(addAmt);
    }

    //Remove 1 Element
    public void RemoveSome()
    {
        hashMap.RemoveX(removeAmt);
    }

    //Debug Prime
    public void PrintNextPrime()
    {
        hashMap.DebugNextPrime(Random.Range(1,200000));
    }

    //Gets a random song from the hash map
    public void GetSong()
    {
        Debug.Log(hashMap.GetRandomSong().getName());
    }

    //Gets a random 4 songs from the hash map
    public void GetSongs()
    {
        List<Song> songs = hashMap.GetRandomSongs();
        Debug.Log(songs[0].getName() + ", " + songs[1].getName() + ", " + songs[2].getName() + ", " + songs[3].getName());
        Debug.Log(songs[0].getArtist() + ", " + songs[1].getArtist() + ", " + songs[2].getArtist() + ", " + songs[3].getArtist());
        Debug.Log(songs[0].getAlbum() + ", " + songs[1].getAlbum() + ", " + songs[2].getAlbum() + ", " + songs[3].getAlbum());
    }

    //Prints the entire hash map
    public void PrintAll()
    {
        hashMap.PrintAll();
    }

    //Changes the probe method
    public void SetProbeMethod(string probeMethod)
    {
        hashMap.SetProbeMethod(probeMethod);
    }
}
