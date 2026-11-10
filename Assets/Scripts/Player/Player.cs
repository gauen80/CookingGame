using System;
using UnityEngine;
using System.Collections.Generic;


public class Player : MonoBehaviour, IKitchenObjectHolder
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangeEventArg> OnSelectedCounterChange;
    public class OnSelectedCounterChangeEventArg : EventArgs
    {
        public KitchenCounter selected;
    }

    public event EventHandler<KitchenObject> OnPlayerPickedUp;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float speedUpScale = 2f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float playerHeight = 2f;
    [SerializeField] private float playerRadius = 0.7f;
    [SerializeField] private float playerInteractDistance = 2f;
    [SerializeField] private float diagnalMovementThreshold = 0.5f;
    [SerializeField] private LayerMask playerInteractLayerMask;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private Transform objectPickupPoint;
    [SerializeField] private Transform raycastPoint;

    private KitchenObject currentKitchenObject = null;

    // 현재 플레이어가 바라보고 있는 대상들
    private KitchenCounter selectedCounter = null;
    private KitchenObject selectedObject = null;


    private bool isWalking = false;
    private bool isSprinting = false;


    //다른데서 데이터 접근할 수 있게
    public bool IsWalking => isWalking;
    public bool IsSprinting => isSprinting;
    public bool HoldingObject => (this as IKitchenObjectHolder).HoldingKitchenObject;

    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogError("Cannot have multiple instance of player");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnPlayerInteract += InteractionAction;
        gameInput.OnPlayerInteractAlt += InteractAltAction;

       
    }

    private void Update()
    {
        Vector2 inputVector = gameInput.GetMovementVector();

        isWalking = inputVector != Vector2.zero;
        if(isWalking)
        {
            Vector3 moveDirection = new(inputVector.x, 0f, inputVector.y);
            HandleMovement(moveDirection);
        }

        HandleInteraction();
    }

    private void HandleMovement(Vector3 moveDirection)
    {
        float moveDistance = moveSpeed * Time.deltaTime;

        isSprinting = gameInput.IsSpeedingUp();
        if (isSprinting)
            moveDistance *= speedUpScale;
        
        Vector3 actualMoveTransform = GetNormalizedMovementTransform(moveDirection, moveDistance);
        if (actualMoveTransform != Vector3.zero)
        {
            // move player
            transform.position += actualMoveTransform;
        }

        // rotate player to face the direction
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
    }

    private Vector3 GetNormalizedMovementTransform(Vector3 moveDirection, float moveDistance)
    {
        Vector3 PlayerBottom = transform.position;
        Vector3 PlayerTop = transform.position + playerHeight * Vector3.up;
        bool CanMoveTowards(Vector3 dir, float dist) => !Physics.CapsuleCast(PlayerBottom, PlayerTop, playerRadius, dir, dist);

        if (CanMoveTowards(moveDirection.normalized , moveDistance))
        {
            return moveDirection.normalized * moveDistance;
        }

        Vector3 alternativeDirection = new(moveDirection.x, 0, 0);
        if(IsMovementAboveThreshold(moveDirection.z) && CanMoveTowards(alternativeDirection, moveDistance))
        {
            return alternativeDirection.normalized * moveDistance;
        }

        return Vector3.zero;
    }

    //조이스틱 조작감개선
    private bool IsMovementAboveThreshold(float movement)
    {
        return movement < -diagnalMovementThreshold || movement > diagnalMovementThreshold;
    }


    /// <summary>
    /// 상호작용 키 이벤트
    /// </summary>
    private void HandleInteraction()
    {
        Debug.DrawRay(raycastPoint.position, transform.forward * playerInteractDistance, Color.red);

        bool interactHit = Physics.Raycast(raycastPoint.position, transform.forward, out RaycastHit interactObject, playerInteractDistance, playerInteractLayerMask);
        
        
        if (interactHit && interactObject.transform.TryGetComponent(out KitchenCounter counter))
        {
            //카운터와 상호작용
            //Debug.Log("충돌 counter" + counter);
            SetSelectedCounter(counter);
        }
        else if (interactHit && interactObject.transform.TryGetComponent(out KitchenObject ingredient))
        {
            //바닥에 있는 재료 줍기
            SetSelectedCounter(null);
            selectedObject = ingredient;
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void InteractionAction(object sender, EventArgs args)
    {
        if (selectedCounter != null )
        {
            selectedCounter.Interact(this);
        }
        else if(currentKitchenObject == null && selectedObject != null)
        {
            //손에 없음
            //selectedObject.SetHolder(this);
           
        }
        else if(currentKitchenObject != null)
        {
            //손에 들고 있음
            //DropObject(currentKitchenObject);
        }
    }

    private void InteractAltAction(object sender, EventArgs args)
    {
        if (selectedCounter != null )
        {
            selectedCounter.InteractAlt(this);
        }
    }

    
    private void SetSelectedCounter(KitchenCounter newCounter)
    {
        selectedCounter = newCounter;
        OnSelectedCounterChange?.Invoke(this, new OnSelectedCounterChangeEventArg { selected = newCounter });
    }

    private void DropObject(KitchenObject holdObject)
    {
        holdObject.SetHolder(null);
    }

    /// <summary>
    /// 인터페이스 함수 구현
    /// </summary>
    /// <returns></returns>
    public KitchenObject GetCurrentKitchenObject()
    {
        return currentKitchenObject;
    }

    public void SetCurrentKitchenObject(KitchenObject newObject)
    {
        if( newObject == null)
        {
            currentKitchenObject = null;
        }
        else if(currentKitchenObject != null)
        {
            Debug.LogError("이미 물건을 들고 있습니다");
        }
        else
        {
            currentKitchenObject = newObject;
            OnPlayerPickedUp?.Invoke(this, newObject);
        }
    }

    public Transform GetReferenceTransform()
    {
        return objectPickupPoint;
    }

    
}
