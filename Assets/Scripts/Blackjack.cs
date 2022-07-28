using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Clase que controla las diferentes funciones durante la partida de Blackjack.
/// </summary>
public class Blackjack : MonoBehaviour
{
    /// <summary>
    /// Lista con las cartas restantes de la baraja.
    /// </summary>
    [Header("Cards")]
    List<Cards> cardsList = new List<Cards>();
    /// <summary>
    /// El prefab de la carta.
    /// </summary>
    [SerializeField] GameObject card = null;
    /// <summary>
    /// Las cartas del jugador.
    /// </summary>
    List<GameObject> cardsPlayer = new List<GameObject>();
    /// <summary>
    /// Las cartas de la IA.
    /// </summary>
    List<GameObject> cardsAI = new List<GameObject>();

    /// <summary>
    /// La puntuación del jugador.
    /// </summary>
    [Header("Score")]
    int scorePlayer1;
    /// <summary>
    /// La puntuación alternativa del jugador (ha salido un as).
    /// </summary>
    int scorePlayer2;
    /// <summary>
    /// La puntuación final del jugador.
    /// </summary>
    int finalScorePlayer;
    /// <summary>
    /// La puntuación de la IA.
    /// </summary>
    int scoreAI1;
    /// <summary>
    /// La puntuación alternativa de la IA (ha salido un as).
    /// </summary>
    int scoreAI2;
    /// <summary>
    /// Panel con la puntuación del jugador.
    /// </summary>
    [SerializeField] TextMeshProUGUI scorePlayer = null;
    /// <summary>
    /// Panel con la puntuación de la IA.
    /// </summary>
    [SerializeField] TextMeshProUGUI scoreAI = null;

    /// <summary>
    /// Los posibles botones activos en la pantalla.
    /// </summary>
    [Header("Buttons")]
    [SerializeField] GameObject[] buttons = null;
    /// <summary>
    /// Los posibles mensajes que pueden aparecer durante la partida.
    /// </summary>
    [SerializeField] GameObject[] messages = null;

    /// <summary>
    /// Los posibles resultados de la partida.
    /// </summary>
    enum Result
    {
        /// <summary>
        /// El jugador gana la partida.
        /// </summary>
        Victory,
        /// <summary>
        /// El jugador pierde la partida.
        /// </summary>
        Lose,
        /// <summary>
        /// El jugador empata la partida.
        /// </summary>
        Draw
    }

    /// <summary>
    /// Corrutina que reparte las tres primeras cartas.
    /// </summary>
    /// <returns></returns>
    IEnumerator InitialSpawn()
    {
        StartCoroutine(InstantiateCardPlayer());

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(InstantiateCardAI());

        yield return new WaitForSeconds(0.75f);

        StartCoroutine(InstantiateCardPlayer());

        yield return new WaitForSeconds(0.75f);

        if (scorePlayer1 != 21)
        {
            EnableButtons(true);
        }

        else
        {
            EndGame(Result.Victory);
        }
    }

    /// <summary>
    /// Función que actualiza las puntuaciones en la pantalla.
    /// </summary>
    void WriteScore()
    {
        if (scorePlayer1 == scorePlayer2)
        {
            scorePlayer.text = scorePlayer1.ToString();
        }

        else if ((scorePlayer1 != scorePlayer2) && scorePlayer1 <= 21)
        {
            scorePlayer.text = scorePlayer1.ToString() + "/" + scorePlayer2.ToString();
        }

        else if ((scorePlayer1 != scorePlayer2) && scorePlayer1 > 21)
        {
            scorePlayer.text = scorePlayer2.ToString();
        }

        if (scoreAI1 == scoreAI2)
        {
            scoreAI.text = scoreAI1.ToString();
        }

        else if ((scoreAI1 != scoreAI2) && scoreAI1 <= 21)
        {
            scoreAI.text = scoreAI1.ToString() + "/" + scoreAI2.ToString();
        }

        else if ((scoreAI1 != scoreAI2) && scoreAI1 > 21)
        {
            scoreAI.text = scoreAI2.ToString();
        }
    }

