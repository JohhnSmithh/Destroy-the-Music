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

    //Current elements
    private int currSize = 0;

    //Data stuff
    private string[] dataLines;
    private int currLine = 1;
    private int removeLine = 1;

    //Probing Type


    //Debug Vars
    private int skipped = 0;
    private int graves = 0;


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
        //Num added
        int added = 0;

        //Add 'em
        while(added < x && currLine < dataLines.Length)
        {
            string[] elements = dataLines[currLine].Split(',');

            //Only add if valid line
            if (elements.Length == 24)
            {
                //Find if it's explicit
                bool ex = false;
                if (elements[8] == "True")
                    ex = true;

                //Add the file elements to the hash map
                Add(elements[0], new Song(elements[0], elements[1], elements[2], elements[4], ex, Double.Parse(elements[9])));
                added++;
            }

            //Increased skipped counter
            else
                skipped++;

            //Go to the next line
            currLine++;
        }

        //Display number of elements added
        Debug.Log("Added " + added + " Songs");
    }

    //Remove x elements to the hashtable
    public void RemoveX(int x)
    {
        //Num removed
        int removed = 0;

        //Remove
        while(removed < x && currSize != 0)
        {
            string[] elements = dataLines[removeLine].Split(',');

            //Only remove if valid line
            if(elements.Length == 24)
            {
                Remove(elements[0]);
                removed++;
            }

            //Go to next line
            removeLine++;
        }

        //Display number of elements removed
        Debug.Log("Removed " + removed + " Songs"); 
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
        if (((double)currSize + (double)graves)/ (double)maxSize >= maxLoadFactor)
            Resize();

    }

    //Remove a specific key
    public void Remove(string key)
    {
        int idx = FindKeyPosLinear(key);

        //Do stuff if it was found
        if (idx != -1)
            Remove(idx);
        else
            Debug.Log("Key " + key + " Not Found");

    }

    //Remove a specific indx
    public void Remove(int idx)
    {
        if (idx < maxSize && hashTable[idx] != null)
        {
            hashTable[idx] = new Song();
            graves++;
            currSize--;
        }
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
        graves = 0;

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
        Debug.Log("Current Size is " + currSize + ", Max Size is " + maxSize + ", Number of Songs Skipped is " + skipped + ", Number of Gravestones is " + graves);
        Debug.Log("Current Line is " + currLine + ", Current Removal Line is " + removeLine);
    }

    //Prints the next prime
    public void DebugNextPrime(int n)
    {
        Debug.Log("Next prime after " + n + " is " + NextPrime(n));
    }

    //Returns whether there are any actual elements in the array or not, ignoring gravestones
    public bool Empty()
    {
        return currSize == 0;
    }
    #endregion
}
