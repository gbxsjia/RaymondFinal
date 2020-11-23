using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoShared;

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
			print(transform.position);
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
		if (Camera.main == null || !isReady)
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

            //float angle = (Mathf.Atan2 (v1.y - v2.y, v1.x - v2.x) * 180.0f / Mathf.PI) + 180.0f;
            //if (firstLaunch)
            //	angle = 0f;

            //if (prevAngle == 361) {
            //	prevAngle = angle;
            //}

            //if (autoOrbit) {

            //	prevAngle = angle;
            //	currentAngle += orbitSpeed;

            //            } else if (rotateWithHeading) {

            //                Input.compass.enabled = true;
            //                currentAngle = Input.compass.trueHeading;

            //            } else if (angle != prevAngle) {

            //	float delta = angle - prevAngle;
            //	if (delta > 180.0f) {
            //		delta -= 360;
            //	} else if (delta < -180.0f) {
            //		delta += 360;
            //	}
            //	prevAngle = angle;
            //	currentAngle += delta * orbitSpeed;
            //}
      
			Vector3 deltaLocation = new Vector3(LastMousePosition.x - newPos.x, 0, LastMousePosition.y - newPos.y) * Time.deltaTime; 
			if (firstLaunch)
			{
				deltaLocation = Vector3.zero;
			}
			LocationManager.instance.AddLocation(deltaLocation);
			transform.position = LocationManager.instance.currentLocation.convertCoordinateToVector(0);

			//print("first: " + firstLaunch + " loc: " + LocationManager.instance.currentLocation.convertCoordinateToVector(0));
			LastMousePosition =newPos;
			firstLaunch = false;
		}
        else
        {
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


		//if (clipPlane != null && clipPlane.IsAboutToClip(false))
		//{

		//	distance += deltaD + 2;
		//}


		//float height = EvaluateCurrentHeight(distance);


		//Quaternion rotation = Quaternion.Euler(height, currentAngle, 0);
		//Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
		//Vector3 position = rotation * negDistance + target.position;

		//objToRotate.rotation = rotation * Quaternion.Euler(-offset, 0, 0);
		//objToRotate.position = position;

	}

	float EvaluateCurrentHeight(float currentDistance)
	{

		float convValue = (distance - distanceMin) / (distanceMax - distanceMin);
		float factor = zoomCurve.Evaluate(convValue);

		float height = factor * (yMaxLimit - yMinLimit) + yMinLimit;

		return height;

	}
}
