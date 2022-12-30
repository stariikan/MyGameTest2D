using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private float _movingSpeed;

    private void Awake()
    {
        this.transform.position = new Vector3()
        {
            x = this._playerTransform.position.x,
            y = this._playerTransform.position.y + 3.2f,
            z = this._playerTransform.position.z - 5f
        };
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void FixedUpdate()
    {
        if(this._playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = this._playerTransform.position.x,
                y = this._playerTransform.position.y + 1.2f,
                z = this._playerTransform.position.z - 5f

            };
            Vector3 pos = Vector3.Lerp(this.transform.position, target, this._movingSpeed * Time.deltaTime);
            this.transform.position = pos;
        }
    }
}
