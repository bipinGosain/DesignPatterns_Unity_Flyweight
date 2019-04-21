using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public enum Shapes{ SPHERE, CUBE, CYLINDER}
public enum Sizes{ SMALL, MEDIUM, LARGE}
/// <summary>
/// The 'Flyweight' interface
/// </summary>
public abstract class AbstractShape: MonoBehaviour
{
    public Shapes Shape;
    public Sizes Size;
    public Color ColorOfShape;
}

class CubeShape : AbstractShape
{
    public override string ToString(){

        return string.Format( "Shape= {0}, Color= {1}, Size= {2}", this.Shape, this.ColorOfShape, this.Size );
    }
}

class SphereShape : AbstractShape
{
    public override string ToString(){

        return string.Format( "Shape= {0}, Color= {1}, Size= {2}", this.Shape, this.ColorOfShape, this.Size );
    }
}

class CylinderShape: AbstractShape{

    public override string ToString(){

        return string.Format( "Shape= {0}, Color= {1}, Size= {2}", this.Shape, this.ColorOfShape, this.Size );
    }
}



public class ShapeFactory : MonoBehaviour{
    
    int currentIndex = 0;
    Queue<AbstractShape> shapes = new Queue<AbstractShape>();
 
    public int TotalObjectsCreated
    {
       get { return shapes.Count; }
    }

    public GameObject Dequeue(){

        return this.shapes.ElementAt(this.currentIndex++).gameObject;
    }

    public void RepositionQueue(List<Vector3> positions){
        
        int index = 0;
        for (int i = this.currentIndex; i < this.currentIndex + 2; i++)
            this.shapes.ElementAt(i).gameObject.transform.position = positions[index++];
        
    }
 
    void SetObject(GameObject toSet, Shapes shape, Sizes size, Color color){

        toSet.AddComponent<BoxCollider>();
        toSet.AddComponent<Rigidbody>();
        toSet.GetComponent<Rigidbody>().isKinematic = true;
        switch(shape){
            case Shapes.CUBE:
                toSet.GetComponent<CubeShape>().Shape = shape;
                toSet.GetComponent<CubeShape>().ColorOfShape = color;
                toSet.GetComponent<CubeShape>().Size = size;
                break;
            case Shapes.CYLINDER:
                toSet.GetComponent<CylinderShape>().Shape = shape;
                toSet.GetComponent<CylinderShape>().ColorOfShape = color;
                toSet.GetComponent<CylinderShape>().Size = size;
                break;
            case Shapes.SPHERE:
                toSet.GetComponent<SphereShape>().Shape = shape;
                toSet.GetComponent<SphereShape>().ColorOfShape = color;
                toSet.GetComponent<SphereShape>().Size = size;break;
        }
        toSet.GetComponent<MeshRenderer>().materials[0].color = color;
        
        switch(size){
            case Sizes.SMALL:
            toSet.transform.localScale = new Vector3(.3f, .3f, .3f);  
            break;
            case Sizes.MEDIUM:
            toSet.transform.localScale = new Vector3(.6f, .6f, .6f);
            break;
            case Sizes.LARGE:
            toSet.transform.localScale = new Vector3(.9f, .9f, .9f);
            break;
        }
    }
    public GameObject GetShape(Shapes cShape, Sizes cSize, Color cColor, out bool created)
    {
        GameObject shape = null;
        if (shapes.Any(x=> x.Shape == cShape && x.ColorOfShape == cColor && x.Size == cSize )){
            
            created = false;
            for (int i = 0; i < this.shapes.Count; i++)
            {
                if(this.shapes.ElementAt(i).Shape == cShape && this.shapes.ElementAt(i).ColorOfShape == cColor && this.shapes.ElementAt(i).Size == cSize ){
                    shape = this.shapes.ElementAt(i).gameObject;
                    break;
                }
            }
        }
        else
        {
            created = true;
            switch (cShape)
            {
                case Shapes.CUBE:
                shape = GameObject.CreatePrimitive(PrimitiveType.Cube);
                shape.AddComponent<CubeShape>();
                this.SetObject(shape, cShape, cSize, cColor);


                this.shapes.Enqueue(shape.GetComponent<CubeShape>());
                break;

                case Shapes.SPHERE:
                shape= GameObject.CreatePrimitive(PrimitiveType.Sphere);
                shape.AddComponent<SphereShape>();
                this.SetObject(shape, cShape, cSize, cColor);
                this.shapes.Enqueue(shape.GetComponent<SphereShape>());
                break;

                case Shapes.CYLINDER:
                shape = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                shape.AddComponent<CylinderShape>();
                this.SetObject(shape, cShape, cSize, cColor);
                this.shapes.Enqueue(shape.GetComponent<CylinderShape>());

                break;
                
                default:
                    throw new System.Exception("Not implemented for this type!");
            }
            
        }
        return shape;
    }
}
