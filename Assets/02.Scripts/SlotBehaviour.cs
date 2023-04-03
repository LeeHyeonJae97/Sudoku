using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SlotBehaviour : MonoBehaviour
{
    public int Row { get; set; }
    public int Column { get; set; }
    public int Number { get; set; }

    private void Start()
    {
        GetComponentInChildren<TextMeshPro>().text = $"{Number}";
    }
}
