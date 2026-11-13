using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMenuSO", menuName = "ScriptableObjects/Menu")]
public class MenuSO : ScriptableObject
{
    public List<MenuRecipeSO> menuRecipes;
}