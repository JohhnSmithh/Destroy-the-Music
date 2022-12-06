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
    private int maxSize = 100003;

    //Max Load Factor
    private double maxLoadFactor = 0.5;

    //Hash Tabel array
    private Song[] hashTable;

    //Load factor, number of current elements, and the like
    private int currSize = 0;

    //Data stuff
    string[] dataLines;
    int currLine = 1;

    //Probing Type


    //Debug Vars
    int skipped = 0;


    //Initialize the HashMap
    public void Initialize()
    {
        hashTable = new Song[maxSize];

        //Read the data for later acces
        dataLines = File.ReadAllLines("Assets/DataSet/tracks_features.csv");

        //Add the initial 100000
        //AddX(100000);

    }

    //Add x elements to the hashtable from the dataset
    public void AddX(int x)
    {
        for (int i = currLine; i < currLine + x; i++)
        {
            string[] elements = dataLines[i].Split(',');
            //Debug.Log("Length: " + elements.Length + " " + elements[0] + " " + elements[1] + " " + elements[2] + " " + elements[4] + " " + elements[8] + " " + elements[9]);

            //Only do stuff if the lines are valid when delimited
            if (elements.Length == 24)
            {
                //Find if it's explicit
                bool ex = false;
                if (elements[8] == "True")
                    ex = true;

                //Add the file elements to the hash map
                Add(elements[0], new Song(elements[0], elements[1], elements[2], elements[4], ex, Double.Parse(elements[9])));
            }
            else
                skipped++;
        }

        //Increase the current line so we don't read them twice
        currLine += x;

        //Debug Successful
        Debug.Log("Successfully Added " + x + " Elements");
    }

    //Remove x elements to the hashtable
    public void RemoveX(int x)
    {

    }

    //Add a specific item
    public void Add(string key, Song song)
    {
        //Find the position to insert
        int pos = FindAddPosLinear(key);

        //Increase the size only if its a new entry or gravestone
        if(hashTable[pos] == null || hashTable[pos].getID() == "")
            currSize++;

        //Add the song in the appropriate location
        hashTable[pos] = song;

        //If size is too big, resize
        if ((double)currSize / (double)maxSize >= maxLoadFactor)
            Resize();

    }

    //Remove a specific key
    public void Remove(string key)
    {
        int idx = FindKeyPosLinear(key);
        Debug.Log(idx + ": " + key + " " + hashTable[idx].getID() + " " + hashTable[idx].getName());
    }

    //Remove a specific indx
    public void Remove(int idx)
    {
        if(idx < maxSize && hashTable[idx] != null)
            hashTable[idx] = new Song();
    }


    #region PRIVATE HELPER FUNCTIONS

    //Find position for a key linearly (for insertion)
    private int FindAddPosLinear(string key)
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

    //Find position for a key linearly (for deletion and search)
    //Returns -1 if we couldn't find it
    private int FindKeyPosLinear(string key)
    {
        int pos = Hash(key);

        //Go through array until we find it (or until a null)
        while(hashTable[pos] != null && hashTable[pos].getID() != "" && hashTable[pos].getID() != key)
        {
            pos++;

            //Check if we exceeded the size
            if (pos >= maxSize)
                pos -= maxSize;
        }

        //Check if we couldn't find it
        if (hashTable[pos] == null)
            return -1;

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
            //If there be a value, add it to the new array (don't add the gravestones)
            if (tempArray[i] != null && tempArray[i].getID() != "")
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
    #endregion

    #region DEBUG FUNCTIONS

    //Prints the array
    public void DebugPrint()
    {
        Debug.Log("Current Size is " + currSize + ", Max Size is " + maxSize + ", Number of Songs Skipped is " + skipped); 
    }

    //Prints the next prime
    public void DebugNextPrime(int n)
    {
        Debug.Log("Next prime after " + n + " is " + NextPrime(n));
    }
    #endregion
}
