using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interacción")]
    public float interactionRange = 3f;
    public LayerMask interactableLayer;

    [Header("UI")]
    public TMP_Text interactionPrompt; 

    private bool interacting = false;

    void Update()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRange, interactableLayer);

        if (hits.Length > 0 && !interacting)
        {
            interactionPrompt.gameObject.SetActive(true);


            Color c = interactionPrompt.color;
            c.a = Mathf.PingPong(Time.time * 2f, 1f); 
            interactionPrompt.color = c;

            InteractionObject obj = hits[0].GetComponent<InteractionObject>();

            if (Input.GetKeyDown(KeyCode.E) && obj != null)
            {
                interacting = true;
                interactionPrompt.gameObject.SetActive(false);
                obj.Interact();
                Debug.Log("Interaccioné con: " + obj.name);
            }
        }
        else if (hits.Length == 0)
        {
            interactionPrompt.gameObject.SetActive(false);
            interacting = false;
        }
    }


    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        Vector3 offset = Vector3.up * 6f;

        Gizmos.DrawWireSphere(transform.position + offset, interactionRange);
    }

}
