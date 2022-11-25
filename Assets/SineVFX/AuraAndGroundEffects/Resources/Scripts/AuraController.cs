using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AuraController : MonoBehaviour {

    public Material sourceMaterial;
    public float auraMargin = 0.75f;

    private Projector projector;
    //private Material instancedMaterial;
    private float resultRadius;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, resultRadius);
    }

    void Start () {

        projector = GetComponent<Projector>();
        //instancedMaterial = new Material(projector.material);
        //projector.material = instancedMaterial;
        projector.material = new Material(sourceMaterial);

        UpdateMaterialAndProjector();

    }
	
	void Update () {

        UpdateMaterialAndProjector();

    }

    // Sending data to an instance of projector material and scaling projector radius
    public void UpdateMaterialAndProjector()
    {
        resultRadius = this.gameObject.transform.lossyScale.x + auraMargin;
        projector.material.SetVector("_AuraSourcePosition", transform.position);
        projector.material.SetFloat("_MaskDistance", this.gameObject.transform.lossyScale.x);
        projector.nearClipPlane = -resultRadius;
        projector.farClipPlane = resultRadius;
        projector.orthographicSize = resultRadius;
    }

    // Step 1: Assign Source Materil for editing
    public void AssignSourceMaterial()
    {
        projector.material = sourceMaterial;
    }

    // Step 2: Update All Materials Instances in Scene
    public void UpdateAllMaterialInstances()
    {
        AuraController[] auraControllers;
        auraControllers = FindObjectsOfType<AuraController>();
        foreach (AuraController ac in auraControllers)
        {
            Projector proj = ac.gameObject.GetComponent<Projector>();
            if (proj.material.name == sourceMaterial.name)
            {
                proj.material = new Material(sourceMaterial);
            }
        }
    }    
}
