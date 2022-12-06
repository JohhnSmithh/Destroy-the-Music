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
    public void AddSome()
    {
        hashMap.AddX(50000);
    }

    //Remove 1 Element
    public void RemoveSome()
    {
        hashMap.Remove("1LP5MfZ4SY8n61NtJ6joBP");
        hashMap.Remove("21dGdGBsiCHMWgjAk9dZTQ");
    }

    //Debug Prime
    public void PrintNextPrime()
    {
        hashMap.DebugNextPrime(Random.Range(1,200000));
    }
}
