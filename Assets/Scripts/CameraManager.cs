using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private float smoothSpeed = 5f;

    [SerializeField]
    private Vector3 offset;

    [SerializeField]
    private float minX;

    [SerializeField]
    private float maxX;

    [SerializeField]
    private float minY;

    [SerializeField]
    private float maxY;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        if (target == null) return;

        Vector3 finalPosition = GetClampPosition();

        transform.position = Vector3.Lerp(
            transform.position,
            finalPosition,
            smoothSpeed * Time.deltaTime
        );
    }

    public void MoveInstant()
    {
        if (target == null) return;

        transform.position = GetClampPosition();
    }

    private Vector3 GetClampPosition()
    {
        Vector3 targetPosition = target.position + offset;

        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float clampX = Mathf.Clamp(
            targetPosition.x,
            minX + camWidth,
            maxX - camWidth
        );

        float clampY = Mathf.Clamp(
            targetPosition.y,
            minY + camHeight,
            maxY - camHeight
        );

        return new Vector3(
            clampX,
            clampY,
            offset.z
        );
    }
        public void SetBounds(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
    }
}