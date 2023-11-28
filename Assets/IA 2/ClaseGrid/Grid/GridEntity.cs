using System;
using UnityEngine;

//ExecuteInEditMode]
public class GridEntity : MonoBehaviour
{
	public event Action<GridEntity> OnMove = delegate {};
	public Vector3 velocity = new Vector3(0, 0, 0);
    public bool onGrid;
    //public Light _rend;

    private void Awake()
    {
        //_rend = GetComponent<Renderer>();
    }

    void Update() {
	    if (onGrid)
	    {
            //_rend.color = Color.red;
		    Debug.Log("In Grid");
	    }
	    else
	    {
		    Debug.Log("Out Grid");
            //_rend.color = Color.gray;
		    
	    }
		//Optimization: Hacer esto solo cuando realmente se mueve y no en el update
		transform.position += velocity * Time.deltaTime;
	    OnMove(this);
	}
}
