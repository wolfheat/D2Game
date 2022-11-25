using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class AuraArrayController : MonoBehaviour {

    public Transform[] affectors;
    public Material sourceMaterial;
    public float auraMargin = 0.75f;

    private Projector projector;
    private Bounds bounds;
    private Vector4[] positions;
    private float[] scales;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        foreach (Transform trans in affectors)
        {
            if (Selection.activeGameObject == gameObject || Selection.activeGameObject == trans.gameObject)
            Gizmos.DrawWireSphere(trans.position, trans.lossyScale.x + auraMargin);
        }        
    }
    #endif

    // Drawing overall bounds
    //void OnDrawGizmosSelected()
    //{
    //    Vector3 center = bounds.center;
    //    float radius = Mathf.Max(Mathf.Max(bounds.extents.z, bounds.extents.x), bounds.extents.y);
    //    Gizmos.color = Color.white;
    //    Gizmos.DrawWireSphere(center, radius);
    //}

    void Start () {

        bounds = new Bounds(affectors[0].position, new Vector3((affectors[0].lossyScale.x + auraMargin) * 2f, (affectors[0].lossyScale.y + auraMargin) * 2f, (affectors[0].lossyScale.z + auraMargin) * 2f));
        projector = GetComponent<Projector>();
        projector.material = new Material(sourceMaterial);
        UpdateMaterialAndProjector();

    }
	
	void Update () {

        UpdateMaterialAndProjector();

    }

    // Sending data to an instance of projector material and scaling projector radius
    public void UpdateMaterialAndProjector()
    {
        bounds = new Bounds(affectors[0].position, new Vector3((affectors[0].lossyScale.x + auraMargin) * 2f, (affectors[0].lossyScale.y + auraMargin) * 2f, (affectors[0].lossyScale.z + auraMargin) * 2f));
        foreach (Transform trans in affectors)
        {
            Bounds affectorBounds = new Bounds(trans.position, new Vector3((trans.lossyScale.x + auraMargin) * 2f, (trans.lossyScale.y + auraMargin) * 2f, (trans.lossyScale.z + auraMargin) * 2f));
            bounds.Encapsulate(affectorBounds);
        }
        projector.gameObject.transform.position = new Vector3(bounds.center.x, bounds.max.y, bounds.center.z);
        projector.orthographicSize = Mathf.Max(bounds.extents.z, bounds.extents.x);
        projector.farClipPlane = bounds.size.y;

        positions = new Vector4[affectors.Length];
        for (int i = 0; i < positions.Length; i++)
        {
            positions[i] = affectors[i].position;
        }
        projector.material.SetVectorArray("_AffectorPositions", positions);

        scales = new float[affectors.Length];
        for (int i = 0; i < scales.Length; i++)
        {
            scales[i] = affectors[i].lossyScale.x;
        }
        projector.material.SetFloatArray("_AffectorScales", scales);
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
