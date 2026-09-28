using UnityEngine;

public class Chest : MonoBehaviour, Interactable
{
    [SerializeField] private Vector3 dropOffsetPosition = new Vector3(0f, -0.5f, 0f);
    private bool isOpened = false;
    public GameObject itemPrefab;
    public Sprite openedSprite;

    public void Interact()
    {
        if (!CanInteract())
        {
            return;
        }
        OpenChest();

    }

    public bool CanInteract()
    {
        return !isOpened;
    }

    public void OpenChest()
    {
        if (itemPrefab)
        {
            isOpened = true;

            GameObject droppedItem = Instantiate(itemPrefab, transform.position + dropOffsetPosition, Quaternion.identity);
            GetComponent<SpriteRenderer>().sprite = openedSprite;

        }
    }
}
