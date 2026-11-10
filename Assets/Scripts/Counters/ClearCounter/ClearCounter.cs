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
