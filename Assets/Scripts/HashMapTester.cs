using System.Collections.Generic;
using UnityEngine;

public class HashMapTester : MonoBehaviour
{
    //HashMap baby
    private HashMap hashMap;

    // Start is called before the first frame update
    void Start()
    {
        hashMap = new();
        hashMap.Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Prints it
    public void PrintHashMap()
    {
        hashMap.DebugPrint();
    }

    //Add 1 Element
    public void AddOne()
    {
        /*List<Song> list = new List<Song>();
        list.Add(new Song("0001Lyv0YTjkZSqzT4WkLy", "Eye of the Hurricane", "Fire in the Sky", "Half Japanese", false, 0.49));
        list.Add(new Song("0001Wtl60puR26ZtSDIF66", "Shikestei Fars", "Secret Museum Of Mankind Vol. 5: Ethnic Music Classics 1925-48", "Khan Sushinksky", false, 0.29600000000000004));
        list.Add(new Song("0006YMdmPphDXTlUBHGr6Q", "Bent Finger Boogie", "Puzzle", "Matt Gray", false, 0.499));
        list.Add(new Song("0007ViJ9W2YqgQX7zDic82", "Ceremony II (\"Incantations\")", "From Behind the Unreasoning Mask", "Paul Chihara", false, 0.139));

        int rand = Random.Range(0, 4 );
        hashMap.Add(list[rand].getID(), list[rand]);

        Debug.Log(list[rand].getName() + " was added!");*/

        hashMap.AddX(2);
        
    }

    //Remove 1 Element
    public void RemoveOne()
    {
        hashMap.DebugNextPrime(7);
    }
}
