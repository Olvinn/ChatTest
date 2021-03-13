using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageCloudsWidget : MonoBehaviour
{
    public float widthOffset;
    public User owner;
    public UnityEvent onDestroy;

    [SerializeField] MessageWidget common;
    [SerializeField] Transform content;
    [SerializeField] Image avatar;

    LinkedList<MessageWidget> commonWidgets;
    [SerializeField] MessageWidget headWidget;

    private void Awake()
    {
        commonWidgets = new LinkedList<MessageWidget>();
        onDestroy = new UnityEvent();
    }

    void Start()
    {
        headWidget.onDestroy.AddListener(() =>
        {
            if (commonWidgets.Count == 0)
            {
                headWidget.Destroy();
                onDestroy?.Invoke();
                Destroy(gameObject, .5f);
            }
            else
            {
                headWidget.SetMsg(commonWidgets.First.Value.msg);
                headWidget.Resize((transform.parent as RectTransform).rect.width - widthOffset);
                Destroy(commonWidgets.First.Value.gameObject);
                commonWidgets.RemoveFirst();
            }
        });
    }

    public void SetUp(Message msg)
    {
        owner = msg.owner;
        if (avatar)
            avatar.sprite = Sprite.Create(msg.owner.avatar, avatar.sprite.rect, new Vector2(.5f, .5f));
        headWidget.SetUp(msg);
    }

    public void AddMessage(Message msg)
    {
        if (msg.owner != owner)
            return;

        MessageWidget newWidget = Instantiate(common.gameObject, content).GetComponent<MessageWidget>();
        newWidget.SetUp(headWidget.msg);
        newWidget.onDestroy.AddListener(() =>
        {
            commonWidgets.Remove(newWidget);
            newWidget.Destroy();
        });
        commonWidgets.AddLast(newWidget);
        headWidget.SetUp(msg);
        headWidget.Resize((transform.parent as RectTransform).rect.width - widthOffset);
        headWidget.transform.SetAsLastSibling();
    }

    public void Resize()
    {
        List<MessageWidget> widgets = new List<MessageWidget>(commonWidgets);
        widgets.Add(headWidget);
        foreach (MessageWidget widget in widgets)
        {
            widget.Resize((transform.parent as RectTransform).rect.width - widthOffset);
        }
    }

    public void ShowRemoveButtons()
    {
        List<MessageWidget> widgets = new List<MessageWidget>(commonWidgets);
        widgets.Add(headWidget);
        foreach (MessageWidget widget in widgets)
        {
            widget.ShowDeleteButton();
        }
    }

    public void HideRemoveButtons()
    {
        List<MessageWidget> widgets = new List<MessageWidget>(commonWidgets);
        widgets.Add(headWidget);
        foreach (MessageWidget widget in widgets)
        {
            widget.HideDeleteButton();
        }
    }

    //private void OnValidate()
    //{
    //    Resize();
    //}
}
