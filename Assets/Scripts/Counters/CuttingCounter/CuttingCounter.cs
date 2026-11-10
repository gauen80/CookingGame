using System;
using UnityEngine;

public class CuttingCounter : ClearCounter, IProgressTracked
{
    public static event EventHandler OnAnyCut;

    public static new void ResetStaticData()
    {
        OnAnyCut = null;
    }

    public event EventHandler<CutProgressUpdatedArg> OnCut;
    public class CutProgressUpdatedArg : EventArgs, IProgressTracked.ProgressChangedArg
    {
        public bool active;

        public bool IsBarActive()
        {
            return active;
        }

        public bool IsWarning()
        {
            return false;
        }
    }

    public event EventHandler<IProgressTracked.ProgressChangedArg> OnProgressChanged;


    public float GetNormalizedProgress()
    {
        return CuttingProgressNormalized;
    }
    [SerializeField] private CuttingRecipeSO[] cuttingRecipes;

    private CuttingRecipeSO activeCuttingRecipe;

    private int cuttingProgress;
    private bool Cutting => activeCuttingRecipe != null;
    private bool DoneCutting => cuttingProgress >= activeCuttingRecipe.cutCount;

    private float CuttingProgressNormalized => (float)cuttingProgress / activeCuttingRecipe.cutCount;

    private void Start()
    {
        OnCounterItemChange += (_, _) => ResetCutting();

        OnCut += (obj, arg) => OnProgressChanged?.Invoke(obj, arg);
        OnCut += (obj, _) => OnAnyCut?.Invoke(obj, EventArgs.Empty);
    }

    protected override void InteractAltAction(Player player)
    { 
        if (Cutting)
        {
            Cut();
        }
        else if (HoldingObject)
        {
            CuttingRecipeSO recipe = GetCuttingRecipe(currentKitchenObject.GetKitchenObjectSO());
            if (recipe)
            {
                InitalizeCutting(recipe);
                Cut();
            }
        }
    }

    private CuttingRecipeSO GetCuttingRecipe(KitchenObjectSO from)
    {
        foreach (CuttingRecipeSO recipe in cuttingRecipes)
        {
            if (recipe.from == from)
            {
                return recipe;
            }
        }
        return null;
    }

    private void InitalizeCutting(CuttingRecipeSO recipe)
    {
        
        if(!Cutting)
        {
            activeCuttingRecipe = recipe;
            SetProgress(0);
        }
        else
        {
            Debug.LogError("한번에 여러물건을 썰수 없습니다");
        }
    }

    private void ResetCutting()
    {
        activeCuttingRecipe = null;
        SetProgress(0);
    }

    private void Cut()
    {
        if (Cutting)
        {
            Debug.Log(cuttingProgress);
            SetProgress(cuttingProgress + 1);

            // Checks for done cutting
            if (DoneCutting)
            {
                currentKitchenObject.DestroySelf();
                KitchenObject.Spawn(activeCuttingRecipe.to, this);
                ResetCutting();
            }
        }
    }

    private void SetProgress(int newProgress)
    {
        cuttingProgress = newProgress;
        OnCut?.Invoke(this, new CutProgressUpdatedArg
        {
            active = cuttingProgress != 0
        });
    }
}
