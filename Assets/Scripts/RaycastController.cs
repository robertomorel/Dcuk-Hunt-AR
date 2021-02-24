using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastController : MonoBehaviour
{

    public float maxDistanceRay = 100.0f;
    public static RaycastController instance;
    public Text birdName;
    public Transform gunFlashTarget;
    public float fireRate = 1.6f;
    private bool nextShot = true;
    private string objName = "";

    AudioSource audioSource;
    public AudioClip[] clips;

    [SerializeField]
    private GameObject _bird;

    [SerializeField]
    private GameObject _gunFlash;

    [SerializeField]
    private GameObject _explosion;

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
        StartCoroutine(SpawNewBird());
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void PlaySound(int sound)
    {
        audioSource.clip = clips[sound];
        audioSource.Play();
    }

    public void Fire()
    {
        if (nextShot)
        {
            StartCoroutine(TakeShot());
            nextShot = false;
        }
    }

    private IEnumerator SpawNewBird()
    {
        yield return new WaitForSeconds(3.0f);

        // -- Spawn a new bird
        GameObject newBird = Instantiate(_bird);

        // -- Make bird child of Terrain
        newBird.transform.parent = GameObject.Find("Terrain").transform;

        // -- Scale bird
        newBird.transform.localScale = new Vector3(10f, 10f, 10f);

        // -- Random start position
        Vector3 temp;
        temp.x = Random.Range(3.4f, 6.5f);
        temp.y = Random.Range(0.4f, 1.4f);
        temp.z = Random.Range(2.7f, 7.7f);
        newBird.transform.position = temp;

    }

    private IEnumerator ClearExplosion()
    {
        yield return new WaitForSeconds(1.5f);

        GameObject[] smokeGroup = GameObject.FindGameObjectsWithTag("Boom");
        foreach (GameObject smoke in smokeGroup)
        {
            Destroy(smoke.gameObject);
        }
    }

    private IEnumerator TakeShot()
    {

        GunController.instance.FireSound();

        // -- 
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        GameManager.instance.shotsPerRound--;

        // -- Utiliza máscara para todos os elementos que estiverem no layer "Bird"
        int layerMask = LayerMask.GetMask("Bird");

        if (Physics.Raycast(ray, out hit, maxDistanceRay, layerMask))
        {

            // debug

            GameObject objHitted = hit.collider.gameObject;

            string objName = objHitted.name;

            birdName.text = objName;

            Vector3 birdPosition = objHitted.transform.position;

            if (objHitted.tag == "Bird")
            {
                GameObject fire = Instantiate(_explosion, birdPosition, Quaternion.identity);

                PlaySound(1);

                Destroy(objHitted);

                StartCoroutine(SpawNewBird());

                StartCoroutine(ClearExplosion());

                GameManager.instance.shotsPerRound = 3;
                GameManager.instance.playerScore++;
                GameManager.instance.roundScore++;
            }

        }

        GameObject gunFlash = Instantiate(_gunFlash);
        gunFlash.transform.position = gunFlashTarget.position;

        yield return new WaitForSeconds(fireRate);

        nextShot = true;

        GameObject[] gunSmokeGroup = GameObject.FindGameObjectsWithTag("GunSmoke");
        foreach (GameObject gunSmoke in gunSmokeGroup)
        {
            Destroy(gunSmoke.gameObject);
        }

    }

}