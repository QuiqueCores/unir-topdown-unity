using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class QuestListItemView : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    [SerializeField] private TMP_Text titleText;

    public QuestStatus BoundStatus { get; private set; }

    public event Action<QuestListItemView> OnSelected;

    public void Bind(QuestStatus status)
    {
        BoundStatus = status;
        string statusText = "[En curso] ";
        if (status.isCompleted)
        {
            statusText = "[Completada] ";
        }
        titleText.text = $"{statusText}{status.QuestData.questName}";
    }

    public void OnSelect(BaseEventData eventData)
    {
        OnSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (EventSystem.current != null)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
        }

        OnSelected?.Invoke(this);
    }
}