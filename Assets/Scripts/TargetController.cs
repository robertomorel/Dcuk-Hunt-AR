using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class TargetController : DefaultTrackableEventHandler
{

    public static TargetController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
     OnTriggerEnter é disparado quando um Rigidbody com colisor intersecciona 
     um colisor marcado como "Trigger". Já OnCollisionEnter dispara quando um 
     Rigidbody com colisor colide com um outro colisor. O trigger é utilizado 
     quando a interação deve ser sem bloqueio do movimento, como quando se cria 
     uma área que dispara um evento quando o personagem entra nela. Já o collider 
     é usado quando um corpo precisa bater no outro para o evento ocorrer.
     */

    private void OnTriggerEnter(Collider other)
    {
        MoveTarget();
    }

    public void MoveTarget()
    {
        Vector3 temp;
        temp.x = Random.Range(3.4f, 6.5f);
        temp.y = Random.Range(0.4f, 1.4f);
        temp.z = Random.Range(2.7f, 7.7f);
        transform.position = temp;
        if (DefaultTrackableEventHandler.trueFalse)
        {
            RaycastController.instance.PlaySound(0);
        }

    }
}
