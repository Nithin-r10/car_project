using UnityEngine;

public class RaycastCarController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody carRB;
    
    [Header("Suspension Properties")]
    [SerializeField] private float springLength = 1f; // Max suspension extension
    [SerializeField] private float springStrength = 50f; // Upward force strength
    [SerializeField] private float springDamping = 5f; // Dampen bouncing
    [SerializeField] private Transform[] tireTransforms; // Points where rays are cast from

    [Header("Movement Properties")]
    [SerializeField] private float motorTorque = 1000f;
    [SerializeField] private float steeringStrength = 3f;

    private float horizontalInput;
    private float verticalInput;
    [SerializeField]
    float tireGrip = 1f; // Adjust this for more or less grip

    private void Awake()
    {
    //    carRB = GetComponent<Rigidbody>();
        if (carRB == null)
        {
            Debug.LogError("Car GameObject needs a Rigidbody component!");
        }
    }

    private void Update()
    {
        // Get player input
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        Debug.Log(verticalInput);
    }

    private void FixedUpdate()
    {
        // Apply suspension force and movement in FixedUpdate for physics consistency
        for (int i = 0; i < tireTransforms.Length; i++)
        {
            ApplySuspensionAndMovement(tireTransforms[i]);
        }
    }

    void ApplySuspensionAndMovement(Transform tireTransform)
    {
        // Cast a ray downward from each tire position
        if (Physics.Raycast(tireTransform.position, -tireTransform.up, out RaycastHit hit, springLength))
        {
            // Draw a debug line in the Scene view to visualize the raycast
            Debug.DrawLine(tireTransform.position, hit.point, Color.red);

            // Calculate suspension force
            Vector3 springDir = tireTransform.up;
            Vector3 tireWorldVelocity = carRB.GetPointVelocity(tireTransform.position);
            
            // Distance calculations
            float groundDistance = hit.distance;
            float offset = springLength - groundDistance;
            
            // Velocity calculations
            float velocity = Vector3.Dot(springDir, tireWorldVelocity);
            
            // Force calculation (Hooke's law + damping)
            float force = (offset * springStrength) - (velocity * springDamping);
            
            // Apply suspension force to the car's Rigidbody at the wheel's position
            carRB.AddForceAtPosition(springDir * force, tireTransform.position);
            carRB.AddForceAtPosition(transform.forward * verticalInput * motorTorque , transform.position);

            // Apply motor force
          
            
                
            
            
            // Apply steering force (sideways friction)
            // Vector3 sidewaysVelocity = Vector3.ProjectOnPlane(tireWorldVelocity, tireTransform.forward);
            // carRB.AddForceAtPosition(-sidewaysVelocity * steeringStrength, tireTransform.position);
            Vector3 SteeringDir = tireTransform.right * horizontalInput; // Assuming right is the direction for steering
            Vector3 TireWorldVel = carRB.GetPointVelocity(tireTransform.position);
            float SteeringVelocity = Vector3.Dot(SteeringDir, TireWorldVel);
            float desireVelchange = - SteeringVelocity * tireGrip;
            float desireAccel = desireVelchange / Time.fixedDeltaTime;
            carRB.AddForceAtPosition(SteeringDir * desireAccel, tireTransform.position);
        }
        else
        {
            // Draw a green line if the ray doesn't hit anything within range
            Debug.DrawLine(tireTransform.position, tireTransform.position - tireTransform.up * springLength, Color.green);
        }

        // // Simple steering (affects the car's rotation directly for an arcade feel)
        // transform.Rotate(Vector3.up, horizontalInput * steeringStrength * Time.fixedDeltaTime);
    }
}
