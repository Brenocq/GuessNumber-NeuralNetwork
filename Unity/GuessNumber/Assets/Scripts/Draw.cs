using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {
	[SerializeField] GameObject pixel;
	bool[,] canvas = new bool[51, 51];
	List<GameObject> pixels;
	private bool mousePressed;


	void Start () {
	}

	void Update () {
		 draw();
	}

	private void draw(){
		// Mouse X and Y in the canvas grid (center of pixels)
		if(Input.GetMouseButtonDown(0)){
			mousePressed = true;
		}
		if(Input.GetMouseButtonUp(0)){
			mousePressed = false;
		}

		if(mousePressed){
			float mouseX = ((Input.mousePosition.x/Screen.width-0.5f)  * 10f*16f/9f+4f) /8f*50f;
			float mouseY = ((Input.mousePosition.y/Screen.height-0.5f) * 10f+4f)			  /8f*50f;
			//Debug.Log("("+mouseX+","+mouseY+")");
			createPixel((int)mouseX,(int)mouseY);
			createPixel((int)mouseX+1,(int)mouseY);
			createPixel((int)mouseX-1,(int)mouseY);
			createPixel((int)mouseX,(int)mouseY+1);
			createPixel((int)mouseX,(int)mouseY-1);
		}
	}

	private void createPixel(int x, int y){
		float posX = -4f+(8f/51f)*(x+0.5f);
		float posY = -4f+(8f/51f)*(y+0.5f);
		if((x>=0 && x<51)&&(y>=0 && y<51)){
			Vector2 pos = new Vector2(posX, posY);
			Quaternion rot = Quaternion.Euler(0, 0, 0);
			GameObject goPixel;
			goPixel = Instantiate(pixel, pos, rot) as GameObject ;
			goPixel.transform.parent = transform;
			//pixels.Add(goPixel);

			// Update canvas matrix
			canvas[x,y] = true;
		}
	}

	public void clearCanvas(){
		for (int i=0; i<canvas.GetLength(0); i++) {
			for (int j=0; j<canvas.GetLength(1); j++) {
				canvas[i,j]=false;
			}
		}
		//Destroy(pixels[0]);
	}
}
