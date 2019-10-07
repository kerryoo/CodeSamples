using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    private readonly float CAMERA_LERP = 3f;
    private readonly float AIMING_OFFSET = 0.5f;
    private readonly Vector3 CAMERA_OFFSET = new Vector3(0, 0.25f, -2);
    private readonly float MAXIMUM_VERT = 30f;
    private readonly float MINIMUM_VERT = -10f;

    [SerializeField] MasterPlayer localPlayer;
    [SerializeField] InputControl inputControl;

    [SerializeField] Transform cameraLookTarget;
    [SerializeField] Transform aimingLookTarget;
    [SerializeField] GameObject crosshairObject;

    public float sensitivityVert;
    public float _rotationX;
    private Vector3 targetPosition;
    private Quaternion targetRotation;

    private void Update()
    {
        if (inputControl.Firing)
        {
            crosshairObject.GetComponent<Renderer>().enabled = true;

            targetPosition = cameraLookTarget.position + localPlayer.transform.forward * (CAMERA_OFFSET.z / 2) +
                localPlayer.transform.right * AIMING_OFFSET;

            targetRotation = Quaternion.LookRotation(aimingLookTarget.position - targetPosition, Vector3.up);

        }

        else
        {
            crosshairObject.GetComponent<Renderer>().enabled = false;

            targetPosition = cameraLookTarget.position + localPlayer.transform.forward * CAMERA_OFFSET.z +
                localPlayer.transform.up * CAMERA_OFFSET.y +
                localPlayer.transform.right * CAMERA_OFFSET.x;

            targetRotation = Quaternion.LookRotation(cameraLookTarget.position - targetPosition, Vector3.up);
        }


        transform.position = Vector3.Lerp(transform.position, targetPosition, CAMERA_LERP * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, CAMERA_LERP * Time.deltaTime);

        _rotationX -= Input.GetAxis("Mouse Y") * sensitivityVert;
        _rotationX = Mathf.Clamp(_rotationX, MINIMUM_VERT, MAXIMUM_VERT);

        Vector3 rotation = transform.localEulerAngles;
        rotation.x = _rotationX;

        transform.localEulerAngles = rotation;
    }
}