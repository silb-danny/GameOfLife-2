using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBase : MonoBehaviour
{
    public GameObject cell;
    [HideInInspector]
    public Dictionary<Vector2,GameObject> StartCell = new Dictionary<Vector2, GameObject>();
    public float placement = 1f;
    public float sizeOrth;
    [HideInInspector]
    public Vector2 moveto;
    [HideInInspector]
    public Vector2 cameraPos;
    public Game_Of_Life_v1 controller;
    void Start()
    {
        sizeOrth = Camera.main.orthographicSize;
        cameraPos = Camera.main.transform.position;
        controller = this.GetComponent<Game_Of_Life_v1>();
        //sqr(50, 10);
    }
    void sqr(int s, int rng)
    {
        StartCell.Clear();
        for(int i = -s; i <= s; i ++)
        {
            for(int j = -s; j <= s; j++)
            {
                if(i%rng == 0)
                {
                    Vector2 mosP = new Vector2(i,j);
                    StartCell.Add(mosP,Instantiate(cell,mosP,Quaternion.identity));
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Game_Of_Life_v1.started = !Game_Of_Life_v1.started;
        }
        controller.enabled =  Game_Of_Life_v1.started;
        sizeOrth -= Input.mouseScrollDelta.y;
        sizeOrth = Mathf.Clamp(sizeOrth,0.1f,100);
        Camera.main.orthographicSize = sizeOrth;
        if(Input.GetMouseButtonDown(2))
        {
            cameraPos = Camera.main.transform.position;
            moveto = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    void FixedUpdate()
    {
        Vector2 mosP = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(Input.GetMouseButton(2))
        {
            cameraPos = cameraPos+(moveto-mosP);
        }
        Camera.main.transform.position=new Vector3(cameraPos.x,cameraPos.y,-10);
        mosP.x = Mathf.Round(mosP.x);
        mosP.y = Mathf.Round(mosP.y);
        if(Input.GetMouseButton(0))
        {
            if(!StartCell.ContainsKey(mosP) && !Game_Of_Life_v1.started)
            {
                //Debug.Log(mosP);
                StartCell.Add(mosP,Instantiate(cell,mosP,Quaternion.identity));
            }
        }
    }
    public void showStart()
    {
        foreach(var c in controller.cells)
        {
            Destroy(c.Value.gameObject);
        }
        controller.cells.Clear();
        StartCell.Clear();
        //controller.cells.Clear();
    }
}
