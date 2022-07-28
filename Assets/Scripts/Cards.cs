using UnityEngine;

/// <summary>
/// Clase que contiene las variables de cada carta.
/// </summary>
[CreateAssetMenu(menuName = "Card Object")]
public class Cards : ScriptableObject
{
    /// <summary>
    /// Valor de la carta.
    /// </summary>
    [SerializeField] int value;
    /// <summary>
    /// Sprite con el dibujo de la carta.
    /// </summary>
    [SerializeField] Sprite image;

    /// <summary>
    /// Función que devuelve el valor de la carta.
    /// </summary>
    /// <returns>El valor de la carta.</returns>
    public int GetValue()
    {
        return value;
    }

    /// <summary>
    /// Función que devuelve el sprite de la carta.
    /// </summary>
    /// <returns>El sprite de la carta.</returns>
    public Sprite GetSprite()
    {
        return image;
    }
}