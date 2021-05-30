using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// パワーアップアイテムの種類
public enum PowerUpType { None, Pushback, Rockets }

public class PowerUp : MonoBehaviour
{
    public PowerUpType powerUpType;
}
