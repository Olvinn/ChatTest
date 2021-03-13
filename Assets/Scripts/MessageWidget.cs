using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MessageWidget : MonoBehaviour
{
    public Message msg { get; private set; }

    [SerializeField] LayoutElement _le;
    [SerializeField] float widthOffset;
    [SerializeField] TextMeshProUGUI _chatText, _time, _userName;
    [SerializeField] Button _delete;

    public UnityEvent onDestroy;

    Animator _anim;

    private void Awake()
    {
        onDestroy = new UnityEvent();
        _anim = GetComponent<Animator>();
    }

    public void SetMsg(Message msg)
    {
        this.msg = msg;
        _chatText.text = msg.message;
        DateTime time = DateTime.FromBinary(msg.time);
        _time.text = $"{time.Hour:d2}:{time.Minute:d2}:{time.Second:d2}";
        if (_userName)
            _userName.text = msg.owner.userName;
    }

    public void SetUp(Message msg)
    {
        this.msg = msg;
        _chatText.text = msg.message;
        DateTime time = DateTime.FromBinary(msg.time);
        _time.text = $"{time.Hour:d2}:{time.Minute:d2}:{time.Second:d2}";
        if (_userName)
            _userName.text = msg.owner.userName;
        if (_anim)
        {
            _anim.Play("Dissapear");
            _anim.SetBool("Active", true);
        }
    }

    public void Resize(float prefferedWidth)
    {
        float width =_chatText.preferredWidth + widthOffset;
        width = Mathf.Max(width, _time.preferredWidth + widthOffset);
        if (_userName)
            width = Mathf.Max(width, _userName.preferredWidth + widthOffset);
        _le.preferredWidth = Mathf.Min(width, prefferedWidth);
    }

    public void StartDestroy()
    {
        onDestroy?.Invoke();
    }

    public void Destroy()
    {
        _anim.SetTrigger("Remove");
        Destroy(gameObject, .5f);
    }

    public void ShowDeleteButton()
    {
        if (_anim)
            _anim.SetBool("Delete", true);
    }

    public void HideDeleteButton()
    {
        if (_anim)
            _anim.SetBool("Delete", false);
    }
}
