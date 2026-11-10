using System;
using UnityEngine;

public class ContainerCounter : KitchenCounter
{
    public static event EventHandler<KitchenObjectSO> OnAnyObjectSpawned;

    public static void ResetStaticData()
    {
        OnAnyObjectSpawned = null;
    }

    [SerializeField] private KitchenObjectSO kitchenObject;

    protected override void InteractAction(Player player)
    {
       
        if (player.HoldingObject)
        {
            //손에 무언갈 들고있으면 보류

        }
        else
        {
      
            KitchenObject.Spawn(kitchenObject, player);
            OnAnyObjectSpawned?.Invoke(this, kitchenObject);
        }
    }
}
