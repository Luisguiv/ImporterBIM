using UnityEngine;
using UnityEngine.EventSystems;

public class SidebarSlide : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float speed = 10f;

    RectTransform rt;
    float hiddenX;              // vem do próprio RectTransform
    float shownX;
    float targetX;

    void Awake()
    {
        rt = GetComponent<RectTransform>();

        // Pega a posição X inicial como "escondida"
        hiddenX = rt.anchoredPosition.x;
        targetX = hiddenX;
        shownX = 155f;
    }

    void Update()
    {
        var pos = rt.anchoredPosition;
        pos.x = Mathf.Lerp(pos.x, targetX, Time.deltaTime * speed);
        rt.anchoredPosition = pos;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetX = hiddenX + shownX;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetX = hiddenX;
    }
}
