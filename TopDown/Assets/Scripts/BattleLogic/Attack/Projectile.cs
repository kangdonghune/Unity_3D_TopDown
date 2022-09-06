using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    #region
    public float speed;
    public GameObject muzzlePrefabs;
    public GameObject hitPrefabs;

    public AudioClip shotSFX;
    public AudioClip hitSFX;

    private bool _collided;
    private Rigidbody rigidbody;

    [HideInInspector]
    public AttackBehavior attackBehavior;
    [HideInInspector]
    public GameObject owner;
    [HideInInspector]
    public GameObject target;
    #endregion

    private void Start()
    {
        if(target != null)
        {
            Vector3 dest = target.transform.position;
            dest.y += 1.5f;
            transform.LookAt(dest);
        }

        if(owner)
        {
            Collider projectileCollider = GetComponent<Collider>();
            Collider[] ownerCollider = owner.GetComponentsInChildren<Collider>();

            foreach (Collider collider in ownerCollider)
            {
                //����ü�� �߻��� ������ �浹���� �ʵ���
                Physics.IgnoreCollision(projectileCollider, collider);
            }
        }

        if (muzzlePrefabs)
        {
            GameObject muzzleVFX = Managers.Resource.Instantiate("VFX/VFX_Boom", transform);
            muzzleVFX.transform.forward = gameObject.transform.forward;
            ParticleSystem particleSystem = muzzleVFX.GetComponent<ParticleSystem>();
            if(particleSystem)
            {
                //��ƼŬ �ֱⰡ ������ �� �ڵ����� ������ �ȵǴ� ��� �ڵ�� ����
                Managers.Resource.Destroy(muzzleVFX, particleSystem.main.duration);
            }
            else
            {
                ParticleSystem childParticleSystem = muzzleVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childParticleSystem)
                {
                    Managers.Resource.Destroy(muzzleVFX, childParticleSystem.main.duration);
                }
            }
        }

        if(shotSFX != null)
        {
            AudioSource source = gameObject.GetOrAddComponent<AudioSource>();
            source.PlayOneShot(shotSFX);
            
        }
    }
    //ridgebody�� ����� �� �Ƚ��� ������Ʈ��
    private void FixedUpdate()
    {
        if(speed != 0)
        {
            rigidbody = gameObject.GetOrAddComponent<Rigidbody>();
            rigidbody.position += (transform.forward) * (speed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(_collided)
        {
            return;
        }

        _collided = true;

        Collider projectileCollider = GetComponent<Collider>();
        projectileCollider.enabled = false;

        if(hitSFX != null)
        {
            AudioSource source = gameObject.GetOrAddComponent<AudioSource>();
            source.PlayOneShot(hitSFX);
        }

        speed = 0;
        rigidbody.isKinematic = true;

        ContactPoint contact = collision.contacts[0];
        Quaternion contactRotation = Quaternion.FromToRotation(Vector3.up, contact.normal);
        Vector3 contactPosition = contact.point;

        if(hitPrefabs)
        {
            //Managers.Resource.Instantiate(hitPrefabs);
            GameObject hitVFX = Instantiate(hitPrefabs, contactPosition, contactRotation);

            ParticleSystem particleSystem = hitVFX.GetComponent<ParticleSystem>();
            if (particleSystem)
            {
                //��ƼŬ �ֱⰡ ������ �� �ڵ����� ������ �ȵǴ� ��� �ڵ�� ����
                Managers.Resource.Destroy(hitVFX, particleSystem.main.duration);
            }
            else
            {
                ParticleSystem childParticleSystem = hitVFX.transform.GetChild(0).GetComponent<ParticleSystem>();
                if (childParticleSystem)
                {
                    Managers.Resource.Destroy(hitVFX, childParticleSystem.main.duration);
                }
            }
        }


    }
}
