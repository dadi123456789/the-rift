using UnityEngine;

public class LootPickup : MonoBehaviour
{
    [SerializeField] int value = 10;
    [SerializeField] float bobSpeed = 2f;
    [SerializeField] float bobHeight = 0.2f;
    Vector3 startPos;

    void Start() => startPos = transform.position;

    void Update()
    {
        transform.position = startPos + Vector3.up * (Mathf.Sin(Time.time * bobSpeed) * bobHeight);
        transform.Rotate(Vector3.up, 90f * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name != "Player") return;
        GameManager.AddCurrency(value);
        Destroy(gameObject);
    }

    public static void Spawn(Vector3 position, Transform parent)
    {
        var go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = "Loot";
        if (parent != null) go.transform.SetParent(parent);
        go.transform.position = position + Vector3.up * 0.5f;
        go.transform.localScale = Vector3.one * 0.4f;
        go.GetComponent<Renderer>().material.color = new Color(1f, 0.85f, 0.2f);
        go.GetComponent<Collider>().isTrigger = true;
        go.AddComponent<LootPickup>();
    }
}