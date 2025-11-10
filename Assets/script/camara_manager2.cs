using UnityEngine;

public class CameraSwitcher : MonoBehaviour
{
    public Camera[] cameras;  // Arrastrá tus 3 cámaras acá
    private int currentIndex = 0;

    void Start()
    {
        // Desactiva todas menos la primera
        for (int i = 0; i < cameras.Length; i++)
            cameras[i].gameObject.SetActive(i == 0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            // Desactivar cámara actual
            cameras[currentIndex].gameObject.SetActive(false);

            // Pasar a la siguiente
            currentIndex = (currentIndex + 1) % cameras.Length;

            // Activar la nueva
            cameras[currentIndex].gameObject.SetActive(true);
        }
    }
}
