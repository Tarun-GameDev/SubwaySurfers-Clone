using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FractureExplode : MonoBehaviour
{
    [Header("Assignables:")]
    [SerializeField] GameObject explosion;
    [SerializeField] Vector3 explosionOffset;
    [SerializeField] GameObject smoke;
    [Header("Forces To Explode:")]
    [SerializeField] int maxSmokes;
    [SerializeField] float minForce;
    [SerializeField] float maxForce;
    [SerializeField] float radius;
    float destroytime = 6f;

    public void Start()
    {
        Explode();
    }

    public void Explode()
    {
        #region Exploding himself

        int smokeCounter = 0;

        if (explosion != null)
        {
            GameObject explosionFx = Instantiate(explosion, transform.position + explosionOffset, Quaternion.identity) as GameObject;
            Destroy(explosionFx, 5f);
        }

        foreach (Transform t in transform)
        {
            var rb = t.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(Random.Range(minForce, maxForce), transform.position, radius);
            }

            if (smoke != null && smokeCounter < maxSmokes)
            {
                GameObject smokeFX = Instantiate(smoke, t.transform) as GameObject;
                smokeCounter++;
                Destroy(smokeFX, 5f);
                Destroy(gameObject, 6f);
                if (Random.Range(1, 4) == 1)
                {

                }
            }
            Destroy(t.gameObject, destroytime);
        }
        #endregion
    }
}
