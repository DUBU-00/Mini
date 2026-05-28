using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private Vector3 offset;

    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private Camera cam;
    private Vector3 currentVelocity = Vector3.zero;
    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void Start()
    {
        if (target == null)
        {
            PlayerStats player = FindAnyObjectByType<PlayerStats>();

            if (player != null)
            {
                target = player.transform;
            }
        }

        MoveInstant();
    }

    void LateUpdate()
    {
        FollowCamera();
    }

    private void FollowCamera()
    {
        if (target == null) return;

        Vector3 finalPosition = GetClampPosition();

        transform.position = Vector3.SmoothDamp(transform.position,finalPosition,ref currentVelocity, smoothTime);
    }

    public void MoveInstant()
    {
        if (target == null) return;

        transform.position = GetClampPosition();
        currentVelocity = Vector3.zero;
    }

    private Vector3 GetClampPosition()
    {
        Vector3 targetPosition = target.position + offset;

        float clampX = Mathf.Clamp(targetPosition.x,minX,maxX);

        float clampY = Mathf.Clamp(targetPosition.y,minY,maxY);

        return new Vector3(clampX,clampY, offset.z == 0 ? -10f : offset.z);
    }
    public void SetBounds(float newMinX, float newMaxX, float newMinY, float newMaxY)
    {
        minX = newMinX;
        maxX = newMaxX;
        minY = newMinY;
        maxY = newMaxY;
    }
}