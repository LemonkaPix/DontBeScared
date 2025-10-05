using UnityEngine;

public class Billboard : MonoBehaviour
{
    Transform cameraTransform;

    [SerializeField] private bool allAxis = false;

    void Start()
    {
        cameraTransform = Camera.main.transform;
    }


    void Update()
    {
        if (!allAxis)
        {
            transform.LookAt(cameraTransform);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y + 180, 0);
        }
        else
        {
            transform.rotation = cameraTransform.rotation;
        }
    }
}
