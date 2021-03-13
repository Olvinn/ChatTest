using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Message
{
    public User owner;
    public long time, id;
    public string message;
}

public class MessageFactory
{
    public static MessageFactory Instance { get { if (_instance == null) _instance = new MessageFactory(); return _instance; } }
    static MessageFactory _instance;

    static long id = 0;

    public Message Create(User user, string message)
    {
        return new Message() { owner = user, id = id++, message = message, time = DateTime.Now.Ticks };
    }
}
