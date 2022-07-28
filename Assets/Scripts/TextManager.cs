using UnityEngine;
using TMPro;

/// <summary>
/// Clase activa en cada texto del juego y que se encarga de modificarlo según el idioma activo.
/// </summary>
public class TextManager : MonoBehaviour
{
    [SerializeField] MultilanguageText multilanguageText = null;

    private void OnEnable()
    {
        ChangeLanguage(SaveManager.activeLanguage);
    }

    private void Start()
    {
        ChangeLanguage(SaveManager.activeLanguage);
    }

    /// <summary>
    /// Función que modifica el texto de acuerdo con el idioma activo.
    /// </summary>
    /// <param name="newLanguage">El idioma que queremos activar.</param>
    void ChangeLanguage(string newLanguage)
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        switch (newLanguage)
        {
            case "EN":
                text.text = multilanguageText.english;
                break;
            case "ES":
                text.text = multilanguageText.spanish;
                break;
        }
    }
}