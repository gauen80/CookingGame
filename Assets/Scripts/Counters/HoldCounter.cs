using System;
using UnityEngine;

public class HoldCounter : KitchenCounter, IKitchenObjectHolder
{
    public static event EventHandler<KitchenObject> OnAnyItemPlaced;

    public static void ResetStaticData()
    {
        OnAnyItemPlaced = null;
    }

    protected KitchenObject currentKitchenObject;

    public bool HoldingObject => (this as IKitchenObjectHolder).HoldingKitchenObject;

    //인터페이스구현
    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public Transform GetReferenceTransform()
    {
        return spawnPoint;
    }

    public void SetCurrentKitchenObject(KitchenObject newObject)
    {
        if (newObject == null)
        {
            currentKitchenObject = null;
        }
        else if (currentKitchenObject != null)
        {
            Debug.LogError("카운터에 이미 물건이 놓여져있습니다");
        }
        else
        {
            currentKitchenObject = newObject;
            OnAnyItemPlaced?.Invoke(this, newObject);
        }
    }
}
