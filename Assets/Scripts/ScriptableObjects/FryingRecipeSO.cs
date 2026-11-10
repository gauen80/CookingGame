using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FryingRecipe", menuName = "ScriptableObjects/FryingRecipe")]
public class FryingRecipeSO : ScriptableObject
{
    public KitchenObjectSO from;
    public KitchenObjectSO to;
    public float fryingTime;
    public bool shouldWarning;
}