using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.1f, 0.2f, -10f);
    public bool usarLimites = true;
    public float minX = -260f;
    public float maxX = -180f;
    public float minY = -15f;
    public float maxY = 10f;

    private GameObject player;
    private Vector3 velocity = Vector3.zero;
    public float smoothTime = 0.25f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogWarning("❌ Player não encontrado!");
        else
            Debug.Log("✅ Player encontrado: " + player.name);
    }

    private void FixedUpdate()
{
    if (player == null)
    {
        player = GameObject.FindGameObjectWithTag("Player");
        return;
    }

    Vector3 playerPos = player.transform.position;
    Vector3 targetPosition = playerPos + offset;

    if (usarLimites)
    {
        float cameraHeight = Camera.main.orthographicSize;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        targetPosition.x = Mathf.Clamp(targetPosition.x, minX + cameraWidth, maxX - cameraWidth);
        targetPosition.y = Mathf.Clamp(targetPosition.y, minY + cameraHeight, maxY - cameraHeight);
    }

    targetPosition.z = offset.z;
    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
}


    // Gizmos para visualização dos limites no Editor
    private void OnDrawGizmosSelected()
    {
        if (!usarLimites) return;

        Gizmos.color = Color.yellow;

        // Posição e tamanho do retângulo de limites
        Vector3 centro = new Vector3((minX + maxX) / 2f, (minY + maxY) / 2f, 0f);
        Vector3 tamanho = new Vector3(Mathf.Abs(maxX - minX), Mathf.Abs(maxY - minY), 0f);

        Gizmos.DrawWireCube(centro, tamanho);
    }
}
