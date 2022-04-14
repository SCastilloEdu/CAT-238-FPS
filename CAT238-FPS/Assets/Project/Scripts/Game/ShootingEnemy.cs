using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Unity AI

public class ShootingEnemy : Enemy
{
    public float shootingInterval = 4f;
    public float shootingDistance = 6f;
    public float chasingInterval = 2f;
    public float chasingDistance = 12f;
	public int weaponType = 1;

    private Player player;
    private float shootingTimer;
    private float chasingTimer;
    private NavMeshAgent agent; // Unity AI
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        agent = GetComponent<NavMeshAgent>(); // Unity AI
        shootingTimer = Random.Range(0, shootingInterval);
        agent.SetDestination(player.transform.position); // Unity AI
    }

    // Update is called once per frame
    void Update()
    {
        if (player.Killed == true)
        {
            agent.enabled = false;
            this.enabled = false;
        }

        // Shooting Logic.
        shootingTimer -= Time.deltaTime;
        if (shootingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= shootingDistance)
        {
            shootingTimer = shootingInterval;

            GameObject bullet = ObjectPoolingManager.Instance.GetBullet(false);
            bullet.transform.position = transform.position;
            bullet.transform.forward = (player.transform.position - transform.position).normalized;
			if (weaponType == 2)
			{
				GameObject bulletObjectL = ObjectPoolingManager.Instance.GetBullet(false);
				bulletObjectL.transform.position = transform.position;
				bulletObjectL.transform.forward = (player.transform.position - transform.position).normalized;
				bulletObjectL.transform.Rotate(0.0f, -10.0f, 0.0f, Space.Self);
				GameObject bulletObjectR = ObjectPoolingManager.Instance.GetBullet(false);
				bulletObjectR.transform.position = transform.position;
				bulletObjectR.transform.forward = (player.transform.position - transform.position).normalized;
				bulletObjectR.transform.Rotate(0.0f, 10.0f, 0.0f, Space.Self);
			}

        }

        // Chasing Logic.
        chasingTimer -= Time.deltaTime;
        if (chasingTimer <= 0 && Vector3.Distance(transform.position, player.transform.position) <= chasingDistance)
        {
            chasingTimer = chasingInterval;
            agent.SetDestination(player.transform.position); // Unity AI
        }

    }
    protected override void OnKill()
    {
        base.OnKill();
        agent.enabled = false;
        this.enabled = false;
        this.GetComponent<Rigidbody>().drag = 0;
        this.GetComponent<Rigidbody>().angularDrag = 0;
        transform.localEulerAngles = new Vector3(10, transform.localEulerAngles.y, transform.localEulerAngles.z);
		if (weaponType == 2)
		{
			player.weaponType = 2;
		}
    }
}