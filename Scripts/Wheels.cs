using UnityEngine;

public class Wheels : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private float WheelRadius;
    [SerializeField]
    private float springstiffness;
    [SerializeField]
    private float resetlength;
    [SerializeField]
    private float springTravel;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
   
    void FixedUpdate()
    {
        raycast();
    }
    void raycast()
    {
        float Maxlength = resetlength + springTravel;
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hit, Maxlength + WheelRadius))
        {
           float currentspringlength = hit.distance - WheelRadius;
           float springCompression = resetlength - currentspringlength/ springTravel;
           float springforce = springCompression * springstiffness;
           rb.AddForceAtPosition(transform.up * springforce, transform.position);
           Debug.DrawLine(transform.position, hit.point, Color.red);
        }
    }
}
