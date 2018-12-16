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
        public int remain;

        public stageSRC(int size, int[,] init, int[,] answer, int[] Oper, int[] Num, int remain)
        {
            this.size = size;
            this.init = init;
            this.answer = answer;
            this.Oper = Oper;
            this.Num = Num;
            this.remain = remain;
        }
    }
public stageSRC[,] stageDB = new stageSRC[5, 101];


// Use this for initialization
public void loadDB()
{
        stageDB[1, 1] = new stageSRC(4,
   new int[,] {
       {0,0,0,0},
       {0,1,0,3},
       {0,0,0,0},
       {0,0,0,0}
   },
   new int[,] {
       {0,0,0,0},
       {0,1,2,0},
       {0,1,2,0},
       {0,0,0,0}
   },
   new int[] { 0, 1, 1, 1, 0 },
   new int[] { 0, 2, 1, 0, 0, 0, 0, 0, 0, 0 },
   3
   
   );
        stageDB[1, 2] = new stageSRC(6,
    new int[,] {
       {1,2,5,0,0,0},
       {3,4,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,0,0,0}
    },
    new int[,] {
       {0,0,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,0,0,0},
       {0,0,0,1,2,5},
       {0,0,0,3,4,0}
    },
    new int[] { 0, 1, 1, 2, 2 },
    new int[] { 0, 2, 2, 2, 0, 0, 0, 0, 0, 0 },
    6
    );

        stageDB[1, 3] = new stageSRC(8,
    new int[,] {
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,3,0,0,0,0,0},
            {0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0}
    },
    new int[,] {
           
            {3,1,3,3,3,3,3,3},
            {3,1,3,3,3,3,3,3},
            {3,1,3,3,3,3,3,3},
            {3,1,3,3,3,3,3,3},
            {3,1,3,3,3,3,3,3},
            {3,1,3,3,3,3,3,3},
            {3,1,1,1,1,1,1,1},
            {3,3,3,3,3,3,3,3}
    },
    new int[] { 9, 9, 9, 9, 9 },
    new int[] { 0, 9, 9, 9, 9, 9, 9, 9, 9, 9 },
    8
    );
}



// Update is called once per frame
void Update()
{

}
}
