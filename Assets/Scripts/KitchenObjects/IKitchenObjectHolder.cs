using UnityEngine;

public interface IKitchenObjectHolder 
{

    public KitchenObject GetCurrentKitchenObject();
    public void SetCurrentKitchenObject(KitchenObject newObject);

    public Transform GetReferenceTransform();

    public bool HoldingKitchenObject => GetCurrentKitchenObject() != null;
}
