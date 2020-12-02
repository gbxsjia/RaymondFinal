using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;
using UnityEngine.EventSystems;

public class CameraAvatar : MonoBehaviour
{
	float prevPinchDist = 0f;

	public float distance = 55.0f;

	public AnimationCurve zoomCurve;

	public float yMinLimit = 20f;
	public float yMaxLimit = 60f;

	public float distanceMin = 20f;
	public float distanceMax = 200f;

	private Vector3 LastMousePosition;
	private bool firstLaunch = true;
	private bool isReady = false;
	public bool canControl=true;

	public float DragSpeed;

	public GameObject projectorPrefab;
	private GameObject currentProjector;

	public GameObject waypointPrefab;

	private float ClickTimer;

	public static CameraAvatar instance;
	public Transform CameraTransform;
    private void Awake()
    {
		instance = this;
    }
    private void Start()
    {
		StartCoroutine(LoadStartPosition());
	}
	private IEnumerator LoadStartPosition()
    {
		while (LocationManager.instance.currentLocation.convertCoordinateToVector(0).magnitude> 1000f)
        {
			yield return null;
		}
		
        while (Vector3.Distance(transform.position, LocationManager.instance.currentLocation.convertCoordinateToVector(0))>1f)
        {
			transform.position = Vector3.Lerp(transform.position, LocationManager.instance.currentLocation.convertCoordinateToVector(0), 0.1f);
			yield return null;
		}
		isReady = true;
		transform.position = LocationManager.instance.currentLocation.convertCoordinateToVector(0);
	}
    private void Update()
    {
		updateOrbit();
    }

    void updateOrbit()
	{
		if (Camera.main == null || !isReady || !canControl)
			return;

		bool drag = false;

		Vector3 newPos = Vector3.zero;
		if (Application.isMobilePlatform)
		{
			drag = Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved;
			if (drag)
				newPos = Input.GetTouch(0).position;
		}
		else
		{
			drag = Input.GetMouseButton(0);
			if (drag)
				newPos = Input.mousePosition;
		}

		if (drag)
		{
			ClickTimer += Time.deltaTime;
			Vector3 deltaLocation = new Vector3(LastMousePosition.x - newPos.x, 0, LastMousePosition.y - newPos.y) * DragSpeed * Time.deltaTime; 
			if (firstLaunch)
			{
				deltaLocation = Vector3.zero;
			}
			transform.position += deltaLocation;
			LocationManager.instance.UpdateLocation(transform.position);
			//print("first: " + firstLaunch + " loc: " + LocationManager.instance.currentLocation.convertCoordinateToVector(0));
			LastMousePosition =newPos;
			firstLaunch = false;
		}
        else
        {
			if(ClickTimer>0 && ClickTimer < 0.2f)
            {
				Click();
            }
			ClickTimer = 0;
			firstLaunch = true;
        }


		float deltaD = 0;
		if (Application.isMobilePlatform)
		{
			if (Input.touchCount >= 2)
			{
				Vector2 touch0, touch1;
				float d;
				touch0 = Input.GetTouch(0).position;
				touch1 = Input.GetTouch(1).position;
				d = Mathf.Abs(Vector2.Distance(touch0, touch1));

				deltaD = Mathf.Clamp(prevPinchDist - d, -1, 1) * (distanceMax - distanceMin) / 25;  //pinchSpeed;
				prevPinchDist = d;

				distance = Mathf.Clamp(distance + deltaD, distanceMin, distanceMax);

			}
		}
		else
		{

			deltaD = Input.GetAxis("Mouse ScrollWheel") * (distanceMax - distanceMin) / 25;
			float newD = distance - deltaD;
			distance = Mathf.Clamp(newD, distanceMin, distanceMax);

		}
	}

	private void Click()
	{
        if (EventSystem.current.currentSelectedGameObject!=null)
		{
			return;
        }

		RaycastHit hit;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit, Mathf.Infinity, GoMap.GOMap.GetActiveMasks()))
		{

			//From the raycast data it's easy to get the vector3 of the hit point 
			Vector3 worldVector = hit.point;
			//And it's just as easy to get the gps coordinate of the hit point.
			Coordinates gpsCoordinates = Coordinates.convertVectorToCoordinates(hit.point);

			if (currentProjector != null)
				Destroy(currentProjector);

			//Add a simple projector to the tapped point
			currentProjector = Instantiate(projectorPrefab);
			worldVector.y += 3f;
			currentProjector.transform.position = worldVector;

            switch (UIManager.instance.mainState)
            {
				case MainStateType.PlanProcess:
					GameObject wp = Instantiate(waypointPrefab, worldVector, Quaternion.identity);
					break;
            }
			
		}
	}
	float EvaluateCurrentHeight(float currentDistance)
	{

		float convValue = (distance - distanceMin) / (distanceMax - distanceMin);
		float factor = zoomCurve.Evaluate(convValue);

		float height = factor * (yMaxLimit - yMinLimit) + yMinLimit;

		return height;

	}

	public void MoveCamera(Vector3 TargetPosition, float duration)
    {
		StartCoroutine(MoveCameraProcess(TargetPosition, duration));
    }
	private IEnumerator MoveCameraProcess(Vector3 TargetPosition, float duration)
	{
		canControl = false;
		Vector3 startPosition = CameraTransform.position;
		Quaternion startRotation = CameraTransform.rotation;
		float timer = duration;
		while (timer > 0)
		{
			CameraTransform.position = Vector3.Lerp(startPosition, TargetPosition, 1 - timer / duration);
			CameraTransform.rotation = Quaternion.Slerp(startRotation, Quaternion.Euler(90, 0, 0), 1 - timer / duration);
			print(Vector3.Lerp(startPosition, TargetPosition, 1 - timer / duration));
			timer -= Time.deltaTime;
			yield return null;
		}

		canControl = true;
	}
}
