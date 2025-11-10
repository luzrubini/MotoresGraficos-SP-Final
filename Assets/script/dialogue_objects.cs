using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text introTextUI;
    public TMP_Text option1TextUI;
    public TMP_Text option2TextUI;

    [Header("Moral System")]
    public int orgullo = 0;
    public int humildad = 0;

    [Header("Puzzle Tracking")]
    private bool libroFound = false;
    private bool cartasFound = false;
    private bool crucifijoFound = false;
    private bool virgilioUnlocked = false;

    private InteractionObject currentObject;
    private bool isChoosing = false;
    private FogController fogController;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        dialoguePanel.SetActive(false);

        fogController = FindObjectOfType<FogController>();
    }

    void Update()
    {
        if (isChoosing)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) ChooseOption(1);
            if (Input.GetKeyDown(KeyCode.Alpha2)) ChooseOption(2);
        }
    }

    public void StartInteraction(InteractionObject obj)
    {
        currentObject = obj;

        // Caso especial: Virgilio
        if (obj.isVirgilio)
        {
            StartCoroutine(VirgilioDialogue());
            return;
        }

        // Si es otro objeto, diálogo normal
        StartCoroutine(InteractionFlow(obj));
    }

    private IEnumerator InteractionFlow(InteractionObject obj)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = obj.introText;
        introTextUI.gameObject.SetActive(true);
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        yield return WaitForSecondsOrSpace(5f);

        if (orgullo > humildad)
            introTextUI.text = obj.msgOrgullo;
        else if (humildad > orgullo)
            introTextUI.text = obj.msgHumildad;
        else
            introTextUI.text = obj.msgIguales;

        yield return WaitForSecondsOrSpace(5f);

        introTextUI.gameObject.SetActive(false);
        option1TextUI.text = "1 - " + obj.option1Text;
        option2TextUI.text = "2 - " + obj.option2Text;
        option1TextUI.gameObject.SetActive(true);
        option2TextUI.gameObject.SetActive(true);

        isChoosing = true;
        Time.timeScale = 0;
    }

    private IEnumerator WaitForSecondsOrSpace(float seconds)
    {
        float timer = 0f;
        while (timer < seconds)
        {
            if (Input.GetKeyDown(KeyCode.Space))
                break;
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    void ChooseOption(int choice)
    {
        isChoosing = false;
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);
        Time.timeScale = 1;

        string finalMessage = "";

        if (choice == 1)
        {
            orgullo++;
            finalMessage = currentObject.msgFinalOrgullo;

            MirrorState[] mirrors = FindObjectsOfType<MirrorState>();
            foreach (MirrorState m in mirrors)
                m.AddCrack();

            if (fogController != null)
                fogController.IncreaseDensity();

            Debug.Log("Elegiste opción 1, Orgullo = " + orgullo);
        }
        else if (choice == 2)
        {
            humildad++;
            finalMessage = currentObject.msgFinalHumildad;

            if (fogController != null)
                fogController.DecreaseDensity();

            Debug.Log("Elegiste opción 2, Humildad = " + humildad);
        }

        StartCoroutine(ShowFinalMessage(finalMessage, 5f));

        // Registrar progreso del puzzle
        RegisterPuzzleObject(currentObject.objectName);
    }

    private IEnumerator ShowFinalMessage(string msg, float tiempo)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = msg;
        introTextUI.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(tiempo);
        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    public void ShowTemporaryMessage(string msg, float tiempo = 2f)
    {
        StartCoroutine(TemporaryMessageCoroutine(msg, tiempo));
    }

    private IEnumerator TemporaryMessageCoroutine(string msg, float tiempo)
    {
        dialoguePanel.SetActive(true);
        introTextUI.text = msg;
        introTextUI.gameObject.SetActive(true);

        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        yield return new WaitForSecondsRealtime(tiempo);
        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }

    // ==============================
    // PROGRESO DEL PUZZLE
    // ==============================

    private void RegisterPuzzleObject(string name)
    {
        if (name.ToLower().Contains("libro")) libroFound = true;
        if (name.ToLower().Contains("cartas")) cartasFound = true;
        if (name.ToLower().Contains("crucifijo")) crucifijoFound = true;

        if (!virgilioUnlocked && libroFound && cartasFound && crucifijoFound)
        {
            virgilioUnlocked = true;
            Debug.Log("🕯️ Puzzle completo. Virgilio ahora tiene nuevo diálogo.");
            ShowTemporaryMessage("Sientes que algo cambia en la habitación...", 3f);
        }
    }

    // ==============================
    // DIÁLOGOS DE VIRGILIO
    // ==============================

    private IEnumerator VirgilioDialogue()
    {
        dialoguePanel.SetActive(true);
        introTextUI.gameObject.SetActive(true);
        option1TextUI.gameObject.SetActive(false);
        option2TextUI.gameObject.SetActive(false);

        string msg = "";

        // Si el puzzle está completo → diálogo final
        if (virgilioUnlocked)
        {
            if (orgullo > humildad)
                msg = "Has abierto las reliquias, pero solo viste lo que querías ver.\n" +
                      "El orgullo busca verdad solo para exhibirla.";
            else if (humildad > orgullo)
                msg = "Has mirado sin huir.\nLa eternidad no es castigo, sino espejo.";
            else
                msg = "Has dicho las palabras, pero aún no comprendes su peso.\n‘Per me si va nell’etterno’…";

            introTextUI.text = msg;
            yield return WaitForSecondsOrSpace(10f);
        }
        else
        {
            // Puzzle incompleto → Virgilio da pistas según moral
            int progress = (libroFound ? 1 : 0) + (cartasFound ? 1 : 0) + (crucifijoFound ? 1 : 0);

            if (progress == 0)
            {
                msg = "Tres fragmentos duermen bajo el polvo, Gabriel.\nEmpieza por aquello que guarda palabras no dichas.";
            }
            else if (progress == 1)
            {
                msg = "El camino se abre un poco más.\nA veces la fe pesa más que el hierro, y las cartas mienten menos que las bocas.";
            }
            else if (progress == 2)
            {
                msg = "Solo queda una verdad por mirar de frente.\n¿Podrás sostenerla sin romperte?";
            }

            if (orgullo > humildad)
                msg += "\nTu voz suena alta, pero el silencio enseña más que tu eco.";
            else if (humildad > orgullo)
                msg += "\nTu paso es leve, pero no olvides que incluso la humildad puede cegar.";

            introTextUI.text = msg;
            yield return WaitForSecondsOrSpace(8f);
        }

        introTextUI.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }
}
