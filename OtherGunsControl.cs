using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class OtherGunsControl : MonoBehaviour
{
    // Start is called before the first frame update

    public Text otherGunsList;
    public Text currentOtherGun;
    public Text currentOthergunAmmunition;
    public Text totalOthergunAmmunition;
    public Text practicalOtherGunAmmunition;

    public Text currentOtherGunRPM;
    public Text maxCurrentOtherGunRPM;
    public Text minCurrentOtherGunRPM;
    public RawImage weaponImg;

    public Texture[] weaponImagesAvailable;

    public string[] otherGuns;
    public float[] otherGunsRPMs;
    public float[] otherGunTotalAmmos;
    public float[] otherGunPracticalAmmunitionNumbers;
    public float[] otherGunCurrentAmmos;
    public float[] otherGunShootVelocities;
    public float[] otherGunReloadTimes;

    public Rigidbody[] otherGunAmmunitionModels;

    public Transform[] shootSoundHolders;
    public Transform[] reloadSoundHolders;
    public Transform []destroySoundHolders;

    public Transform[] shootPositions;
    //public Transform[][] shootEffects; 
    public Transform[] shootEffects;

    bool readyToShoot = true;
    bool reloading;
    float shootTimer;
    float reloadTimer;

    int currentOtherGunIndex;


    

    void Start()
    {
        currentOtherGunIndex = 0;
        foreach(string otherGun in otherGuns)
        {
            otherGunsList.text += otherGun + System.Environment.NewLine; //silah listesi güncellenir
        }
        otherGunCurrentAmmos[currentOtherGunIndex] = otherGunPracticalAmmunitionNumbers[currentOtherGunIndex];
        otherGunTotalAmmos[currentOtherGunIndex] -= otherGunPracticalAmmunitionNumbers[currentOtherGunIndex];
    }

    private void Update()
    {
       
        
        if (otherGunTotalAmmos[currentOtherGunIndex] > 0 && !reloading && otherGunCurrentAmmos[currentOtherGunIndex] <= 0)
        {
            StartCoroutine(reloadGun());
        }

        if (Input.GetKeyDown(KeyCode.Q)) //yukarı
        {
            currentOtherGunIndex++;
            currentOtherGun.text = otherGuns[currentOtherGunIndex];
            if (currentOtherGunIndex > otherGuns.Length-1)
            {
                currentOtherGunIndex = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            currentOtherGunIndex--;
            currentOtherGun.text = otherGuns[currentOtherGunIndex];
            if (currentOtherGunIndex < 0)
            {
                currentOtherGunIndex = otherGuns.Length-1;
            }
        }

        if (Input.GetButton("Fire2"))
        {
           
            if (otherGunCurrentAmmos[currentOtherGunIndex] > 0)
            {
                if(readyToShoot && !reloading)
                {
                    Debug.Log("Firing");
                    StartCoroutine(shootGun());
                }
           
            }
            else
            {
                Debug.Log("ReloadAfterClick");
                //reload
                if(otherGunTotalAmmos[currentOtherGunIndex] > 0 && !reloading)
                {
                    StartCoroutine(reloadGun());
                }
             
            }
        }
    }

    IEnumerator shootGun()
    {
        readyToShoot = false;    
        Transform shootPosition = shootPositions[currentOtherGunIndex];
            Rigidbody instantiatedShell = Instantiate(otherGunAmmunitionModels[currentOtherGunIndex], shootPosition.transform.position, shootPosition.transform.rotation) as Rigidbody;
            instantiatedShell.transform.parent = null;
            instantiatedShell.velocity = -otherGunShootVelocities[currentOtherGunIndex] * shootPosition.forward;
        //foreach (Transform[] listAccordingToGun in shootEffects) //listAccordingToGun is each gun
        //{
        //    foreach (Transform effect in listAccordingToGun) //the list of each particle for each gun
        //    {
        //        Transform toInst = Instantiate(effect, shootPositions[currentOtherGunIndex].position, shootPositions[currentOtherGunIndex].localRotation) as Transform;
        //        toInst.gameObject.AddComponent<DestroyerScript>().duration = 3f;
        //        toInst.GetComponent<ParticleSystem>().Play();
        //        toInst.transform.parent = null;
        //    }
        //}
         foreach (Transform effect in shootEffects)
        {                            
            Transform toInst = Instantiate(effect, shootPositions[currentOtherGunIndex].position, shootPositions[currentOtherGunIndex].localRotation) as Transform;
        toInst.gameObject.AddComponent<DestroyerScript>().duration = 3f;
        toInst.GetComponent<ParticleSystem>().Play();
        toInst.transform.parent = null;
           }
        otherGunCurrentAmmos[currentOtherGunIndex] -= 1;
        shootTimer = otherGunsRPMs[currentOtherGunIndex];

        if(shootSoundHolders.Length > 0 && shootSoundHolders[currentOtherGunIndex] != null)
        {
            shootSoundHolders[currentOtherGunIndex].GetComponent<AudioSource>().Play();
        }
    //while(shootTimer > 0)
        //{
        //    shootTimer--;
        //    yield return new WaitForSeconds(0.1f);
        //}
        yield return new WaitForSeconds(1 / (otherGunsRPMs[currentOtherGunIndex] / 60));
          
        readyToShoot = true;
        //text
        currentOthergunAmmunition.text = otherGunCurrentAmmos[currentOtherGunIndex].ToString() + "/";
        totalOthergunAmmunition.text = otherGunTotalAmmos[currentOtherGunIndex].ToString();
        practicalOtherGunAmmunition.text = otherGunPracticalAmmunitionNumbers[currentOtherGunIndex].ToString();
        currentOtherGun.text = otherGuns[currentOtherGunIndex];
  
}

    IEnumerator reloadGun()
    {
        reloading = true;
        reloadTimer = otherGunReloadTimes[currentOtherGunIndex];
        while(reloadTimer > 0)
        {
            reloadTimer--;
            if(reloadTimer == otherGunReloadTimes[currentOtherGunIndex] - 2)
            {
                if(reloadSoundHolders.Length > 0 && reloadSoundHolders[currentOtherGunIndex] != null)
                {
                    reloadSoundHolders[currentOtherGunIndex].GetComponent<AudioSource>().Play();
                }
            }
            yield return new WaitForSeconds(1f);
        }
        otherGunCurrentAmmos[currentOtherGunIndex] = otherGunPracticalAmmunitionNumbers[currentOtherGunIndex];
        otherGunTotalAmmos[currentOtherGunIndex] -= otherGunPracticalAmmunitionNumbers[currentOtherGunIndex];
        reloading = false;
        //Text
        currentOthergunAmmunition.text = otherGunCurrentAmmos[currentOtherGunIndex].ToString() + "/";
        totalOthergunAmmunition.text = otherGunTotalAmmos[currentOtherGunIndex].ToString();
        practicalOtherGunAmmunition.text = otherGunPracticalAmmunitionNumbers[currentOtherGunIndex].ToString();
        currentOtherGun.text = otherGuns[currentOtherGunIndex];
    }


    // Update is called once per frame
   
}
