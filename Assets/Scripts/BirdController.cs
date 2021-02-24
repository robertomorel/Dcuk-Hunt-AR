using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{

    private GameObject _targetFocus;

    [SerializeField]
    private float _speed;

    // Start is called before the first frame update
    void Start()
    {
        _targetFocus = GameObject.FindGameObjectWithTag("Target");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 target = _targetFocus.transform.position - this.transform.position;
        Debug.Log(target.magnitude); // -- Distância do pássaro ao alvo

        if (target.magnitude < 1)
        {
            TargetController.instance.MoveTarget();
            //_targetFocus.SendMessage("MoveTarget");
        }

        transform.LookAt(_targetFocus.transform);

        _speed = Random.Range(0.1f, 0.8f);
        transform.Translate(0, 0, _speed * Time.deltaTime);

    }
}
