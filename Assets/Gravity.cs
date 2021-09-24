using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour
{

    //credit some: podperson

    public Transform planet;
    public bool AlignToPlanet;
    private float gravityConstant = 9.8f;

    void Start()
    {

    }

    void FixedUpdate()
    {
        Vector3 toCenter = planet.position - transform.position;
        toCenter.Normalize();

        GetComponent<Rigidbody>().AddForce(toCenter * gravityConstant, ForceMode.Acceleration);

        if (AlignToPlanet)
        {
            Quaternion q = Quaternion.FromToRotation(transform.up, -toCenter);
            q = q * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, q, 1);
        }
    }
}
