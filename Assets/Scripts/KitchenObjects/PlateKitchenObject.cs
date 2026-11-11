using System;
using UnityEngine;

public class PlateKitchenObject : MonoBehaviour
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
        //public MenuRecipeSO currentRecipe;
        //public List<KitchenObjectSO> currentIngredients;
        //public KitchenObjectSO NewIngredient => currentIngredients[^1];  // the last index
    }
}
