using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{
    private KitchenCounter counter;

    void Start()
    {
        Player.Instance.OnSelectedCounterChange += Player_CounterChange;
        counter = GetComponentInParent<KitchenCounter>();
    }

    private void Player_CounterChange(object sender, Player.OnSelectedCounterChangeEventArg e)
    {
        SetSelectedVisual(counter == e.selected);
    }

    private void SetSelectedVisual(bool active)
    {
        
    }
}
