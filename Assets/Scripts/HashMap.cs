using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
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
    string probeMethod = "quadratic";

    //Debug Vars
    private int skipped = 0;
    private int graves = 0;


    //Initialize the HashMap
    public void Initialize(string probeMethod)
    {
        hashTable = new Song[maxSize];

        //Read the data for later acces
        dataLines = File.ReadAllLines("Assets/DataSet/tracks_features.csv");

        //Add the initial 100000
        AddX(100000);

        //Set the probing method
        this.probeMethod = probeMethod;

    }

    //Add x elements to the hashtable from the dataset
    public void AddX(int x)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

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

                //Get rid of the extra characters around the artist name
                string artist = elements[4].Substring(2, elements[4].Length - 4);

                //Add the file elements to the hash map
                Add(elements[0], new Song(elements[0], elements[1], elements[2], artist, ex, Double.Parse(elements[9])));
                added++;
            }

            //Increased skipped counter
            else
                skipped++;

            //Go to the next line
            currLine++;
        }

        watch.Stop();

        //Display number of elements added
        Debug.Log("Added " + added + " Songs, Execution Time: " + watch.ElapsedMilliseconds + "ms, Average Time per Insertion: " + watch.ElapsedMilliseconds / (double)added + "ms");
    }

    //Remove x elements to the hashtable
    public void RemoveX(int x)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();

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

        watch.Stop();

        //Display number of elements removed
        Debug.Log("Removed " + removed + " Songs, Execution Time: " + watch.ElapsedMilliseconds + "ms, Average Time per Deletion: " + watch.ElapsedMilliseconds / (double)removed + "ms");
    }

    //Add a specific item
    public void Add(string key, Song song)
    {
        //Find the position to insert
        int pos = 0;
        if (this.probeMethod == "linear") pos = FindAddPosLinear(key);
        else pos = FindAddPosQuadratic(key);

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
        int idx = 0;
        if (this.probeMethod == "linear") idx = FindKeyPosLinear(key);
        else idx = FindKeyPosQuadratic(key);

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

    //Gets 4 songs
    public List<Song> GetRandomSongs()
    {
        List<Song> songs = new List<Song>();
        int idx = UnityEngine.Random.Range(0, maxSize);

        for (int i = 0; i < 4; i++)
        {
            //Find the next index that contains a value
            while (hashTable[idx] == null || hashTable[idx].getID() == "")
            {
                idx++;

                //Don't go too far
                if (idx >= maxSize)
                    idx -= maxSize;
            }

            //Add it to the list then increase idx so that we don't get 4 of the same song
            songs.Add(hashTable[idx]);
            idx++;
        }

        return songs;
    }


    //Gets a song
    public Song GetRandomSong()
    {
        int idx = UnityEngine.Random.Range(0, maxSize);

        //Find the next index that contains a value
        while (hashTable[idx] == null || hashTable[idx].getID() == "")
        {
            idx++;

            //Don't go too far
            if (idx >= maxSize)
                idx -= maxSize;
        }
        
        return hashTable[idx];
    }


    #region PRIVATE HELPER FUNCTIONS

    //Find position for a key linearly (for insertion)
    private int FindAddPosLinear(string key)
    {
        int pos = Hash(key);

        //Go through the array to locate the position
        while (hashTable[pos] != null && hashTable[pos].getID() != key && hashTable[pos].getID() != "")
        {
            pos++;

            //Check if we exceeded the size
            if (pos >= maxSize)
                pos -= maxSize;
        }

        return pos;
    }

    //Find position for a key quadratically (for insertion)
    private int FindAddPosQuadratic(string key)
    {
        int pos = Hash(key);
        int collisions = 0;

        //Go through the array to locate the position
        while (hashTable[pos] != null && hashTable[pos].getID() != key && hashTable[pos].getID() != "")
        {
            collisions++;
            pos += collisions*collisions;

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
        while(hashTable[pos] != null && hashTable[pos].getID() != key)
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

    //Find position for a key quadratically (for deletion and search)
    //Returns -1 if we couldn't find it
    private int FindKeyPosQuadratic(string key)
    {
        int pos = Hash(key);
        int collisions = 0;

        //Go through array until we find it (or until a null)
        while (hashTable[pos] != null && hashTable[pos].getID() != key)
        {
            collisions++;
            pos += collisions * collisions;

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
        Debug.Log("Probing Method is " + probeMethod + ", Current Load Factor is " + (((double)currSize + (double)graves)/ (double)maxSize) + ", Max Load Factor is " + maxLoadFactor);
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

    //Prints the entire hash table
    public void PrintAll()
    {
        for(int i = 0; i < maxSize; i++)
        {
            if (hashTable[i] == null) Debug.Log(i + ": Empty spot");
            else if (hashTable[i].getID() == "") Debug.Log(i + ": Gravestone");
            else Debug.Log(i + ": " + Hash(hashTable[i].getID()) + ", " + hashTable[i].getName() + ", " + hashTable[i].getAlbum() + ", " + hashTable[i].getArtist() + ", " + hashTable[i].isExplicit() + ", " + hashTable[i].getDanceability());
        }
    }

    //Sets the probe method (never going to be used in the actual game)
    public void SetProbeMethod(string probeMethod)
    {
        this.probeMethod = probeMethod;
    }
    #endregion
}
