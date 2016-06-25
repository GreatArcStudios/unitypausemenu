using UnityEngine;
using UnityEngine.UI;
using System.Collections;
/// <summary>
///  Copyright (c) 2016 Eric Zhu 
/// </summary>
namespace GreatArcStudios
{

    public class PopulateUI : MonoBehaviour
    {
        public Transform parentPanel;
        public GameObject populatePrefab;
        public float borderY;
        public bool changeControls;
        public Text controlName;
        public Text controlButton;
        private string[] keys = { "backspace",
 "delete",
 "tab",
 "clear",
 "return",
 "pause",
 "escape",
 "space",
 "up",
 "down",
 "right",
 "left",
 "insert",
 "home",
 "end",
 "page up",
 "page down",
 "f1",
 "f2",
 "f3",
 "f4",
 "f5",
 "f6",
 "f7",
 "f8",
 "f9",
 "f10",
 "f11",
 "f12",
 "f13",
 "f14",
 "f15",
 "0",
 "1",
 "2",
 "3",
 "4",
 "5",
 "6",
 "7",
 "8",
 "9",
 "!",
 "\"",
 "#",
 "$",
 "&",
 "'",
 "(",
 ")",
 "*",
 "+",
 ",",
 "-",
 ".",
 "/",
 ":",
 ";",
 "<",
 "=",
 ">",
 "?",
 "@",
 "[",
 "\\",
 "]",
 "^",
 "_",
 "`",
 "a",
 "b",
 "c",
 "d",
 "e",
 "f",
 "g",
 "h",
 "i",
 "j",
 "k",
 "l",
 "m",
 "n",
 "o",
 "p",
 "q",
 "r",
 "s",
 "t",
 "u",
 "v",
 "w",
 "x",
 "y",
 "z",
 "numlock",
 "caps lock",
 "scroll lock",
 "right shift",
 "left shift",
 "right ctrl",
 "left ctrl",
 "right alt",
 "left alt" };
        // Populate panel
        void Start()
        {
            if (changeControls)
            {
                //Iterate thorugh list of controls
                for(int i =0; i < keys.Length; i++)
                {
              
                }
                //If it matches instance a new prefab 
            }
        }

        public void clickButton()
        {

        }
    }
}