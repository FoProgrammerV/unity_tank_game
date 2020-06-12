using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float destroyTime;
    public string enemyTag;
    public ParticleSystem impacteffectDirt;
    public ParticleSystem impacteffectArmor;
    public ParticleSystem destroyEffect;
    public Transform impactSoundEffect;
    public float PenetrationValue;
    bool hasInteracted;

    void Start()
    {
        Destroy(gameObject, destroyTime);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
       
            hasInteracted = true;
            if (collision.transform.tag == "floor")
            {

                
ParticleSystem instp = Instantiate(impacteffectDirt, collision.transform.position, transform.rotation, collision.transform) as ParticleSystem;
            instp.gameObject.AddComponent<DestroyerScript>().duration = 1f ;
            instp.transform.parent = null;
                instp.Play();
            Transform soundInst = Instantiate(impactSoundEffect, collision.transform.position, transform.rotation, collision.transform) as Transform;
               soundInst.GetComponent<AudioSource>().Play();

            Destroy(soundInst.gameObject, 1f);
            
               
            }
            else if(collision.transform.tag == enemyTag)
        
            {
            ArmorScript enemyArmor = collision.gameObject.GetComponent<ArmorScript>();
              if(PenetrationValue < enemyArmor.Thiccness)
            {
                //hasar yok
            }else if(PenetrationValue > enemyArmor.Thiccness)
            {
                enemyArmor.Tparent.GetComponent<TankHealth>().Helth -= Mathf.Abs(PenetrationValue - enemyArmor.Thiccness);
                

            } else if(PenetrationValue == enemyArmor.Thiccness)
            {
                enemyArmor.Tparent.GetComponent<TankHealth>().Helth -= PenetrationValue / 0.1f;
            }
      
                impacteffectArmor.Play();
            }
            
            Destroy(gameObject, 0.1f);
       
       
    }
}