    /// <summary>
    /// Función que comprueba si el jugador ha perdido la partida durante su turno.
    /// </summary>
    /// <returns>Verdadero si no ha perdido, falso si ha perdido.</returns>
    bool CheckFirstVictory()
    {
        if ((scorePlayer1 > 21 && scorePlayer2 == 0) || scorePlayer2 > 21)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Función que comprueba si la partida ha terminado después del turno de la IA.
    /// </summary>
    void CheckLastVictory()
    {
        if (scoreAI1 > finalScorePlayer && scoreAI1 <= 21)
        {
            EndGame(Result.Lose);
        }

        else if (scoreAI2 > finalScorePlayer && scoreAI2 <= 21 && scoreAI1 > 21)
        {
            EndGame(Result.Lose);
        }

        else if (scoreAI1 > 21 && scoreAI2 > 21)
        {
            EndGame(Result.Victory);
        }

        else if ((scoreAI1 == finalScorePlayer && scoreAI1 >= 17) || (scoreAI2 == finalScorePlayer && scoreAI2 >= 17))
        {
            EndGame(Result.Draw);
        }

        else
        {
            NewCardAI();
        }
    }

    /// <summary>
    /// Función que se activa al pulsar el botón "Plantarse".
    /// </summary>
    public void StandUp()
    {
        EnableButtons(false);

        if (!CheckFirstVictory())
        {
            EndGame(Result.Lose);

            return;
        }

        finalScorePlayer = scorePlayer1 <= 21 ? scorePlayer1 : scorePlayer2;

        NewCardAI();
    }

    /// <summary>
    /// Function that activates or deactivates the buttons on the screen.
    /// Función que activa y desactiva los botones de la pantalla.
    /// </summary>
    /// <param name="enable">Verdadero para activarlos, falso para desactivarlos.</param>
    public void EnableButtons(bool enable)
    {
        if (enable)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(true);
            buttons[2].SetActive(true);
        }

        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(false);
            buttons[2].SetActive(false);
        }
    }

    /// <summary>
    /// Función que termina la partida.
    /// </summary>
    /// <param name="result">El resultado de la partida.</param>
    void EndGame(Result result)
    {
        switch (result)
        {
            case Result.Victory:
                SaveManager.gamesWon += 1;
                messages[0].SetActive(true);
                buttons[2].SetActive(true);
                break;

            case Result.Lose:
                SaveManager.gamesLost += 1;
                messages[1].SetActive(true);
                buttons[2].SetActive(true);
                break;

            case Result.Draw:
                SaveManager.gamesDraw += 1;
                messages[2].SetActive(true);
                buttons[2].SetActive(true);
                break;
        }

        SaveManager.gamesPlayed += 1;
        SaveManager.SaveOptions();
    }

    /// <summary>
    /// Función que reinicia la partida.
    /// </summary>
    public void ResetGame()
    {
        EnableButtons(false);

        for (int i = 0; i < messages.Length; i++)
        {
            messages[i].SetActive(false);
        }

        GameObject[] cardsInScreen = GameObject.FindGameObjectsWithTag("Card");

        if (cardsInScreen != null)
        {
            for (int i = 0; i < cardsInScreen.Length; i++)
            {
                Destroy(cardsInScreen[i]);
            }
        }

        cardsPlayer.Clear();
        cardsAI.Clear();

        scoreAI1 = 0;
        scoreAI2 = 0;
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        finalScorePlayer = 0;

        WriteScore();

        cardsList.Clear();

        Cards[] tempCards = GameManager.manager.GetCardsList();
        
        for (int i = 0; i < tempCards.Length; i++)
        {
            cardsList.Add(tempCards[i]);
        }

        StartCoroutine(InitialSpawn());
    }

    #region Player

    /// <summary>
    /// Corrutina que reparte una nueva carta al jugador.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateCardPlayer()
    {
        ReorganizeCardsPlayer();

        int randomCard = Random.Range(0, cardsList.Count);
        GameObject newCard = Instantiate(card, SpawnPositionPlayer(), Quaternion.identity);
        cardsPlayer.Add(newCard);

        yield return new WaitForSeconds(0.5f);

        newCard.GetComponent<SpriteRenderer>().sprite = cardsList[randomCard].GetSprite();
        UpdateScorePlayer(cardsList[randomCard].GetValue());
        cardsList.Remove(cardsList[randomCard]);

        if (!CheckFirstVictory())
        {
            EndGame(Result.Lose);
            yield break;
        }

        if (cardsPlayer.Count > 2)
        {
            EnableButtons(true);
        }
    }

    /// <summary>
    /// Función que calcula la posición donde se va a colocar la nueva carta.
    /// </summary>
    /// <returns>Vector de posición donde se va a repartir la carta.</returns>
    Vector2 SpawnPositionPlayer()
    {
        int activeCards = cardsPlayer.Count;

        return new Vector2(activeCards * 0.75f, -3.48f);
    }

    /// <summary>
    /// Función que organiza las cartas del jugador en la pantalla cuando se reparte una nueva carta.
    /// </summary>
    void ReorganizeCardsPlayer()
    {
        int activeCards = cardsPlayer.Count;

        for (int i = 0; i < activeCards; i++)
        {
            cardsPlayer[i].transform.position = new Vector2(cardsPlayer[i].transform.position.x - 0.75f, -3.48f);
        }
    }

    /// <summary>
    /// Función activada para repartir una nueva carta al jugador.
    /// </summary>
    public void NewCard()
    {
        EnableButtons(false);

        StartCoroutine(InstantiateCardPlayer());
    }

    /// <summary>
    /// Función que actualiza la puntuación del jugador.
    /// </summary>
    /// <param name="score">La puntuación de la carta.</param>
    void UpdateScorePlayer(int score)
    {
        if (score != 1)
        {
            scorePlayer1 += score;
            scorePlayer2 += score;
        }

        else
        {
            scorePlayer1 += 11;
            scorePlayer2 += 1;
        }

        WriteScore();
    }

    #endregion

    #region AI

    /// <summary>
    /// Corrutina que reparte una nueva carta a la IA.
    /// </summary>
    /// <returns></returns>
    IEnumerator InstantiateCardAI()
    {
        ReorganizeCardsAI();

        int randomCard = Random.Range(0, cardsList.Count);
        GameObject newCard = Instantiate(card, SpawnPositionAI(), Quaternion.identity);
        cardsAI.Add(newCard);

        yield return new WaitForSeconds(0.5f);

        newCard.GetComponent<SpriteRenderer>().sprite = cardsList[randomCard].GetSprite();
        UpdateScoreAI(cardsList[randomCard].GetValue());
        cardsList.Remove(cardsList[randomCard]);

        if (cardsAI.Count > 1)
        {
            yield return new WaitForSeconds(0.25f);

            CheckLastVictory();
        }
    }

    /// <summary>
    /// Función que calcula la posición donde se va a colocar la nueva carta.
    /// </summary>
    /// <returns>Vector de posición donde se va a repartir la carta.</returns>
    Vector2 SpawnPositionAI()
    {
        int activeCards = cardsAI.Count;

        return new Vector2(activeCards * 0.75f, 3.48f);
    }

    /// <summary>
    /// Función que organiza las cartas del jugador en la pantalla cuando se reparte una nueva carta.
    /// </summary>
    void ReorganizeCardsAI()
    {
        int activeCards = cardsAI.Count;

        for (int i = 0; i < activeCards; i++)
        {
            cardsAI[i].transform.position = new Vector2(cardsAI[i].transform.position.x - 0.75f, 3.48f);
        }
    }

    /// <summary>
    /// Función activada para repartir una nueva carta a la IA.
    /// </summary>
    void NewCardAI()
    {
        StartCoroutine(InstantiateCardAI());
    }

    /// <summary>
    /// Función que actualiza la puntuación de la IA.
    /// </summary>
    /// <param name="score">La puntuación de la carta.</param>
    void UpdateScoreAI(int score)
    {
        if (score != 1)
        {
            scoreAI1 += score;
            scoreAI2 += score;
        }

        else
        {
            scoreAI1 += 11;
            scoreAI2 += 1;
        }

        WriteScore();
    }

    #endregion
}