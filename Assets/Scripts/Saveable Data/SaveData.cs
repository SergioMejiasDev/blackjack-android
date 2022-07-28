using System;

/// <summary>
/// Clase con todas las posibles variables de las opciones que pueden ser guardadas.
/// </summary>
[Serializable]
public class SaveData
{
    public string activeLanguage;
    public bool muteVolume;
    public int gamesPlayed;
    public int gamesWon;
    public int gamesLost;
    public int gamesDraw;
}