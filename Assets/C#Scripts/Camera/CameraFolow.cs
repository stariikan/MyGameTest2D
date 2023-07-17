using UnityEngine;

public class CameraFolow : MonoBehaviour
{
    private Transform _playerTransform;
    public string objectNameToFind;
    [SerializeField] private float _movingSpeed;

    // Start is called before the first frame update
    private void Start()
    {
        GameObject obj = GameObject.Find("Hero");
        _playerTransform = obj.transform;
        this.transform.position = new Vector3()
        {
            x = this._playerTransform.position.x,
            y = this._playerTransform.position.y
        };
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(this._playerTransform)
        {
            Vector3 target = new Vector3()
            {
                x = this._playerTransform.position.x,
                y = this._playerTransform.position.y + 4f

            };
            Vector3 pos = Vector3.Lerp(this.transform.position, target, this._movingSpeed * Time.deltaTime);
            this.transform.position = pos;
        }
    }
}
