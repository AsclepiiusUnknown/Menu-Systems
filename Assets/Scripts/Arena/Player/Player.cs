using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public float moveSpeed = 5;

    Camera viewCamera;
    PlayerController playerController;
    GunController gunController;

    public CrossHairs crossHairs;
    public Transform crossHairsT;
    public GameObject crossHairsGO;

    public int levelIndex;
    public float playerHealth;
    private MapGenerator mapGenerator;

    private void Awake()
    {
        PlayerPrefs.SetInt("Can Cheer", 1);

        FindObjectOfType<Spawner>().OnNewWave += OnNewWave;
        playerController = GetComponent<PlayerController>();
        viewCamera = Camera.main;
        gunController = GetComponent<GunController>();

        if (GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>() != null && mapGenerator == null)
        {
            mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        }
        playerHealth = health;
        levelIndex = mapGenerator.mapIndex;


    }

    protected override void Start()
    {
        base.Start();
    }

    void OnNewWave(int waveNumber)
    {
        health = startingHealth;
        gunController.EquipGun(waveNumber - 1);
    }

    void Update()
    {
        if (GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>() != null && mapGenerator == null)
        {
            mapGenerator = GameObject.FindGameObjectWithTag("MapGenerator").GetComponent<MapGenerator>();
        }

        float horizontal = 0;
        float vertical = 0;

        if (Input.GetKey(KeyBindManager.keys["Up"]))
        {
            vertical++;
        }
        if (Input.GetKey(KeyBindManager.keys["Down"]))
        {
            vertical--;
        }
        if (Input.GetKey(KeyBindManager.keys["Left"]))
        {
            horizontal--;
        }
        if (Input.GetKey(KeyBindManager.keys["Right"]))
        {
            horizontal++;
        }

        //Movement Input
        Vector3 moveInput = new Vector3(horizontal, 0, vertical);
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        playerController.Move(moveVelocity);

        //Look Input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.up * gunController.GetHeight());
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Debug.DrawLine(ray.origin, point, Color.red);
            playerController.Look(point);
            crossHairsGO.transform.position = point;
            crossHairs.DetectTargets(ray);
            //print((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).magnitude);
            if ((new Vector2(point.x, point.z) - new Vector2(transform.position.x, transform.position.z)).sqrMagnitude > 1)
            {
                gunController.Aim(point);
            }
        }

        //Weapon Input
        if (Input.GetKey(KeyBindManager.keys["Shoot"]))
        {
            gunController.OnTriggerHold();
        }

        if (Input.GetKeyUp(KeyBindManager.keys["Shoot"]))
        {
            gunController.OnTriggerRelease();
        }

        if (Input.GetKeyDown(KeyBindManager.keys["Reload"]))
        {
            gunController.Reload();
        }

        //Out of bounds death
        if (transform.position.y < -10)
        {
            TakeDamage(health);
        }
    }

    public override void Die()
    {
        AudioManager.instance.PlaySound("Player Death", transform.position);
        base.Die();
    }
}
