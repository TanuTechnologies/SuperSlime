using UnityEngine;

public class FadeColliderManager : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>())
            SetTransparent(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<MeshRenderer>())
            SetOriginal(other.gameObject);
    }

    public Transform startPoint;
    public Transform endPoint;

    public BoxCollider boxCollider;

    private void Start()
    {
        // Initial setup
        UpdateBoxCollider();
    }

    private void Update()
    {
        // Update the BoxCollider if the start or end point has changed
        if (startPoint.hasChanged || endPoint.hasChanged)
        {
            UpdateBoxCollider();
        }
    }

    private void UpdateBoxCollider()
    {
        // Calculate the distance and direction between start and end points
        Vector3 direction = endPoint.position - startPoint.position;
        float distance = direction.magnitude;
        Vector3 center = startPoint.position + (direction.normalized * distance * 0.5f);

        // Set the size and position of the BoxCollider
        boxCollider.size = new Vector3(0.1f, 0.1f, distance);
        boxCollider.center = center - transform.position;

        // Rotate the collider to match the direction between the points
        boxCollider.transform.rotation = Quaternion.LookRotation(direction);

        // Reset hasChanged flags
        startPoint.hasChanged = false;
        endPoint.hasChanged = false;
    }

    private void SetTransparent(GameObject obj)
    {
        Material targetMaterial = obj.GetComponent<MeshRenderer>().material;

        targetMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        targetMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        targetMaterial.SetInt("_ZWrite", 0);
        targetMaterial.DisableKeyword("_ALPHATEST_ON");
        targetMaterial.EnableKeyword("_ALPHABLEND_ON");
        targetMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        targetMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        Color currentColor = targetMaterial.color;
        currentColor.a = .5f;
        targetMaterial.color = currentColor;

        obj.GetComponent<MeshRenderer>().material = targetMaterial;
    }

    private void SetOriginal(GameObject obj)
    {
        Material targetMaterial = obj.GetComponent<MeshRenderer>().material;
        // Set the rendering mode to opaque.
        targetMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        targetMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        targetMaterial.SetInt("_ZWrite", 1);
        targetMaterial.DisableKeyword("_ALPHATEST_ON");
        targetMaterial.DisableKeyword("_ALPHABLEND_ON");
        targetMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        targetMaterial.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;

        Color currentColor = targetMaterial.color;
        currentColor.a = 1;
        targetMaterial.color = currentColor;

        obj.GetComponent<MeshRenderer>().material = targetMaterial;
    }
}
