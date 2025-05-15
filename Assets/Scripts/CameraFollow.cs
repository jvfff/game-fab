using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //public Transform player;
    public Vector3 offset = new Vector3(0.1f, 0f, -10f);
    private Vector3 Player = new Vector3(0f, 0f, 0f);
    public Vector3 speed;
    GameObject player;
    private float smoothTime = 0.25f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
    void FixedUpdate()
    {
        if (player != null)
        {
            //transform.position = player.position + offset;
            Move();
        }

    }
    void Move()
    {
        Player = player.transform.position;
        offset.y = Player.y + 0.2f;
        offset.x = Player.x;
        Vector3 targetposition = offset;
        offset.z = -10f;
        transform.position = Vector3.SmoothDamp(transform.position, targetposition, ref speed, smoothTime);

    }
}

