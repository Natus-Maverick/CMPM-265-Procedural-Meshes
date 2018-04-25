using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]

public class CubeMaker : MonoBehaviour {

    public Vector3 size = Vector3.one;
    public MeshCreator mc;
    public MeshFilter meshFilter;
    public Vector3 cubeSize;
    public float factor;
    public float coordy;
    public Material tt;
    bool inc;

    public GameObject firework;
    public GameObject ring;

    public bool MusicOn;
    public float keyTimer;

    // Use this for initialization
    void Start () {
        meshFilter = this.GetComponent<MeshFilter>();
        mc = new MeshCreator();
        factor = 0.1f;
    }

    // Update is called once per frame
    void Update() {

        mc.ListClear();
        //if (factor > 1) { inc = false; }
        //if (factor < -1) { inc = true; }
        /*if (inc) {*/
        factor += Time.deltaTime * 0.4f;
        keyTimer += Time.deltaTime;
        //}
        //else if (!inc) { factor -= 0.005f; }
        for (int i = 0; i < 40; ++i)
        {
            for (int j = 0; j < 40; ++j)
            {                
                cubeSize = size * 0.5f;
                key(i, j);
                if (!MusicOn)
                {
                    NoiseAnim(i, j);
                    this.GetComponent<AudioSource>().enabled = false;
                }
                if (MusicOn)
                {
                    this.GetComponent<AudioSource>().enabled = true;
                    SoundAnim(i, j);
                }

                // top of the cube        
                // t0 is top left point      
                Vector3 t0 = new Vector3(cubeSize.x + size.x * i * 1.2f, coordy, -cubeSize.z + size.z * j * 1.2f);
                Vector3 t1 = new Vector3(-cubeSize.x + size.x * i * 1.2f, coordy, -cubeSize.z + size.z * j * 1.2f);
                Vector3 t2 = new Vector3(-cubeSize.x + size.x * i * 1.2f, coordy, cubeSize.z + size.z * j * 1.2f);
                Vector3 t3 = new Vector3(cubeSize.x + size.x * i * 1.2f, coordy, cubeSize.z + size.z * j * 1.2f);
                // bottom of the cube        
                Vector3 b0 = new Vector3(cubeSize.x + size.x * i * 1.2f, -cubeSize.y, -cubeSize.z + size.z * j * 1.2f);
                Vector3 b1 = new Vector3(-cubeSize.x + size.x * i * 1.2f, -cubeSize.y, -cubeSize.z + size.z * j * 1.2f);
                Vector3 b2 = new Vector3(-cubeSize.x + size.x * i * 1.2f, -cubeSize.y, cubeSize.z + size.z * j * 1.2f);
                Vector3 b3 = new Vector3(cubeSize.x + size.x * i * 1.2f, -cubeSize.y, cubeSize.z + size.z * j * 1.2f);
                //Debug.Log(t0);
                //Debug.Log(Perlin.Noise(t0));
                //Debug.Log(Perlin.Noise(100, 100, Random.Range(0, 1000) * 0.1f));

                // Top square        
                mc.BuildTriangle(t0, t1, t2);
                mc.BuildTriangle(t0, t2, t3);
                // Bottom square        
                mc.BuildTriangle(b2, b1, b0);
                mc.BuildTriangle(b3, b2, b0);
                // Back square        
                mc.BuildTriangle(b0, t1, t0);
                mc.BuildTriangle(b0, b1, t1);
                mc.BuildTriangle(b1, t2, t1);
                mc.BuildTriangle(b1, b2, t2);
                mc.BuildTriangle(b2, t3, t2);
                mc.BuildTriangle(b2, b3, t3);
                mc.BuildTriangle(b3, t0, t3);
                mc.BuildTriangle(b3, b0, t0);

            }
        }
        
        if (coordy > 20f)
        {
            GameObject instR = Instantiate(ring, new Vector3(-50, coordy, 0), Quaternion.identity);
            Destroy(instR, 5f);
            if (coordy > 30f)
            {
                GameObject instF = Instantiate(firework, new Vector3(-50, coordy, 0), Quaternion.identity);
                Destroy(instF, 10f);
            }
        }
        meshFilter.mesh = mc.CreateMesh();
    }

    private float NoiseAnim(int i, int j)
    {
        coordy = (cubeSize.y + Perlin.Noise(cubeSize.x + size.x * i * 0.12f, 0, -cubeSize.z + size.z * j * 0.12f + factor)) * 5;
        return coordy;
    }
    private float SoundAnim(int i, int j)
    {
        float[] spectrum = new float[256];
        float sum = 0;

        GetComponent<AudioSource>().GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        for (int x = (i + j) * 3; x < (i + j+ 1) * 3; ++x)
        {
            if (i + j <= 40)
            {
                sum += Mathf.Pow(spectrum[x],2);
            }
            else
            {
                sum += Mathf.Pow(spectrum[237 - x], 2);
            }
        }
        coordy = Mathf.Sqrt(sum) * 50f;        

        return coordy;
    }
    private void key(int i, int j)
    {
        Color ori = tt.GetColor("_Color");
        if (Input.GetKeyUp(KeyCode.R))
        {
                tt.SetColor("_Color", new Color(0.8f, 0.125f, 0.125f, 1));
                keyTimer = 0;
        }
        else if (Input.GetKeyUp(KeyCode.G))
        {
                tt.SetColor("_Color", new Color(0.125f, 1, 0.21f, 1));
                keyTimer = 0;
        }
        else if (Input.GetKeyUp(KeyCode.B))
        {
                tt.SetColor("_Color", new Color(0.4f, 0.8f, 1, 1));
                keyTimer = 0;
        }
    }
}
