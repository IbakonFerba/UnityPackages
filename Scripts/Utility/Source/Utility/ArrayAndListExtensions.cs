using System.Collections.Generic;
using UnityEngine;


namespace FK.Utility.ArraysAndLists
{
    /// <summary>
    /// Extension Methods for manipulating and using Arrays and Lists
    /// 
    /// v1.1 06/2018
    /// Written by Fabian Kober
    /// fabian-kober@gmx.net
    /// </summary>
    public static class ArrayAndListExtensions
    {
        #region SHUFFEL
        /// <summary>
        /// Shuffels the given Array using the Fisher Yates Algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        public static void Shuffle<T>(this T[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int index = Random.Range(0, i);

                //swap
                T tmp = array[index];
                array[index] = array[i];
                array[i] = tmp;
            }
        }


        /// <summary>
        /// Shuffels the given Array using the Fisher Yates Algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this List<T> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int index = Random.Range(0, i);

                //swap
                T tmp = list[index];
                list[index] = list[i];
                list[i] = tmp;
            }
        }
        #endregion


        #region SEARCH
        /// <summary>
        /// Returns the index of the first occurence of the given element or -1 if the element was not found
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="element"></param>
        /// <returns></returns>
        public static int Search<T>(this T[] array, T element)
        {
            for(int i = 0; i < array.Length; ++i)
            {
                T elm = array[i];
                if(elm.Equals(element))
                {
                    return i;
                }
            }

            return -1;
        }
        #endregion


        #region GET_OBJECT
        /// <summary>
        /// Returns a random object from the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this T[] array)
        {
            int randomIndex = Random.Range(0, array.Length);

            return array[randomIndex];
        }

        /// <summary>
        /// Returns a random object from the array
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandom<T>(this List<T> list)
        {
            int randomIndex = Random.Range(0, list.Count);

            return list[randomIndex];
        }


        /// <summary>
        /// Returns an object from the given Array/list using given probabilites
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        public static T GetWithProbability<T>(this T[] objects, float[] probabilities)
        {
            if (objects.Length != probabilities.Length) //make sure there are as many probabilities as objects
            {
                throw new System.ArgumentException("Objects must have the same count as probabilities");
            }
            else
            {

                T[] objs = new T[100]; //make the array that is later used to choose an object

                int lastObjsIndex = -1; //the last index something got added

                for (int i = 0; i < objects.Length; i++)
                {
                    int objectCount = Mathf.FloorToInt(probabilities[i] * 100); //determine how many times this object should be in the array

                    for (int j = 0; j < objectCount; j++) //ad the object to the array
                    {
                        int objsIndex = lastObjsIndex + 1;
                        objs[objsIndex] = objects[i];
                        lastObjsIndex = objsIndex;
                    }
                }

                return objs[Random.Range(0, objs.Length)]; //choose an object
            }
        }


        /// <summary>
        /// Returns an object from the given Array/list using given probabilites
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        public static T GetWithProbability<T>(this List<T> objects, List<float> probabilities)
        {
            T[] objectsArray = objects.ToArray();
            float[] probArray = probabilities.ToArray();

            return objectsArray.GetWithProbability(probArray);
        }


        /// <summary>
        /// Returns an object from the given Array/list using given probabilites
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        public static T GetWithProbability<T>(this T[] objects, List<float> probabilities)
        {
            float[] probArray = probabilities.ToArray();

            return objects.GetWithProbability(probArray);
        }


        /// <summary>
        /// Returns an object from the given Array/list using given probabilites
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="objects"></param>
        /// <param name="probabilities"></param>
        /// <returns></returns>
        public static T GetWithProbability<T>(this List<T> objects, float[] probabilities)
        {
            T[] objectsArray = objects.ToArray();

            return objectsArray.GetWithProbability(probabilities);
        }

        #endregion


        #region ARRAY_MANIPULATION
        /// <summary>
        /// Returns a new array that is the old array cut of at the given index (exclusive)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static T[] CutAt<T>(this T[] array, int index)
        {
            T[] newArray = new T[index + 1];

            for(int i = 0; i <= index; i++)
            {
                newArray[i] = array[i];
            }

            return newArray;
        }
        #endregion

        #region LOGIC
        /// <summary>
        /// ANDs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool AND(this bool[] values)
        {
            bool returnValue = true;

            foreach(bool value in values)
            {
                returnValue &= value;
            }

            return returnValue;
        }

        /// <summary>
        /// ANDs all the values of the list together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool AND(this List<bool> values)
        {
            bool returnValue = true;

            foreach (bool value in values)
            {
                returnValue &= value;
            }

            return returnValue;
        }

        /// <summary>
        /// ORs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool OR(this bool[] values)
        {
            bool returnValue = false;

            foreach (bool value in values)
            {
                returnValue |= value;
            }

            return returnValue;
        }

        /// <summary>
        /// ORs all the values of the list together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool OR(this List<bool> values)
        {
            bool returnValue = false;

            foreach (bool value in values)
            {
                returnValue |= value;
            }

            return returnValue;
        }

        /// <summary>
        /// XORs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool XOR(this bool[] values)
        {
            bool returnValue = false;

            foreach (bool value in values)
            {
                returnValue ^= value;
            }

            return returnValue;
        }

        /// <summary>
        /// XORs all the values of the list together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool XOR(this List<bool> values)
        {
            bool returnValue = false;

            foreach (bool value in values)
            {
                returnValue ^= value;
            }

            return returnValue;
        }

        /// <summary>
        /// NANDs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NAND(this bool[] values)
        {
            return !values.AND();
        }

        /// <summary>
        /// NANDs all the values of the list together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NAND(this List<bool> values)
        {
            return !values.AND();
        }

        /// <summary>
        /// NORs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NOR(this bool[] values)
        {
            return !values.OR();
        }

        /// <summary>
        /// NORs all the values of the lsit together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool NOR(this List<bool> values)
        {
            return !values.OR();
        }

        /// <summary>
        /// NXORs all the values of the array together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool XNOR(this bool[] values)
        {
            return !values.XOR();
        }

        /// <summary>
        /// NXORs all the values of the list together
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool XNOR(this List<bool> values)
        {
            return !values.XOR();
        }

        #endregion
    }
}