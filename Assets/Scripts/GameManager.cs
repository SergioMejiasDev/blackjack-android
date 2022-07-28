using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Clase que controla las funciones principales del juego durante los menús.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager manager;

    /// <summary>
    /// El panel principal del menú.
    /// </summary>
    [Header("Panels")]
    [SerializeField] GameObject panelMenu = null;
    /// <summary>
    /// El panel de juego.
    /// </summary>
    [SerializeField] GameObject panelGame = null;
    /// <summary>
    /// El panel con las estadísticas.
    /// </summary>
    [SerializeField] GameObject panelStats = null;
    /// <summary>
    /// El panel con las instrucciones.
    /// </summary>
    [SerializeField] GameObject panelInstructions = null;
    /// <summary>
    /// El panel con los créditos.
    /// </summary>
    [SerializeField] GameObject panelCredits = null;

    /// <summary>
    /// La clase con las funciones que controlan la partida de Blackjack.
    /// </summary>
    [Header("Cards")]
    [SerializeField] Blackjack gameClass = null;
    /// <summary>
    /// Todas las posibles cartas de la baraja.
    /// </summary>
    [SerializeField] Cards[] cardsList = null;

    /// <summary>
    /// La imagen con el altavoz que indica si está activado el volumen.
    /// </summary>
    [Header("Volume")]
    [SerializeField] Image volumeImage = null;
    /// <summary>
    /// El sprite que indica que el volumen está activado.
    /// </summary>
    [SerializeField] Sprite volumeOn = null;
    /// <summary>
    /// El sprite que indica que el volumen está desactivado.
    /// </summary>
    [SerializeField] Sprite volumeOff = null;

    /// <summary>
    /// Texto donde se indican las partidas jugadas.
    /// </summary>
    [Header("Stats")]
    [SerializeField] TextMeshProUGUI gamesPlayedText = null;
    /// <summary>
    /// Texto donde se indican las partidas ganadas.
    /// </summary>
    [SerializeField] TextMeshProUGUI gamesWonText = null;
    /// <summary>
    /// Texto donde se indican las partidas perdidas.
    /// </summary>
    [SerializeField] TextMeshProUGUI gamesLostText = null;
    /// <summary>
    /// Texto donde se indican las partidas empatadas.
    /// </summary>
    [SerializeField] TextMeshProUGUI gamesDrawText = null;

    /// <summary>
    /// La imagen que mostrará la bandera correspondiente al idioma activo.
    /// </summary>
    [Header("Language")]
    [SerializeField] Image flag = null;
    /// <summary>
    /// El sprite de la bandera inglesa.
    /// </summary>
    [SerializeField] Sprite flagEN = null;
    /// <summary>
    /// El sprite de la bandera española.
    /// </summary>
    [SerializeField] Sprite flagES = null;

    void Awake()
    {
        manager = this;

        LetterBoxer.AddLetterBoxingCamera();

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        SaveManager.LoadOptions();

        CheckVolume();

        SelectFlag();
    }

    /// <summary>
    /// Función que inicia la partida.
    /// </summary>
    public void StartGame()
    {
        panelMenu.SetActive(false);
        panelGame.SetActive(true);

        gameClass.ResetGame();
    }

    /// <summary>
    /// Función que cierra la partida y vuelve al menú.
    /// </summary>
    public void CloseGame()
    {
        panelMenu.SetActive(true);
        panelGame.SetActive(false);

        GameObject[] cardsInScreen = GameObject.FindGameObjectsWithTag("Card");

        if (cardsInScreen != null)
        {
            for (int i = 0; i < cardsInScreen.Length; i++)
            {
                Destroy(cardsInScreen[i]);
            }
        }
    }

    /// <summary>
    /// Función que abre y cierra el panel de estadísticas.
    /// </summary>
    public void OpenStats()
    {
        if (panelStats.activeSelf)
        {
            panelStats.SetActive(false);
        }

        else
        {
            UpdateStats();

            panelStats.SetActive(true);
        }
    }

    /// <summary>
    /// Función que abre y cierra el panel de instrucciones.
    /// </summary>
    public void OpenInstructions()
    {
        if (panelInstructions.activeSelf)
        {
            panelInstructions.SetActive(false);
        }

        else
        {
            panelInstructions.SetActive(true);
        }
    }

    /// <summary>
    /// Función que abre y cierra el panel de los créditos.
    /// </summary>
    public void OpenCredits()
    {
        if (panelCredits.activeSelf)
        {
            panelCredits.SetActive(false);
        }

        else
        {
            panelCredits.SetActive(true);
        }
    }

    /// <summary>
    /// Función que abre links externos.
    /// </summary>
    /// <param name="link">La referencia del link que queremos abrir.</param>
    public void OpenLink(int link)
    {
        switch (link)
        {
            case 1:
                Application.OpenURL("https://play.google.com/store/apps/developer?id=Sergio+Mejias");
                break;
            case 2:
                Application.OpenURL("https://freesound.org/people/f4ngy/");
                break;
            case 3:
                Application.OpenURL("https://gitlab.com/SergioMejiasDev/blackjack-android");
                break;
        }
    }

    /// <summary>
    /// Función que actualiza el panel de estadísticas.
    /// </summary>
    void UpdateStats()
    {
        gamesPlayedText.text = SaveManager.gamesPlayed.ToString();
        gamesWonText.text = SaveManager.gamesWon.ToString();
        gamesLostText.text = SaveManager.gamesLost.ToString();
        gamesDrawText.text = SaveManager.gamesDraw.ToString();
    }

    /// <summary>
    /// Función que devuelve la lista completa de cartas.
    /// </summary>
    /// <returns>La lista completa de cartas.</returns>
    public Cards[] GetCardsList()
    {
        return cardsList;
    }

    /// <summary>
    /// Función que activa y desactiva el volumen de acuerdo con la configuración establecida.
    /// </summary>
    void CheckVolume()
    {
        if (!SaveManager.muteVolume)
        {
            AudioListener.volume = 1f;

            volumeImage.sprite = volumeOn;
        }

        else
        {
            AudioListener.volume = 0f;

            volumeImage.sprite = volumeOff;
        }
    }

    /// <summary>
    /// Función que activa y desactiva el sonido.
    /// </summary>
    public void ManageVolume()
    {
        if (SaveManager.muteVolume)
        {
            AudioListener.volume = 1f;

            volumeImage.sprite = volumeOn;

            SaveManager.muteVolume = false;
        }

        else
        {
            AudioListener.volume = 0f;

            volumeImage.sprite = volumeOff;

            SaveManager.muteVolume = true;
        }

        SaveManager.SaveOptions();
    }

    /// <summary>
    /// Función encargada de cambiar el idioma de los textos en el juego.
    /// </summary>
    /// <param name="newLanguage">El idioma que queremos activar.</param>
    void ChangeLanguage(string newLanguage)
    {
        SaveManager.activeLanguage = newLanguage;
        SaveManager.SaveOptions();

        SelectFlag();
    }

    /// <summary>
    /// Función que alterna el idioma activo.
    /// </summary>
    public void AlternateLanguage()
    {
        switch (SaveManager.activeLanguage)
        {
            case "EN":
                ChangeLanguage("ES");
                break;
            case "ES":
                ChangeLanguage("EN");
                break;
        }

        panelMenu.SetActive(false);
        panelMenu.SetActive(true);
    }

    /// <summary>
    /// Función que actualiza la imagen de la bandera de acuerdo con el idioma activo.
    /// </summary>
    void SelectFlag()
    {
        switch (SaveManager.activeLanguage)
        {
            case "EN":
                flag.sprite = flagEN;
                break;
            case "ES":
                flag.sprite = flagES;
                break;
        }
    }

    /// <summary>
    /// Función a la que se llama para cerrar la aplicación.
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}