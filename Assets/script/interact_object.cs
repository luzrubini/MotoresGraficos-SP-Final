using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionObject : MonoBehaviour
{
    [TextArea(3, 5)] public string introText;   
    [TextArea(3, 5)] public string option1Text; 
    [TextArea(3, 5)] public string option2Text;
    [TextArea(3, 5)] public string msgOrgullo;
    [TextArea(3, 5)] public string msgHumildad;
    [TextArea(3, 5)] public string msgIguales;
    [TextArea(3, 5)] public string msgFinalOrgullo;
    [TextArea(3, 5)] public string msgFinalHumildad;

    [HideInInspector] public bool alreadyInteracted = false; 

    public void Interact()
    {
        if (alreadyInteracted)
        {
            
            DialogueManager.Instance.ShowTemporaryMessage("Ya pasé por aquí, no tiene sentido arrepentirse.", 3f);
            return;
        }

        DialogueManager.Instance.StartInteraction(this);
        alreadyInteracted = true; 
    }
}
