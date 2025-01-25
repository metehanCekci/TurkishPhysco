using UnityEngine;
using UnityEngine.UI;

public class SpeedText : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] public Text Text;
    Rigidbody rb;
    private float currentSpeed;
    
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontalVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        currentSpeed = horizontalVelocity.magnitude;
        Text.text = currentSpeed.ToString("F2") + " M/s";
    }
}
