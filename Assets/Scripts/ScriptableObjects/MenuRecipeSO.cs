using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewMeneRecipeSO", menuName = "ScriptableObjects/MenuRecipe")]
public class MenuRecipeSO : ScriptableObject
{
    public List<KitchenObjectSO> ingredients;
    public GameObject prefabVisual;
    public string recipeName;
}
