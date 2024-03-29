using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region
    public float speed;
    public GameObject muzzlePrefabs;
    public GameObject hitPrefabs;
    protected float _maxDistance;

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    public bool collided;
    public Rigidbody _rigidbody;

    [HideInInspector]
    public AttackBehavior attackBehavior;
    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public GameObject target;
    public int projectileDamage;

    #endregion

    public void Init()
    {
        if (target != null)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }

        collided = false;
        _maxDistance = 200f;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = true;
        if (owner)
        {
            Collider[] ownerCollider = owner.GetComponentsInChildren<Collider>();

            foreach (Collider collider in ownerCollider)
            {
                //투사체와 발사한 유닛이 충돌하지 않도록
                Physics.IgnoreCollision(projectileCollider, collider);
            }
        }

        _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
        _rigidbody.isKinematic = false;

        if (muzzlePrefabs)
        {
            Managers.Effect.Instantiate(muzzlePrefabs, gameObject.transform.position, Quaternion.identity);
        }

        if (shotSFX != null)
        {
            AudioSource source = gameObject.GetOrAddComponent<AudioSource>();
            source.PlayOneShot(shotSFX);

        }
    }

    protected virtual void Start()
    {
        Init();
    }
    //ridgebody를 사용할 땐 픽스드 업데이트로
    protected virtual void FixedUpdate()
    {
        if(speed != 0)
        {
            _rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            _rigidbody.position += (transform.forward) * (speed * Time.deltaTime);
            _maxDistance -= speed * Time.deltaTime;
            if (_maxDistance < 0)
                Managers.Resource.Destroy(gameObject);
        }
    }


    

    private void OnCollisionEnter(Collision collision)
    {
        if(collided)
        {
            return;
        }

        collided = true;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        if(hitSFX != null)
        {
            AudioSource source = gameObject.GetOrAddComponent<AudioSource>();
            source.PlayOneShot(hitSFX);
        }
        _rigidbody.isKinematic = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPosition = contact.point;

        if(hitPrefabs)
        {
            Managers.Effect.Instantiate(hitPrefabs, contactPosition, contactRotation);
        }

        IDamageable damageable = collision.gameObject.GetComponent<IDamageable>();
        if(damageable != null)
        {
            damageable.TakeDamage(projectileDamage, null, owner);
        }

        Managers.Resource.Destroy(gameObject);
    }



    public IEnumerator CoDestroyProjectile(float waitTime)
    {
        if(transform.childCount > 0 &&  waitTime != 0)
        {
            List<Transform> childs = new List<Transform>();

            foreach (Transform t in  transform.GetChild(0).transform)
            {
                childs.Add(t);
            }
            while(transform.GetChild(0).localScale.x >0)
            {
                yield return new WaitForSeconds(0.01f);

                transform.GetChild(0).localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                for (int i = 0; i < childs.Count; i ++)
                {
                    childs[i].localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                }
            }

        }

        yield return new WaitForSeconds(waitTime);
        Managers.Resource.Destroy(gameObject);
    }
}
