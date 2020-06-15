using System;

namespace Framework 
{
    public class SimilarPairsFramework : IDisposable
    {
        public int[] Array {get; set; }
        public int ArrayLength {get; set; }
        
        public SimilarPairsFramework(int arrayLength) {
            Array = new int[arrayLength];
            this.ArrayLength = arrayLength;
        }

        public void GenerateRandomArray() {
            Random randNum = new Random();
            for (int i = 0; i < ArrayLength; i++) 
            {
                Array[i] = randNum.Next(1, 20);
            }
        }

        public void Dispose() {
            //this.Dispose();
        }
    }
}