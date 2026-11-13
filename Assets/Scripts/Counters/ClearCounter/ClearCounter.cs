using System;
using UnityEngine;

public class ClearCounter : HoldCounter
{
    public event EventHandler<KitchenObject> OnCounterItemChange;

    protected override void InteractAction(Player player)
    {
        Debug.Log("player.holing : " + player.HoldingObject);
        if (player.HoldingObject)
        {
            if (!this.HoldingObject)
            {
                //카운터가 아무것도 안들고 잇음

                if (!FilterAllowedObject(player.GetCurrentKitchenObject()))
                {
                    Debug.Log("카운터에 올릴 수 없는 물건입니다");
                }
                else
                {
                    
                    player.GetCurrentKitchenObject().SetHolder(this);

       
                    OnCounterItemChange?.Invoke(this, null);
                }
            }
            else
            {
                bool counterHoldingPlate = currentKitchenObject.TryGetObject(out PlateKitchenObject counterPlate);
                bool playerHoldingPlate = player.GetCurrentKitchenObject().TryGetObject(out PlateKitchenObject playerPlate);

                if (counterHoldingPlate && !playerHoldingPlate)
                {

                    player.GetCurrentKitchenObject().AddToPlate(counterPlate);
                }
                else if (!counterHoldingPlate && playerHoldingPlate)
                {
                    if (this.currentKitchenObject.AddToPlate(playerPlate))
                    {
                    
                        OnCounterItemChange?.Invoke(this, null);
                    }
                }
                else
                {
                    //카운터 & 플레이어 모두 접시를 들고있거나 재료를 들고 있을때
  
                }

            }
        }
        else if (this.HoldingObject)
        {
            //플레이어가 빈손
            this.currentKitchenObject.SetHolder(player);
            OnCounterItemChange?.Invoke(this, null);
        }
    }

    
    protected virtual bool FilterAllowedObject(KitchenObject kitchenObject)
    {
        return true;
    }
}
