using UnityEngine;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

public class PlayerController : MonoBehaviour, IDamage, IPickup, IOpen
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [SerializeField] float HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpVel;
    [SerializeField] int jumpMax;
    [SerializeField] float gravity;

    [SerializeField] GameObject gunModel;
    // [SerializeField] gunStats equippedGun;

    float shootTimer;
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    int gunListPos;
    [SerializeField] float shootDamage;
    [SerializeField] float shootDistance;
    [SerializeField] float shootRate;

    bool isSprinting;
    int jumpCount;
    float HPOrig;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOrig = HP;
        spawnPlayer();
    }

    Vector3 moveDir;
    Vector3 playerVel;

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);

        Movement();
        Sprint();
        if (gunList.Count > 0)
        {
            if (Input.GetAxis("Mouse ScrollWheel") != 0)
            {
                shootTimer = 0;
                selectGun();
            }

            if (gunList[gunListPos].ammo > 0)
            {
                if (Input.GetButton("Fire1") && shootTimer >= shootRate)
                {
                    AudioClip sound = gunList[gunListPos].shootSound[Random.Range(0, gunList[gunListPos].shootSound.Count())];
                    gunModel.GetComponent<AudioSource>().clip = sound;
                    gunModel.GetComponent<AudioSource>().Play();
                    gunModel.GetComponent<ParticleSystem>().Play();
                    Shoot();
                    gunList[gunListPos].ammo -= 1;
                }
            }

            if (Input.GetButton("Reload") && gunList[gunListPos].ammo < gunList[gunListPos].maxAmmo && shootTimer >= shootRate)
            {
                AudioClip sound = gunList[gunListPos].reloadSound[Random.Range(0, gunList[gunListPos].reloadSound.Count())];
                gunModel.GetComponent<AudioSource>().clip = sound;
                gunModel.GetComponent<AudioSource>().Play();
                shootTimer = 0;
                gunList[gunListPos].ammo = gunList[gunListPos].maxAmmo;
            }
        }

    }

    void Movement()
    {

        shootTimer += Time.deltaTime;

        if (controller.isGrounded) {

            jumpCount = 0;
            playerVel.y = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right)
               + (Input.GetAxis("Vertical") * transform.forward);

        /*if (controller.isGrounded && playerVel.y < 0)
        {
            playerVel.y = -1f;
        } else
        { 
            playerVel.y -= gravity * Time.deltaTime;
        }*/

        //playerVel = moveDir * speed * Time.deltaTime;
        playerVel.y -= gravity * Time.deltaTime;

        Jump();
        //controller.Move();
        controller.Move(((speed * moveDir) + playerVel) * Time.deltaTime);
    }

    void Sprint()
    {

        if (Input.GetButtonDown("Sprint"))
        {

            speed *= sprintMod;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {

            speed /= sprintMod;
            isSprinting = false;
        }
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {

            playerVel.y = jumpVel;
            jumpCount++;
        }

    }

    void Shoot()
    {

        shootTimer = 0;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {

            Debug.Log(hit.collider.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (dmg != null)
            {
                dmg.TakeDamage(shootDamage);
            }

        }

    }

    public void TakeDamage(float amount)
    {
        HP -= amount;
        updatePlayerUI();
        StartCoroutine(damageFlash());

        if (HP <= 0)
        {
            GameManager.instance.YouLose();
        }
    }

    void updatePlayerUI()
    {
        GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
    }

    IEnumerator damageFlash()
    {
        GameManager.instance.playerDamageScript.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.instance.playerDamageScript.SetActive(false);
    }

    public void getGunStats(gunStats gun)
    {
        gunList.Add(gun);
        gunListPos = gunList.Count - 1;
        changeGun();
    }

    void changeGun()
    {
        if (gunList.Count == 0) return;
        shootDamage = gunList[gunListPos].damage;
        shootDistance = gunList[gunListPos].maxRadius;
        shootRate = gunList[gunListPos].fireRate;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[gunListPos].model.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[gunListPos].model.GetComponent<MeshRenderer>().sharedMaterial;
    }
    void selectGun()
    {
        int scroll = (int)Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
        gunListPos = Mathf.Clamp(gunListPos + scroll, 0, Mathf.Max(gunList.Count - 1, 0));
        changeGun();
    }

    public void spawnPlayer()
    {
        controller.transform.position = GameManager.instance.playerSpawn.transform.position;

        HP = HPOrig;
        updatePlayerUI();
    }
}
