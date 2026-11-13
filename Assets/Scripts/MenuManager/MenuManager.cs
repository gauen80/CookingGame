using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private MenuSO menu;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
    }

    public bool DoesMakeMenuItem(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        return CanMake(ingredients, recipe);
    }

    public MenuRecipeSO FindRecipeCandidate(List<KitchenObjectSO> ingredients)
    {
        foreach (MenuRecipeSO recipe in menu.menuRecipes)
        {
            if (WillMake(ingredients, recipe))
            {
                return recipe;
            }
        }
        return null;
    }

    //부분적으로 완성되었는지
    private bool WillMake(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        ISet<int> ingrSet = new HashSet<int>();
        foreach (KitchenObjectSO plateIngr in ingredients)
        {
            int ingrIndex = FindNotInSet(recipe.ingredients, plateIngr, ingrSet);
            if (ingrIndex < 0)
            {
                return false;
            }
            ingrSet.Add(ingrIndex);
        }
        return true;
    }

    //요리가 완전히 완성되었는지
    private bool CanMake(List<KitchenObjectSO> ingredients, MenuRecipeSO recipe)
    {
        ISet<int> ingrSet = new HashSet<int>();
        foreach (KitchenObjectSO recipeIngr in recipe.ingredients)
        {
            int ingrIndex = FindNotInSet(ingredients, recipeIngr, ingrSet);
            if (ingrIndex < 0)
            {
                return false;
            }
            ingrSet.Add(ingrIndex);
        }
        return true;
    }
    private int FindNotInSet(List<KitchenObjectSO> soList, KitchenObjectSO so, ISet<int> exclusionSet)
    {
        for (int index = 0; index < soList.Count; ++index)
        {
            if (exclusionSet.Contains(index))
            {
                continue;
            }
            else if (soList[index] != so)
            {
                continue;
            }
            return index;
        }
        return -1;
    }
}
