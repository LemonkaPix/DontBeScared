using Unity.VisualScripting;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    public static Interaction Instance;


    Inventory inventory;
    float blockTime = 0.5f;
    float currentBlockTime = 0f;

    public float interactionRange = 3;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        inventory = GetComponent<Inventory>();
    }

    void Update()
    {

        Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        Ray ray2 = Camera.main.ScreenPointToRay(screenCenter);
        RaycastHit hit2;

        if (Physics.Raycast(ray2, out hit2, interactionRange))
        {
            MouseOverEvent mouseOverComponent = hit2.collider.GetComponent<MouseOverEvent>();
            if (mouseOverComponent != null)
            {
                mouseOverComponent.OnMouseEnter();
            }
        }

        if (Input.GetKeyDown(KeyCode.E) && !PaperUi.IsReadingPaper)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();

                if (interactable)
                {
                    interactable.Interact();
                }
            }
        }

        if (currentBlockTime > 0)
        {
            currentBlockTime -= Time.deltaTime;
            return;
        }

        if (Input.GetMouseButtonDown(0) && !PaperUi.IsReadingPaper)
        {
            currentBlockTime = blockTime;

            if (inventory.HandItem)
            {
                inventory.HandItem.GetComponent<Item>().LeftClick();
            }
        }

        if (Input.GetMouseButtonDown(1) && !PaperUi.IsReadingPaper)
        {
            currentBlockTime = blockTime;

            if (inventory.HandItem)
                inventory.HandItem.GetComponent<Item>().RightClick();
        }

        if (Input.GetKeyDown(KeyCode.Q)  && !PaperUi.IsReadingPaper)
        {
            currentBlockTime = blockTime;


            Inventory.instance.NextItem();
        }




    }
}