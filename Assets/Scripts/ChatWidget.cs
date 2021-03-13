using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatWidget : MonoBehaviour
{
    [SerializeField] User _me;
    [SerializeField] User[] _users;

    [SerializeField] ScrollRect _scrollPane;
    [SerializeField] MessageCloudsWidget _inputMsg, _outputMsg;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Transform _chatScrollView;

    LinkedList<MessageCloudsWidget> _clouds;

    void Start()
    {
        _clouds = new LinkedList<MessageCloudsWidget>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            AddMessage();
    }

    //через этот метод должны добавляться сообщения. но сейчас они здесь создаются
    public void AddMessage()
    {
        if (_inputField.text != "")
        {
            User user = _users[Random.Range(0, _users.Length)];
            Message msg = MessageFactory.Instance.Create(user, _inputField.text);

            if (user != _clouds.Last?.Value.owner)
            {
                MessageCloudsWidget widget;
                if (user == _me)
                {
                    widget = Instantiate(_outputMsg.gameObject, _chatScrollView).GetComponent<MessageCloudsWidget>();
                    widget.onDestroy.AddListener(() =>
                    {
                        _clouds.Remove(widget);
                    });
                }
                else
                {
                    widget = Instantiate(_inputMsg.gameObject, _chatScrollView).GetComponent<MessageCloudsWidget>();
                }

                widget.SetUp(msg);
                _clouds.AddLast(widget);
                _scrollPane.normalizedPosition = Vector2.zero;
                StartCoroutine(ResizeWidget(_clouds.Last.Value));
            }
            else
            {
                _clouds.Last.Value.AddMessage(msg);
                StartCoroutine(ResizeWidget(_clouds.Last.Value));
            }

            _inputField.text = "";
        }
        _inputField.ActivateInputField();
    }

    public void OnPushDeleteMessages()
    {
        foreach (MessageCloudsWidget widget in _clouds)
            if (widget.owner == _me)
                widget.ShowRemoveButtons();
    }

    public void OnPushOkDeleteMessages()
    {
        foreach (MessageCloudsWidget widget in _clouds)
            if (widget.owner == _me)
                widget.HideRemoveButtons();
    }

    IEnumerator ResizeWidget(MessageCloudsWidget widget)
    {
        yield return new WaitForEndOfFrame();
        widget.Resize();
        _scrollPane.normalizedPosition = Vector2.zero;
    }
}
