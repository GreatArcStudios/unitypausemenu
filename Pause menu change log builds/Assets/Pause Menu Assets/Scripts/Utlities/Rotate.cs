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
        public int x =5;
        public int y =3 ;
        public int z = 5;
       
        // Update is called once per frame
        /// <summary>
        /// Rotate the cube
        /// </summary>
        public void Update()
        {
            transform.Rotate(Vector3.right, x*Time.deltaTime); 
            transform.Rotate(Vector3.up, y * Time.deltaTime);
            transform.Rotate(Vector3.down, z * Time.deltaTime);
        }
    }
}

