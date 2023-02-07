using System.Collections.Generic;
using UnityEngine;

public class Cubescript : MonoBehaviour
{
    [SerializeField] GameObject quad;
    private int sides = 6;
    private int tilesAMT = 3;
    private List<Color> colors = new List<Color>() {Color.red,Color.blue,Color.green,Color.gray,Color.cyan,Color.white};        

	private void Start()
    {
        for (int i = 0; i < sides; i++)
        {
            var newSide = new GameObject();
            for (int j = 0; j < tilesAMT; j++)
            {
                for (int k = 0; k < tilesAMT; k++)
                {
                    var tile = Instantiate(quad,newSide.transform);
                    tile.transform.localPosition = new Vector3(-(tilesAMT-1)/2+j, -(tilesAMT - 1) / 2 + k,0);                    
                    tile.GetComponent<Renderer>().material.color = colors[Random.Range(0,5)];
                }
            }
            switch (i) // Sets position And Rotation
            {   
                case 0:
                    newSide.transform.position = new Vector3(0, 0,-(tilesAMT) / 2f);
                    break;
                    case 1:
                    newSide.transform.position = new Vector3((tilesAMT) / 2f, 0, 0);
                    newSide.transform.Rotate(Vector3.up * -90f);
                    break;
                    case 2:
					newSide.transform.position = new Vector3(0, 0, (tilesAMT) / 2f);
					newSide.transform.Rotate(Vector3.up * 180f);
					break;
                    case 3:
					newSide.transform.position = new Vector3(-(tilesAMT) / 2f, 0, 0);
					newSide.transform.Rotate(Vector3.up * 90f);
					break;
                    case 4:
					newSide.transform.position = new Vector3(0,(tilesAMT) / 2f, 0);
					newSide.transform.Rotate(Vector3.right * 90f);
					break;
                    case 5:
					newSide.transform.position = new Vector3(0,-(tilesAMT) / 2f, 0);
					newSide.transform.Rotate(Vector3.right * -90f);
					break;
                default:
                    break;
            }
        }
    }
}
