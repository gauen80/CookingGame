using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewKitchenObject", menuName = "ScriptableObjects/KitchenObject")]
public class KitchenObjectSO : ScriptableObject
{
    public GameObject prefab;
    public Sprite icon;
    public string objectName; 
}