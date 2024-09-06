using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorTransparency : MonoBehaviour
{

    public GameObject plane;
    public GameObject player;

    private Material planeMaterial;
    // Start is called before the first frame update
    void Start()
    {
        planeMaterial = plane.GetComponent<Renderer>().material;
        
    }
    // Update is called once per frame
    void Update()
    {
        if(player.transform.position.y < plane.transform.position.y)
        {
            SetMaterialTransparent();
        }
        else
        {
            SetMatBasic();
        }
    }

    void SetMaterialTransparent() 
    {
        planeMaterial.SetFloat("_Mode", 3);

        planeMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        planeMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        planeMaterial.SetInt("_ZWrite", 0);
        planeMaterial.DisableKeyword("_ALPHATEST_ON");
        planeMaterial.EnableKeyword("_ALPHABLEND_ON");
        planeMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        planeMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        Color customColor = new Color(0.5f, 0.3f, 0.7f, 0.3f);
        planeMaterial.color = customColor;
    }
    private void SetMatBasic()
    {
        planeMaterial.SetFloat("_Mode", 1);
        Color customColor = new Color(0.5f, 0.3f, 0.7f, 1f);
        planeMaterial.color = customColor;
    }
}
