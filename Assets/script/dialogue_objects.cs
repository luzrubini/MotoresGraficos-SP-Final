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

    public int orgullo = 0;
    public int humildad = 0;

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
            if (Input.GetKeyDown(KeyCode.Alpha1))
                ChooseOption(1);
            if (Input.GetKeyDown(KeyCode.Alpha2))
                ChooseOption(2);
        }
    }


    public void StartInteraction(InteractionObject obj)
    {
        StartCoroutine(InteractionFlow(obj));
    }


    private IEnumerator InteractionFlow(InteractionObject obj)
    {
        currentObject = obj;
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
            {
                m.AddCrack();
            }

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
}
