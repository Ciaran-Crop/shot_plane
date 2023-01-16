using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{

    Material material;
    [SerializeField]Vector2 scrollVelocity;
    // Start is called before the first frame update

    void Awake() 
    {
        material = GetComponent<Renderer>().material; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        material.mainTextureOffset += scrollVelocity * Time.deltaTime;
    }
}
