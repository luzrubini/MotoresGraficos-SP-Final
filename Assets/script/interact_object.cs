using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [Header("Diálogo Base")]
    [TextArea(3, 5)] public string introText;
    [TextArea(3, 5)] public string option1Text;
    [TextArea(3, 5)] public string option2Text;
    [TextArea(3, 5)] public string msgOrgullo;
    [TextArea(3, 5)] public string msgHumildad;
    [TextArea(3, 5)] public string msgIguales;
    [TextArea(3, 5)] public string msgFinalOrgullo;
    [TextArea(3, 5)] public string msgFinalHumildad;

    [Header("Identificación del Objeto")]
    public string objectName;        // Ej: "Libro", "Cartas", "Crucifijo", "Reloj", "Fotos", etc.
    public bool isVirgilio = false;  // Marcar TRUE solo en el NPC Virgilio

    [HideInInspector] public bool alreadyInteracted = false;

    public void Interact()
    {
        // Evita repetir interacción excepto con Virgilio
        if (alreadyInteracted && !isVirgilio)
        {
            DialogueManager.Instance.ShowTemporaryMessage("Ya pasé por aquí, no tiene sentido arrepentirse.", 3f);
            return;
        }

        DialogueManager.Instance.StartInteraction(this);

        // Solo los objetos comunes se marcan como usados
        if (!isVirgilio)
            alreadyInteracted = true;
    }
}
