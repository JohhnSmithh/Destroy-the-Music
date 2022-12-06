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
        hashMap.Initialize();
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

    //Debug if it really empty
    public void IsEmpty()
    {
        Debug.Log(hashMap.Empty());
    }
}
