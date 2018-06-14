using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generates a plane with a specific resolution and transforms the plane to make waves
public class WaterSquare
{
    public Transform squareTransform;

    //Add the wave mesh to the MeshFilter
    public MeshFilter terrainMeshFilter;

    //For the thread to update the water
    //The local center position of this square to fake transformpoint in a thread
    public Vector3 centerPos;
    //The latest vertices that belong to this square
    public Vector3[] vertices;

    public WaterSquare(GameObject waterSquareObj)
    {
        this.squareTransform = waterSquareObj.transform;

        this.terrainMeshFilter = squareTransform.GetComponent<MeshFilter>();

        //Save the center position of the square
        this.centerPos = waterSquareObj.transform.localPosition;


        //Generate the sea
        //To calculate the time it took to generate the terrain
        float startTime = System.Environment.TickCount;

        //Save the vertices so we can update them in a thread
        this.vertices = terrainMeshFilter.mesh.vertices;
    }

    //If we are updating the square from outside of a thread 
    public void MoveSea(Vector3 oceanPos, float timeSinceStart)
    {
        Vector3[] vertices = terrainMeshFilter.mesh.vertices;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = vertices[i];

            Vector3 vertexGlobal = vertex + centerPos + oceanPos;

            //Get the water height at this coordinate
            vertex.y = WaterController.current.GetWaveYPos(vertexGlobal, timeSinceStart);

            //Don't need to go from global to local because the y pos is always at 0
            vertices[i] = vertex;
        }

        terrainMeshFilter.mesh.vertices = vertices;

        terrainMeshFilter.mesh.RecalculateNormals();
    }
}