using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    private float cameraSpeed;
    private bool right;
    private void Start()
    {
        transform.position = new Vector3(4.69f, 0, 0);
        right = true;
        cameraSpeed = 0.0005f;
    }
    private void Update()
    {
        ChangeDirection();
        CameraMove();
    }
    private void ChangeDirection() //смена направленеие когда до конца картин доходит камера
    {
        if (transform.position.x < 222)
        {
            right = false;
        }
        if (transform.position.x > 4)
        {
            right = true;
        }
    }
    private void CameraMove() //движение камеры
    {
        Vector3 targetRight = new Vector3()
        {
            x = 223
        };
        Vector3 targetLeft = new Vector3()
        {
            x = 0
        };
        if (right)
        {
            Vector3 pos = Vector3.Lerp(transform.position, targetRight, cameraSpeed * Time.fixedDeltaTime);
            transform.position = pos;
        }
        if (!right)
        {
            Vector3 pos = Vector3.Lerp(transform.position, targetLeft, cameraSpeed * Time.fixedDeltaTime);
            transform.position = pos;
        }
    }
}