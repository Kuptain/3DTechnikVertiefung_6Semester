using UnityEngine;
using System.Collections;

public class ThirdPersonSpaceWalker : MonoBehaviour
{

    public Transform LookTransform;

    public float speed = 6.0f;
    public float maxVelocityChange = 10.0f;
    public float jumpForce = 5.0f;
    public float GroundHeight = 1.1f;
    private bool canJump = true;
    private bool grounded = true;

    public GameObject cameraHolder;
    public GameObject bridgeObject;
    public float lookSpeed = 1f;
    float rotationX = 60f;
    public float lookXLimit = 90;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //RaycastHit groundedHit;
        //bool grounded = Physics.Raycast(transform.position, -transform.up, out groundedHit, GroundHeight);


        // Calculate how fast we should be moving
        Vector3 forward = Vector3.Cross(transform.up, -LookTransform.right).normalized;
        Vector3 right = Vector3.Cross(transform.up, LookTransform.forward).normalized;
        Vector3 targetVelocity = (forward * Input.GetAxis("Vertical") + right * Input.GetAxis("Horizontal")) * speed;

        Vector3 velocity = transform.InverseTransformDirection(GetComponent<Rigidbody>().velocity);
        velocity.y = 0;
        velocity = transform.TransformDirection(velocity);
        Vector3 velocityChange = transform.InverseTransformDirection(targetVelocity - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
        velocityChange.y = 0;
        velocityChange = transform.TransformDirection(velocityChange);

        GetComponent<Rigidbody>().AddForce(velocityChange, ForceMode.VelocityChange);
        if (grounded && canJump)
        {
            if (Input.GetButton("Jump"))
            {
                GetComponent<Rigidbody>().AddForce(transform.up * jumpForce, ForceMode.VelocityChange);
                StartCoroutine(JumpCooldown());
            }
        }
    }
    IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(0.4f);
        canJump = true;
    }
    private void OnCollisionEnter(Collision collision)
    {
        grounded = true;
        if (collision.gameObject.tag == "planet")
        {          
            transform.SetParent(null);
        }
        if (collision.gameObject.tag == "bridge")
        {
            grounded = true;
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "planet" || collision.gameObject.tag == "bridge")
        {
            grounded = false;
        }
        if (collision.gameObject.tag == "bridge")
        {
            transform.SetParent(null);
        }
    }
    private void Update()
    {
        //cameraHolder.transform.position = Vector3.Lerp(cameraHolder.transform.position, transform.position, 0.2f);
        rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
        rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
        cameraHolder.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
        transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }
}
