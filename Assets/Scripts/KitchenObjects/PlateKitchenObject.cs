using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public static event EventHandler<KitchenObjectSO> OnAnyIngredientAdded;

    public static void ResetStaticData()
    {
        OnAnyIngredientAdded = null;
    }

    public event EventHandler<PlateUpdatedArg> OnPlateUpdated;
    public class PlateUpdatedArg : EventArgs
    {
        public bool recipeChanged;
        public MenuRecipeSO currentRecipe;
        public List<KitchenObjectSO> currentIngredients;
        public KitchenObjectSO NewIngredient => currentIngredients[^1];
    }

    private MenuRecipeSO currentRecipe;
    private List<KitchenObjectSO> currentIngredients;

    private void Awake()
    {
        currentIngredients = new List<KitchenObjectSO>();
    }

    public int GetNumIngredients()
    {
        return currentIngredients.Count;
    }

    public bool TryAddIngredient(KitchenObjectSO newIngredient)
    {
        if (newIngredient == GetKitchenObjectSO())
        {
            //같은 접시면 추가할 수 없음
            return false;
        }

        List<KitchenObjectSO> tempIngredients = new(currentIngredients)
        {
            //복사해서 새 재료추가한 tempIngr리스트
            newIngredient
        };

        MenuRecipeSO recipeCandidate = MenuManager.Instance.FindRecipeCandidate(tempIngredients);
        if (recipeCandidate)
        {
            bool recipeChanged = recipeCandidate != currentRecipe;
            if (recipeChanged)
            {
                currentRecipe = recipeCandidate;
            }
            currentIngredients = tempIngredients;

            OnPlateUpdated?.Invoke(this, new PlateUpdatedArg
            {
                recipeChanged = recipeChanged,
                currentRecipe = currentRecipe,
                currentIngredients = currentIngredients,
            });

            OnAnyIngredientAdded?.Invoke(this, newIngredient);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool TryMakeMenuItem(out MenuRecipeSO deliveredRecipe)
    {
        deliveredRecipe = null;
        return false;
    }

    public override string ToString()
    {
        string str = "Plate {";
        foreach (KitchenObjectSO ko in currentIngredients)
        {
            str += ko.objectName + ", ";
        }
        str += "}";
        return str;
    }
}
