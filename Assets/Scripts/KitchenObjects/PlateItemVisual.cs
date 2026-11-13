using UnityEngine;

public class PlateItemVisual : MonoBehaviour
{
    private PlateKitchenObject plate;
    private GameObject plateItem;

    private void Awake()
    {
        plate = GetComponentInParent<PlateKitchenObject>();
        // called here because Start is not called on Instansiate
        plate.OnPlateUpdated += Plate_OnPlateUpdated;
    }

    private void Plate_OnPlateUpdated(object sender, PlateKitchenObject.PlateUpdatedArg e)
    {
        if (plateItem == null || e.recipeChanged)
        {
            //첫재료가 담김 or 요리 레시피가 바뀜
            if (plateItem != null)
            {
                Destroy(plateItem);
            }

            plateItem = Instantiate(e.currentRecipe.prefabVisual, transform);
            plateItem.transform.localPosition = Vector3.zero;

            foreach (Transform plateItemChild in plateItem.transform)
            {
                plateItemChild.gameObject.SetActive(false);
            }

            foreach (KitchenObjectSO ingredient in e.currentIngredients)
            {
                SyncIngredientVisual(e.currentRecipe, ingredient);
            }
        }
        else
        {
            // 접시가 비어잇으니 접시위에 그냥 재료 올리기
            SyncIngredientVisual(e.currentRecipe, e.NewIngredient);
        }


    }

    private void SyncIngredientVisual(MenuRecipeSO recipe, KitchenObjectSO ingredient)
    {
        for (int index = 0; index < recipe.ingredients.Count; ++index)
        {
            // new ingredient should be matched to an previously not active item
            if (recipe.ingredients[index] == ingredient && !plateItem.transform.GetChild(index).gameObject.activeSelf)
            {
                plateItem.transform.GetChild(index).gameObject.SetActive(true);
                return;
            }
        }
        Debug.LogErrorFormat("Cannot find visual for {0} in recipe {1}", ingredient.objectName, recipe.recipeName);
    }
}
