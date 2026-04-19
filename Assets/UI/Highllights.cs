using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Highllights : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    [SerializeField] private GameObject highlight;

    private void Start()
    {
        if (highlight != null)
            highlight.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(false);
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (highlight != null)
            highlight.SetActive(false);
    }
}
