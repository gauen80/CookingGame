using UnityEngine;


[CreateAssetMenu(fileName = "NewCuttingRecipe", menuName = "ScriptableObjects/CuttingRecipe")]

public class CuttingRecipeSO : ScriptableObject
{
    public KitchenObjectSO from;
    public KitchenObjectSO to;
    public int cutCount;
}
