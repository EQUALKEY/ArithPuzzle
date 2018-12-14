using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DB : MonoBehaviour
{

    public struct stageSRC
    {
        public int size;
        public int[,] init;
        public int[,] answer;
        public int[] Oper;
        public int[] Num;

        public stageSRC(int size, int[,] init, int[,] answer, int[] Oper, int[] Num)
        {
            this.size = size;
            this.init = init;
            this.answer = answer;
            this.Oper = Oper;
            this.Num = Num;
        }
    }
public stageSRC[,] stageDB = new stageSRC[5, 101];


// Use this for initialization
void Awake()
{

    stageDB[1, 1] = new stageSRC(4,
    new int[,] {
            {0,0,0,0},
            {0,0,0,0},
            {0,0,0,0},
            {0,0,0,0}
    },
    new int[,] {
            {0,0,0,0},
            {0,1,1,0},
            {0,2,2,0},
            {0,0,0,0}
    },
    new int[] { 0, 5, 1, 2, 3 },
    new int[] { 9, 9, 9, 9, 9, 9, 9, 9, 9, 9 }
    );
}



// Update is called once per frame
void Update()
{

}
}
