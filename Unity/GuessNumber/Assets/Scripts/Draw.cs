using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour {
	[SerializeField] GameObject pixel;
	[SerializeField] int pixelQtd = 51;
	public bool[,] screen;
	List<GameObject> pixels = new List<GameObject>();
	private bool mousePressed;


	void Start () {
		screen = new bool[pixelQtd, pixelQtd];
	}

	void Update () {
		 draw();
	}

	public void draw(){
		// Mouse X and Y in the screen grid (center of pixels)
		if(Input.GetMouseButtonDown(0)){
			mousePressed = true;
		}
		if(Input.GetMouseButtonUp(0)){
			mousePressed = false;
		}

		if(mousePressed){
			Camera gameCamera = Camera.main;
			float mouseX = gameCamera.ViewportToWorldPoint(new Vector3(Input.mousePosition.x/Screen.width,0,0)).x;
			float mouseY = gameCamera.ViewportToWorldPoint(new Vector3(0,Input.mousePosition.y/Screen.height,0)).y;

			float canvasPointX = (mouseX+4f)/8f *pixelQtd;
			float canvasPointY = (mouseY+4f)/8f *pixelQtd;

			//Debug.Log("("+canvasPointX+","+canvasPointY+")");

			createPixel((int)canvasPointX,(int)canvasPointY);
			createPixel((int)canvasPointX+1,(int)canvasPointY);
			createPixel((int)canvasPointX-1,(int)canvasPointY);
			createPixel((int)canvasPointX,(int)canvasPointY+1);
			createPixel((int)canvasPointX,(int)canvasPointY-1);
		}
	}

	public void createPixel(int x, int y){
		float posX = -4f+(8f/pixelQtd)*(x+0.5f);
		float posY = -4f+(8f/pixelQtd)*(y+0.5f);
		if((x>=0 && x<pixelQtd)&&(y>=0 && y<pixelQtd) && screen[x,y]==false){
			Vector2 pos = new Vector2(posX, posY);
			Quaternion rot = Quaternion.Euler(0, 0, 0);
			GameObject goPixel;
			goPixel = Instantiate(pixel, pos, rot) as GameObject ;
			goPixel.transform.parent = transform;
			pixels.Add(goPixel);

			// Update screen matrix
			screen[x,y] = true;
		}
	}

	public void clearScreen(){
		for (int i=0; i<screen.GetLength(0); i++) {
			for (int j=0; j<screen.GetLength(1); j++) {
				screen[i,j]=false;
			}
		}
		for (int i=0; i<pixels.Count; i++) {
			Destroy(pixels[i]);
		}
	}

	public List<int> getData(){
		List<int> data = new List<int>();
		for (int y=0; y<pixelQtd; y++) {
			for (int x=0; x<pixelQtd; x++) {
				data.Add(screen[x,y]==true? 1 : 0);
			}
		}
		return data;
	}

	public int getPixelQtd(){ return pixelQtd; }

}
