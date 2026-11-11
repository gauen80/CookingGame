using UnityEngine;

public class KitchenObject : MonoBehaviour
{
    [SerializeField] private KitchenObjectSO kitchenObjectSO;

    public int currentCutCount = 0;
    public Rigidbody rb;

    public KitchenObjectSO GetKitchenObjectSO() { return kitchenObjectSO; }

    private IKitchenObjectHolder currentHolder;

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>(); rb.useGravity = false; rb.isKinematic = true;
    }

    private void OffGravity()
    {
        
            rb.useGravity = false;
            rb.isKinematic = true;
    }

    private void OnGravity()
    {
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    public void SetHolder(IKitchenObjectHolder newHolder)
    {
        //현재 홀더를 비우고 새홀더로 옮기기
        currentHolder?.SetCurrentKitchenObject(null);
        newHolder?.SetCurrentKitchenObject(this);

        currentHolder = newHolder;

        if(newHolder != null)
        {
            transform.parent = newHolder.GetReferenceTransform();
            transform.localPosition = Vector3.zero;
            OffGravity();
        }
        else
        {
            transform.parent = null;
            OnGravity();
        }
    }

    public IKitchenObjectHolder GetHolder()
    {
        return currentHolder;
    }

    public bool TryGetObject<T>(out T output) where T: KitchenObject
    {
        if(this is T)
        {
            output = this as T;
            return true;
        }

        else
        {
            output = null;
            return false;
        }
    }

    // public bool AddToPlate(PlateKitchenObject plate)
    public void DestroySelf()
    {
        SetHolder(null);
        Destroy(gameObject);
        
    }

    public static KitchenObject Spawn(KitchenObjectSO koSO, IKitchenObjectHolder holder)
    {
        GameObject spawned = Instantiate(koSO.prefab);
        KitchenObject newKitchenObj = spawned.GetComponent<KitchenObject>();
        newKitchenObj.SetHolder(holder);
       

        return newKitchenObj;
    }
}
