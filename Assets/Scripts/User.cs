using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "User")]
public class User : ScriptableObject
{
    public Texture2D avatar;
    public string userName;
}
