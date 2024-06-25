using System.Collections.Generic; // Add this line to include the List<> class
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARInteractionManager : MonoBehaviour
{
    [SerializeField] private Camera arCamera;

    private ARRaycastManager arRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private GameObject arPointer;
    private GameObject item3DModel;
    private GameObject itemSelected;
    private bool isInitialPosition;
    private bool isOverUI;
    private bool isOver3DModel;

    private Vector2 initialTouchPos;

    public GameObject Item3DModel
    {
        set
        {
            item3DModel = value;
            if (item3DModel != null && arPointer != null)
            {
                item3DModel.transform.position = arPointer.transform.position;
                item3DModel.transform.parent = arPointer.transform;
                isInitialPosition = true;
                Debug.Log("Item3DModel set and positioned.");
            }
            else
            {
                Debug.LogWarning("Item3DModel or arPointer is null.");
            }
        }
    }

    private void Start()
    {
        arPointer = transform.GetChild(0).gameObject;
        arRaycastManager = FindObjectOfType<ARRaycastManager>();

        if (arPointer == null)
        {
            Debug.LogError("arPointer is not set. Please ensure it is set in the inspector.");
        }

        if (arRaycastManager == null)
        {
            Debug.LogError("ARRaycastManager is not found in the scene. Please ensure it is added.");
        }

        if (GameManager.instance != null)
        {
            GameManager.instance.OnMainMenu += SetItemPosition;
        }
        else
        {
            Debug.LogError("GameManager instance is not set. Please ensure GameManager is initialized.");
        }
    }

    private void Update()
    {
        HandleInitialPosition();
        HandleTouchInput();
    }

    private void HandleInitialPosition()
    {
        if (isInitialPosition)
        {
            Vector2 middlePointScreen = new Vector2(Screen.width / 2, Screen.height / 2);
            if (arRaycastManager.Raycast(middlePointScreen, hits, TrackableType.Planes))
            {
                if (hits.Count > 0)
                {
                    transform.position = hits[0].pose.position;
                    transform.rotation = hits[0].pose.rotation;

                    arPointer.SetActive(true);
                    isInitialPosition = false;
                    Debug.Log("Initial position set.");
                }
            }
        }
    }

    private void HandleTouchInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touchOne = Input.GetTouch(0);

            if (touchOne.phase == TouchPhase.Began)
            {
                Vector2 touchPosition = touchOne.position;
                isOverUI = IsTouchOverUI(touchPosition);
                isOver3DModel = IsTouchOver3DModel(touchPosition);
                Debug.Log($"Touch began. Over UI: {isOverUI}, Over 3DModel: {isOver3DModel}");
            }

            if (touchOne.phase == TouchPhase.Moved && !isOverUI && isOver3DModel)
            {
                if (arRaycastManager.Raycast(touchOne.position, hits, TrackableType.Planes))
                {
                    if (hits.Count > 0)
                    {
                        Pose hitPose = hits[0].pose;
                        transform.position = hitPose.position;
                        Debug.Log("Touch moved. Position updated.");
                    }
                }
            }

            if (Input.touchCount == 2)
            {
                Touch touchTwo = Input.GetTouch(1);

                if (touchOne.phase == TouchPhase.Began || touchTwo.phase == TouchPhase.Began)
                {
                    initialTouchPos = touchTwo.position - touchOne.position;
                }

                if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
                {
                    Vector2 currentTouchPos = touchTwo.position - touchOne.position;
                    float angle = Vector2.SignedAngle(initialTouchPos, currentTouchPos);
                    if (item3DModel != null)
                    {
                        item3DModel.transform.rotation = Quaternion.Euler(0, item3DModel.transform.eulerAngles.y - angle, 0);
                        initialTouchPos = currentTouchPos;
                        Debug.Log("Two-finger touch moved. Rotation updated.");
                    }
                }
            }

            if (isOver3DModel && item3DModel == null && !isOverUI)
            {
                if (GameManager.instance != null)
                {
                    GameManager.instance.ARPosition();
                    item3DModel = itemSelected;
                    itemSelected = null;
                    arPointer.SetActive(true);
                    transform.position = item3DModel.transform.position;
                    item3DModel.transform.parent = arPointer.transform;
                    Debug.Log("3D model selected and positioned.");
                }
            }
        }
    }

    private bool IsTouchOver3DModel(Vector2 touchPosition)
    {
        Ray ray = arCamera.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit3DModel))
        {
            if (hit3DModel.collider.CompareTag("Item"))
            {
                itemSelected = hit3DModel.transform.gameObject;
                return true;
            }
        }

        return false;
    }

    private bool IsTouchOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current) { position = touchPosition };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        return results.Count > 0;
    }

    private void SetItemPosition()
    {
        if (item3DModel != null)
        {
            item3DModel.transform.parent = null;
            arPointer.SetActive(false);
            item3DModel = null;
            Debug.Log("Item position reset.");
        }
    }

    public void DeleteItem()
    {
        if (item3DModel != null)
        {
            Destroy(item3DModel);
            arPointer.SetActive(false);
            if (GameManager.instance != null)
            {
                GameManager.instance.MainMenu();
            }
            Debug.Log("Item deleted.");
        }
    }

    public void ToggleMeasurementTool()
    {
        if (item3DModel != null)
        {
            var measurementTool = item3DModel.transform.GetChild(0).gameObject;
            measurementTool.SetActive(!measurementTool.activeSelf);
            Debug.Log("Measurement tool toggled.");
        }
    }
}
