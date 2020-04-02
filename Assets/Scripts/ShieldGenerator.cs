using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ShieldGenerator))]
public class ShieldGeneratorEditor : Editor {

    public override void OnInspectorGUI () {
        DrawDefaultInspector();

        ShieldGenerator script = (ShieldGenerator)target;

        if(GUILayout.Button("Generate")){
            script.Generate();
        }
    }
}


public class ShieldGenerator : MonoBehaviour
{
    public GameObject shieldPart = null;
    public Texture2D shieldTexture = null;
    public int[] rows = null;
    public int longestPart = 0;
    public Sprite[] sprites = null;

    public void Generate()
    {
        while (transform.childCount != 0)
            DestroyImmediate(transform.GetChild(0).gameObject);

        sprites = Resources.LoadAll<Sprite>(shieldTexture.name);
        int index = 0;

        for (int x = 0; x != rows.Length; x++) {
            GameObject line = new GameObject("Line" + x);
            line.transform.parent = this.transform;
            for (int col = 0; col != rows[x]; col++) {
                int cubes = longestPart - rows[x];

                GameObject shieldPixel = Instantiate(shieldPart, transform.position + new Vector3(0.2f * col, -0.2f * x, 0), transform.rotation, line.transform);
                shieldPixel.GetComponentInChildren<SpriteRenderer>().sprite = sprites[index];
                index++;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
