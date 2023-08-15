using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
public class SmallMeteor : MonoBehaviour
{
    Transform trail;
    ParticleSystem particle;
    AudioSource aud;
    public AudioClip clip;
    private void Start()
    {
        trail = transform.root.GetChild(1);
        particle = transform.root.GetChild(2).GetComponent<ParticleSystem>();
        aud = GetComponent<AudioSource>();
    }
    private void Update()
    {
        trail.position = transform.position;
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Boom"))
        {
            MarchingCubes.Instance.MakeGround(collision.contacts[0].point, 1.5f);
            Destroy(collision.gameObject);
        }
        else
        {
            MarchingCubes.Instance.MakeHole(collision.contacts[0].point, 1.5f);
        }
        aud.loop = false;
        aud.clip = clip;
        aud.volume = 0.7f;
        aud.Play();
        Destroy(transform.root.gameObject, 5);
        trail.GetComponent<VisualEffect>().Stop();
        particle.transform.position = transform.position;
        particle.Play();
        Destroy(gameObject.GetComponent<MeshFilter>());
        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject.GetComponent<SphereCollider>());
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}
