﻿using UnityEngine;

namespace LowPolyWater
{
    public class LowPolyWater : MonoBehaviour
    {
        public float waveHeight = 0.5f;
        public float waveFrequency = 0.5f;
        public float waveLength = 0.75f;

        //Position where the waves originate from
        public Vector3 waveOriginPosition = new Vector3(0.0f, 0.0f, 0.0f);

        private MeshFilter _meshFilter;
        private Mesh _mesh;
        [SerializeField] private Vector3[] vertices;

        private void Awake()
        {
            //Get the Mesh Filter of the gameobject
            _meshFilter = GetComponent<MeshFilter>();
        }

        private void Start()
        {
            CreateMeshLowPoly(_meshFilter);
        }

        /// <summary>
        /// Rearranges the mesh vertices to create a 'low poly' effect
        /// </summary>
        /// <param name="mf">Mesh filter of gamobject</param>
        /// <returns></returns>
        private void CreateMeshLowPoly(MeshFilter mf)
        {
            _mesh = mf.sharedMesh;

            //Get the original vertices of the gameobject's mesh
            var originalVertices = _mesh.vertices;

            //Get the list of triangle indices of the gameobject's mesh
            var triangles = _mesh.triangles;

            //Create a vector array for new vertices 
            var meshVertices = new Vector3[triangles.Length];
            
            //Assign vertices to create triangles out of the mesh
            for (var i = 0; i < triangles.Length; i++)
            {
                meshVertices[i] = originalVertices[triangles[i]];
                triangles[i] = i;
            }

            //Update the gameobject's mesh with new vertices
            _mesh.vertices = meshVertices;
            _mesh.SetTriangles(triangles, 0);
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
            vertices = _mesh.vertices;
        }
        
        void Update()
        {
            GenerateWaves();
        }

        /// <summary>
        /// Based on the specified wave height and frequency, generate 
        /// wave motion originating from waveOriginPosition
        /// </summary>
        private void GenerateWaves()
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                var v = vertices[i];

                //Initially set the wave height to 0
                v.y = 0.0f;

                //Get the distance between wave origin position and the current vertex
                var distance = Vector3.Distance(v, waveOriginPosition);
                distance = (distance % waveLength) / waveLength;

                //Oscilate the wave height via sine to create a wave effect
                v.y = waveHeight * Mathf.Sin(Time.time * Mathf.PI * 2.0f * waveFrequency
                + Mathf.PI * 2.0f * distance);
                
                //Update the vertex
                vertices[i] = v;
            }

            //Update the mesh properties
            _mesh.vertices = vertices;
            _mesh.RecalculateNormals();
            _mesh.MarkDynamic();
            _meshFilter.mesh = _mesh;
        }
    }
}