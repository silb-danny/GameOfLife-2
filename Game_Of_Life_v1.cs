using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Of_Life_v1 : MonoBehaviour
{
    public GameObject cell;
    public Dictionary<Vector2,GameObject> cells;
    //public GameObject[,] world = new GameObject[100,100];
    public static bool started = false;
    public bool lastS;
    public CellBase putitdown;
    public int iter = 5;
    void OnEnable()
    {
        cells = new Dictionary<Vector2, GameObject>(putitdown.StartCell);
        StartCoroutine(Loop());
    }
    void OnDisable()
    {
        StopAllCoroutines();
        putitdown.showStart();
    }
    void Awake()
    {
        lastS = started;
        putitdown = this.GetComponent<CellBase>();
        cell = putitdown.cell;
    }

    // Update is called once per frame
    void Update()
    {
        // if(lastS && !started)
        // {
        //     putitdown.showStart();
        // }
        if(started && !lastS)
        {
            //StartCoroutine(Loop());
        } 
        lastS = started;
    }
    void FixedUpdate()
    {
        //Time.fixedDeltaTime = 1;
        
    }
     IEnumerator Loop()
    {
        while (started)
        {
            for(int i = 0; i < iter; i ++)
            {yield return new WaitForSeconds(1.0f/iter);
                updateCells(cells); 
            }
           
        }
    }
    void updateCells(Dictionary<Vector2, GameObject> cells)
    {
        Dictionary<Vector2,GameObject> current = new Dictionary<Vector2, GameObject>(cells);
        bool[] activ = new bool[current.Count];
        int i = 0;
        foreach (var cell in current)
        {
            activ[i] = checkAround(cells,current,cell.Key,true,cell.Value.activeInHierarchy);
            i++;
        }
        i=0;
        foreach (var cell in current)
        {
            if(activ[i])
                cell.Value.SetActive(activ[i]);
            else
            {
                Destroy(cell.Value);
                cells.Remove(cell.Key);
            }
            i++;
        }
    }
    bool checkAround(Dictionary<Vector2, GameObject> cells,Dictionary<Vector2, GameObject> current, Vector2 pos, bool nulls, bool actv)
    {
        int sum = 0;
        for(int x = 0; x < 3*3; x ++)
        {   
            Vector2 cellP = new Vector2(x%3-1,(int)(x/3)-1) + pos;
            if(cellP != pos)
            {
                if(current.ContainsKey(cellP))
                    if(current[cellP].activeInHierarchy)
                    {
                        sum ++;
                    }
            }
        }
        bool active = ((sum == 3) || (sum == 2) && actv);// && activ);
        //Debug.Log(active);
        if(nulls)
        {
            for(int x = 0; x < 3*3; x ++)
            {   
                Vector2 cellP = new Vector2(x%3-1,(int)(x/3)-1) + pos;
                if(!cells.ContainsKey(cellP))
                {
                    bool activate = checkAround(cells,current,cellP,false,false);
                    if(activate)
                    {   
                        GameObject cll = Instantiate(cell,cellP,Quaternion.identity);
                        cll.SetActive(true);
                        cells[cellP] = cll;
                    }
                }
            }
        }
        return active;
    }
}
