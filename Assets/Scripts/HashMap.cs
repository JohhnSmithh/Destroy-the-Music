using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HashMap
{
    //Starting size
    private int maxSize = 3;

    //Max Load Factor
    private double maxLoadFactor = 0.5;

    //Hash Tabel array
    private Song[] hashTable;

    //Load factor, number of current elements, and the like
    private int currSize = 0;


    //Initialize the HashMap
    public void Initialize()
    {
        hashTable = new Song[maxSize];
    }

    //Add x elements to the hashtable from the dataset
    public void AddX(int x)
    {
        //Read from file
        string[] lines = File.ReadLines("Assets/DataSet/tracks_features.csv").Take(x).ToArray();

        foreach (string line in lines)
            Debug.Log(line);

    }

    //Remove x elements to the hashtable
    public void RemoveX(int x)
    {

    }

    //Add a specific item
    public void Add(string key, Song song)
    {
        //Find the position to insert
        int pos = FindPositionLinear(key);

        //Increase the size only if its a new entry
        if(hashTable[pos] == null)
            currSize++;

        //Add the song in the appropriate location
        hashTable[pos] = song;

        //If size is too big, resize
        if ((double)currSize / (double)maxSize >= maxLoadFactor)
            Resize();

    }

    //Remove a specific item
    public void Remove(string key)
    {

    }

    /*==== PRIVATE HELPER FUNCTIONS ====*/

    //Find position for a key linearly
    private int FindPositionLinear(string key)
    {
        int pos = Hash(key);

        //Go through the array to locate the position
        while (hashTable[pos] != null && hashTable[pos].getID() != key)
        {
            pos++;

            //Check if we exceeded the size
            if (pos >= maxSize)
                pos -= maxSize;
        }

        return pos;
    }

    //Hash Function
    private int Hash(string key)
    {
        return Math.Abs(key.GetHashCode() % maxSize);
    }

    //Resizes the array
    private void Resize()
    {
        //Need to resize to a prime number
        maxSize = NextPrime(maxSize * 2);

        //Store current elements in a temp array and create a new, larger array
        Song[] tempArray = (Song[]) hashTable.Clone();
        hashTable = new Song[maxSize];
        currSize = 0;

        //Rehash / add everything
        for(int i = 0; i < tempArray.Length; i++)
        {
            //If there be a value, add it to the new array
            if (tempArray[i] != null)
                Add(tempArray[i].getID(), tempArray[i]);
        }
    }

    //Finds next prime number
    private int NextPrime(int n)
    {
        int max = n * 2;
        while (n < max)
        {
            bool prime = true;

            //Iterate forward until a prime is found
            n++;

            //Check the factors
            for (int i = 2; i <= (int)Math.Sqrt(n); i++)
            {
                //Not prime, proceed to next number
                if (n % i == 0)
                {
                    prime = false;
                    break;
                }
            }

            //We checked the factors, if its good then we found the prime
            if (prime)
                return n;

        }

        return -1;
    }

    /*==== DEBUG FUNCTIONS ====*/

    //Prints the array
    public void DebugPrint()
    {
        Debug.Log("Current Size is " + currSize + " Max Size is " + maxSize);

        for(int i = 0; i < hashTable.Length; i++)
        {
            if (hashTable[i] == null)
                Debug.Log("Yeah " + i + " be null");
            else
                Debug.Log(i + " " + hashTable[i].getID() + " " + hashTable[i].getName());
        }
        
    }

    //Prints the next prime
    public void DebugNextPrime(int n)
    {
        Debug.Log("Next prime after " + n + " is " + NextPrime(n));
    }
}
