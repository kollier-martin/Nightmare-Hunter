using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Santa : MonoBehaviour
{
    [SerializeField] private GameObject Present;
    [SerializeField] private Transform ProjectileField;

    private Rigidbody2D rb;
    private List<PolygonCollider2D> polygons;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        polygons = new List<PolygonCollider2D>();

        var polys = GetComponents<PolygonCollider2D>();

        foreach (PolygonCollider2D poly in polys)
        {
            polygons.Add(poly);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
