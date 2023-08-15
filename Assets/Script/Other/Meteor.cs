using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.VFX;
public class Meteor : MonoBehaviour
{
    Transform trail;
    GameObject cylinder;
    ParticleSystem particle;
    public GameObject piece;
    public AudioClip clip;
    AudioSource aud;
    private void Start()
    {
        trail = transform.root.GetChild(1);
        cylinder = transform.root.GetChild(2).gameObject;
        particle = transform.root.GetChild(3).GetComponent<ParticleSystem>();
        aud = GetComponent<AudioSource>();
        StartCoroutine(Cut());
    }
    private void Update()
    {
        trail.position = transform.position;
    }
    IEnumerator Cut()
    {
        yield return new WaitForSeconds(1);
        cylinder.transform.DOScaleY(0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            cylinder.SetActive(false);
        });
    }
    public void OnCollisionEnter(Collision collision)
    {
        Destroy(transform.root.gameObject, 5);
        trail.GetComponent<VisualEffect>().Stop();
        particle.transform.position = transform.position;
        aud.loop = false;
        particle.Play();
        aud.clip = clip;
        aud.volume = 0.6f;
        aud.Play();
        if (collision.gameObject.CompareTag("Boom"))
        {
            Destroy(collision.gameObject);
            for(int i = 0; i < 15; i++)
            {
                Vector3 vec = Random.onUnitSphere * 0.1f;
                Rigidbody rigid = Instantiate(piece, collision.contacts[0].point + vec, Quaternion.identity).GetComponent<Rigidbody>();
                rigid.AddExplosionForce(100, transform.position, 1, 2);
            }
        }
        else
        {
            MarchingCubes.Instance.MakeHole(collision.contacts[0].point, 5);
        }
        Destroy(gameObject.GetComponent<MeshFilter>());
        Destroy(gameObject.GetComponent<MeshRenderer>());
        Destroy(gameObject.GetComponent<SphereCollider>());
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}
