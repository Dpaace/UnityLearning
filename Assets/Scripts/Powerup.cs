using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;

    [SerializeField] // 0=Triple Shot, 1=Speed, 2=Shields
    private int _powerupID;
    [SerializeField]
    private AudioClip _clip;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(_speed * Time.deltaTime * Vector3.down); 

        if(transform.position.y < -4.5f)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            AudioSource.PlayClipAtPoint(_clip, transform.position);
            if(player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        //Debug.Log("Collected Speed Boost");
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        //Debug.Log("Collected Shields");
                        player.ShieldsActive();
                        break;
                    default:
                        Debug.Log("Default Value");
                        break;

                }
                
            }
            Destroy(this.gameObject);
        }
    }
}
