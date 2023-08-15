using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public AudioClip[] clips;
    AudioSource aud;
    public GameObject sphere;
    private void Start()
    {
        transform.rotation = Quaternion.identity;
        aud = GetComponent<AudioSource>();
    }
    public void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.CompareTag("Boom"))
            {
                MarchingCubes.Instance.MakeGround(collision.contacts[0].point, 4);
                transform.GetChild(1).GetComponent<ParticleSystem>().Play();
                Destroy(GetComponent<SphereCollider>());
                Destroy(GetComponent<Rigidbody>());
                transform.GetChild(0).gameObject.SetActive(false);
                aud.clip = clips[0];
                aud.Play();
                Destroy(gameObject, 5);
                Instantiate(sphere, collision.contacts[0].point, Quaternion.identity);
                StopAllCoroutines();
            }
            else
            {
                StartCoroutine(Explosion());
            }
        }
    }
    IEnumerator Explosion()
    {
        yield return new WaitForSeconds(1);
        MarchingCubes.Instance.MakeHole(transform.position, 4);
        transform.GetChild(2).GetComponent<ParticleSystem>().Play();
        Destroy(GetComponent<SphereCollider>());
        Destroy(GetComponent<Rigidbody>());
        aud.clip = clips[1];
        aud.Play();
        transform.GetChild(0).gameObject.SetActive(false);
        Destroy(gameObject, 5);
    }
}
