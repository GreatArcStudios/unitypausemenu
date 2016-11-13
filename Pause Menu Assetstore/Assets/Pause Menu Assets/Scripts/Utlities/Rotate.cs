using UnityEngine;
using System.Collections;
/// <summary>
/// Copyright (c) 2016 Eric Zhu. 
/// </summary>
namespace GreatArcStudios
{
    /// <summary>
    /// This script is used for rotating a cube around in the demo.    
    /// </summary>

    public class Rotate : MonoBehaviour
    {
        public int X =5;
        public int Y =3 ;
        public int Z = 5;
       
        // Update is called once per frame
        /// <summary>
        /// Rotate the cube
        /// </summary>
        public void Update()
        {
            transform.Rotate(Vector3.right, X*Time.deltaTime); 
            transform.Rotate(Vector3.up, Y * Time.deltaTime);
            transform.Rotate(Vector3.down, Z * Time.deltaTime);
        }
    }
}

