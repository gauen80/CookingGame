using System;
using UnityEngine;

public class PlatesCounter : KitchenCounter
{
    public event EventHandler<int> OnPlatesCountUpdated;

    [SerializeField] private KitchenObjectSO plateSO;
    private PlateKitchenObject spawnedPlate;
    private GameObject visualPlate;
    private bool hasPlate ;

    private void Awake()
    {
        hasPlate = false;
    }
    private void Update()
    {
        if (!hasPlate)
        {
            visualPlate = Instantiate(plateSO.prefab, this.spawnPoint);
            hasPlate = true;
        }
    }

    protected override void InteractAction(Player player)
    {
        if (player.HoldingObject)
        {   //플레이어가 재료를 손에 들고있는 경우

            //더미 접시 생성(재료 접시위에 올리려고)
            PlateKitchenObject plate = KitchenObject.Spawn(plateSO, null) as PlateKitchenObject;
            if (player.GetCurrentKitchenObject().AddToPlate(plate))
            {

                plate.SetHolder(player);
                hasPlate = false;
            }
            else
            {
                plate.DestroySelf();
                
            }
        }
        else
        {
            //손에 아무것도 없는 경우
            KitchenObject.Spawn(plateSO, player);
            Destroy(visualPlate);
            hasPlate = false;
        }
    }


}
