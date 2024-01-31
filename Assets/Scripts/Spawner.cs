using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Clayxels;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class Spawner : MonoBehaviour
{
    public Transform ground;
    public Camera cam;
    public Transform planet;
    public Transform pivot;
    public ClayContainer clayContainer;

    public List<GameObject> objectsToSpawn;
    public float minScale;
    public float maxScale;

    public float minBlend;
    public float maxBlend;

    public Gradient colorRamp;
    public float maxHeight;
    
    public float offsetSpeed;
    public float offset;

    private bool emissiveOn;
    private float emissiveTimer;
    public float emissiveTime;
    public float emissiveOnTime;
    public float emissiveTransTime;
    
    private static readonly int Emission = Shader.PropertyToID("_Emission");
    private Material clayMat;

    public SwitchHue switchHue;
    
    void Start()
    {
        ground.parent = FindObjectOfType<ClayContainer>().transform;
        ground.gameObject.SetActive(true);
        ground.GetComponent<ClayObject>().blend = maxBlend;
        clayMat = clayContainer.getMaterial();
    }
    
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(SpawnTimer());
        }

        OffsetGradient();
        
        EmissiveTimer();
    }
    
    IEnumerator SpawnTimer()
    {
        while (Input.GetMouseButton(0))
        {
            Clicked();
            
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Spawn(float x, float z, Vector3 gravity)
    {
        Vector3 pos = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);

        var scale = Random.Range(minScale, maxScale);
        var scaleV3 = new Vector3(scale, scale, scale);
        
        var itemIndex = Random.Range(0, objectsToSpawn.Count);
        var itemToSpawn = objectsToSpawn[itemIndex];

        var spawned = Instantiate(itemToSpawn, pos, Quaternion.identity);
        spawned.transform.localScale = scaleV3;

        var blend = Random.Range(minBlend, maxBlend);
        spawned.GetComponent<ClayPhysics>().blend = blend;

        var force = new Vector3(Random.Range(-1, 1),Random.Range(-1, 1),Random.Range(-1, 1));
        spawned.GetComponent<Rigidbody>().AddForce(force);

        spawned.GetComponent<CustomGravity>().SetUpVector(gravity);
        spawned.GetComponent<CustomGravity>().planet = planet;
        
        spawned.GetComponent<ClayPhysics>().planet = planet;
        spawned.GetComponent<ClayPhysics>().spawner = GetComponent<Spawner>();
    }

    void Clicked()
    {
        var ray = cam.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast (ray, out var hit))
        {
            var direction = hit.point - planet.position;
            direction.Normalize();
            
            pivot.up = direction;
            pivot.position = hit.point;
            Spawn(0, 0, direction);
        }
    }

    private void OffsetGradient()
    {
        offset += offsetSpeed * Time.deltaTime;

        if (offset > maxHeight)
        {
            offset -= maxHeight;
        }
    }

    private void EmissiveTimer()
    {
        if (!emissiveOn)
        {
            emissiveTimer += Time.deltaTime;
            
            if (emissiveTimer >= emissiveTime + emissiveTransTime)
            {
                emissiveOn = true;
                emissiveTimer = 0f;

                clayMat.DOColor(Color.white, Emission, emissiveTransTime).OnComplete(switchHue.Shift);
            }
        }
        else
        {
            emissiveTimer += Time.deltaTime;

            if (emissiveTimer >= emissiveOnTime + emissiveTransTime)
            {
                emissiveOn = false;
                emissiveTimer = 0f;
                
                clayMat.DOColor(Color.black, Emission, emissiveTransTime);
            }
        }
    }
}
