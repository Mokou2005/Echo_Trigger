using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptTest : MonoBehaviour
{
    private GameObject Parent;

    private Vector3 Position;

    private RaycastHit Hit;

    private float Distance;

    private int Mask;

    void Start()
    {
        Parent = transform.root.gameObject;

        Position = transform.localPosition;

        Distance = Vector3.Distance(Parent.transform.position, transform.position);

        Mask = ~(1 << LayerMask.NameToLayer("Player"));
    }

    void Update()
    {
        if (Physics.CheckSphere(Parent.transform.position, 0.3f, Mask))
        {
            transform.position = Vector3.Lerp(transform.position, Parent.transform.position, 1);
        }
        else if (Physics.SphereCast(Parent.transform.position, 0.3f, (transform.position - Parent.transform.position).normalized, out Hit, Distance, Mask))
        {
            transform.position = Parent.transform.position + (transform.position - Parent.transform.position).normalized * Hit.distance;
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Position, 1);
        }
    }
}