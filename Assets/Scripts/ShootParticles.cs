using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootParticles : MonoBehaviour
{
    public GameObject explosion;
    private Animator anim;
    private AudioSource audioSource;
    //private ParticleSystem particles;

    private Vector3 position, target;
    private Quaternion rotation;
    private float nextFire, cooldown_time = 0.1f;

    [SerializeField] ParticleSystem particles = null;


    void Start()
    {
        anim = this.GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
        nextFire = Time.time + cooldown_time;
    }

    void Update()
    {
        // use mouse left button or space key to shoot
        //if (Input.GetMouseButtonDown(0) && Time.time > nextFire)        
        if (Input.GetKey(KeyCode.Space) && Time.time > nextFire)
        {
            nextFire = Time.time + cooldown_time;
            position = this.transform.position;
            position.y = 2;   // fire from soldier gun at shoulder position
            rotation = this.transform.rotation;

            // translate mouse position to world 3d position
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
                target = hitInfo.point;
            else
            {
                float maxdist = 100;
                float x = Input.mousePosition.x;
                float y = Input.mousePosition.y;
                target = Camera.main.ScreenToWorldPoint(new Vector3(x, y, maxdist));
            }

            Shoot();
        }
    }
    private void Shoot()
    {
        // play shooting animation one time
        anim.Play("demo_combat_shoot");
        audioSource.Play();
        //particles.Play();

        Vector3 direction = (target - position);
        direction = direction.normalized;
        var rotation = Quaternion.LookRotation(direction);
        var psShape = particles.shape;
        psShape.rotation = rotation.eulerAngles - this.transform.rotation.eulerAngles;
        particles.Emit(10);
    }

    /*
    public void RaycastShoot()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RayCastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.tag == "Enemy") ;
            {
                CharacterStats enemystats = hit.transform.GetComponent<CharacterStats>();
                enemystats.TakeDamage();
            }
        }
            

    }
    */
    
    public void OnParticleCollision(GameObject other)
    {
        List<ParticleCollisionEvent> collisionEvents =
        new List<ParticleCollisionEvent>();
        int numCollisionEvents = particles.GetCollisionEvents(other,
        collisionEvents);
        for (int i = 0; i < numCollisionEvents; i++)
        {
            Vector3 pos = collisionEvents[i].intersection;
            GameObject exp = Instantiate(explosion, pos, Quaternion.identity);
        }
    }
    
}
