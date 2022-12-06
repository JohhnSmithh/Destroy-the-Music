using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Song
{
    private string id;
    private string name;
    private string album;
    private string artist;
    private bool explicitness;
    private double danceability;

    //Constructor
    public Song(string id, string name, string album, string artist, bool explicitness, double danceability)
    {
        this.id = id;
        this.name = name;
        this.album = album;
        this.artist = artist;
        this.explicitness = explicitness;
        this.danceability = danceability;
    }

    //Default Constructor
    public Song()
    {
        this.id = "";
        this.name = "";
        this.album = "";
        this.artist = "";
        this.explicitness = false;
        this.danceability = 0;
    }

    //Getters
    public string getID()
    {
        return this.id;
    }
    public string getName()
    {
        return this.name;
    }
    public string getAlbum()
    {
        return this.album;
    }
    public string getArtist()
    {
        return this.artist;
    }
    public bool isExplicit()
    {
        return explicitness;
    }
    public double getDanceability()
    {
        return danceability;
    }
}
