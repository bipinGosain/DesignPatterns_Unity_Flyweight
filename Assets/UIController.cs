using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class UIController : MonoBehaviour
{
    List<Vector3> createPositions = new List<Vector3>();
    [SerializeField]
    GameObject container;
    [SerializeField]
    Button createObject;
    [SerializeField]
    GameObject selectionPanel;
    [SerializeField]
    ToggleGroup shape, color, size ;
    ShapeFactory shapeFac = new ShapeFactory();
    
    
    Shapes currentShape;
    Sizes currentSize;
    Color currentColor;
    
    void Start()
    {
        createPositions.Add(new Vector3(-2, 2, 0));
        createPositions.Add(new Vector3(2, 2, 0));

        createObject.onClick.AddListener(SelectObject);
    }

    void SelectObject(){

        List<Toggle> selectedShape = shape.ActiveToggles().ToList<Toggle>();
        List<Toggle> selectedColor = color.ActiveToggles().ToList<Toggle>();
        List<Toggle> selectedSize = size.ActiveToggles().ToList<Toggle>();

        selectionPanel.SetActive(false);
        
        this.currentShape = (Shapes)System.Enum.Parse(typeof(Shapes), selectedShape[0].name.ToUpper());
        this.currentSize = (Sizes)System.Enum.Parse(typeof(Sizes), selectedSize[0].name.ToUpper());

        switch(selectedColor[0].name){
            case "RED":
            this.currentColor = Color.red;
            break;
            case "GREEN":
            this.currentColor = Color.green;
            break;
            case "BLUE":
            this.currentColor = Color.blue;
            break;
        }
        

        bool createdNew = false;
        GameObject desiredObject =  this.shapeFac.GetShape(this.currentShape, this.currentSize, this.currentColor, out createdNew);   
        
        

        if(createdNew){

            if(this.shapeFac.TotalObjectsCreated <= 2)
                desiredObject.transform.position = this.createPositions[this.shapeFac.TotalObjectsCreated - 1];
            else{
                
                GameObject top = this.shapeFac.Dequeue();      
                top.GetComponent<Rigidbody>().isKinematic = false;
                Vector3 pos = this.container.transform.position;
                top.transform.position =  new Vector3(UnityEngine.Random.Range(-2.5f, 2.5f), pos.y + 2, pos.z - .5f);
                this.shapeFac.RepositionQueue(this.createPositions);
            }
        }
        
        Debug.Log("Is a new Object Created: " + createdNew);
        Debug.Log(string.Format("Retrieved Object = {0}", desiredObject));
        Debug.Log(string.Format("Total Objects Created = {0}", this.shapeFac.TotalObjectsCreated));
    }
}
