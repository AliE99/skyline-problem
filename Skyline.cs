using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SkylineProblem
{
    public class Skyline
    {
        public static void Main(string[] args)
        {
            
            int[,] building =
            {
                {2, 9, 10}, {3, 6, 15}, {5, 12, 12}, {13, 16, 10},
                {15, 17, 5}
            };
            
            Skyline skyline = new Skyline();

            List<int[]> skylines = skyline.GetSkyline(0, building.Length / 3 - 1, building);

            /* for (int i = 0; i < skylines.Count; i++)
             {
                 Console.WriteLine(skylines.ElementAt(i)[0] + " , " + skylines.ElementAt(i)[1]);
             }*/
            
            string json = JsonConvert.SerializeObject(skylines.ToArray());
            System.IO.File.WriteAllText(@"D:\workspace\SkylineProblem\js\skylineFile.json", json);
            Console.WriteLine("Saved into the Json File.");
        }
        
        private List<int[]> GetSkyline(int start, int end, int[,] building)
        {
            //This List Holds the "KeyPoints"
            List<int[]> skylineKeyPointList = new List<int[]>();
            //Wrong Case
            if (start > end)
            {
                return skylineKeyPointList;
            }
            //In this Case we have one building
            else if (end == start) 
            {
                int x1 = building[start, 0];
                int x2 = building[start, 1];
                int height = building[start, 2];
                int[] firstKeyPoint = {x1, height};
                int[] secondKeyPoint = {x2, 0};
                skylineKeyPointList.Add(firstKeyPoint);
                skylineKeyPointList.Add(secondKeyPoint);
                return skylineKeyPointList;
            }
            
            //In this case we should merge two Skylines
            else 
            {
                int middle = (start + end) / 2;
                List<int[]> firstSkyline = GetSkyline(start, middle, building);
                List<int[]> secondSkyline = GetSkyline(middle + 1, end, building);
                return MergeSkylines(firstSkyline, secondSkyline);
            }
        }

        private List<int[]> MergeSkylines(List<int[]> firstSkyline, List<int[]> secondSkyline)
        {
            int height1 = 0;
            int height2 = 0;
            List<int[]> mergedSkyline = new List<int[]>();
            while (true)
            {
                if (firstSkyline.Count == 0 || secondSkyline.Count == 0)
                {
                    break;
                }
                
                //keyPoint[0] = x ;  kePoint[1] = height
                int[] keyPoint1 = firstSkyline.ElementAt(0);
                int[] keyPoint2 = secondSkyline.ElementAt(0);

                //We should merge two keyPoints
                int[] mergedKeyPoint = new int[2];

                //Now We have 3 Cases according to the solution
                if (keyPoint1[0] < keyPoint2[0])
                {
                    mergedKeyPoint[0] = keyPoint1[0];
                    mergedKeyPoint[1] = keyPoint1[1];

                    if (keyPoint1[1] < height2)
                    {
                        mergedKeyPoint[1] = height2;
                    }

                    height1 = keyPoint1[1];
                    firstSkyline.RemoveAt(0);
                }
                else if (keyPoint1[0] > keyPoint2[0])
                {
                    mergedKeyPoint[0] = keyPoint2[0];
                    mergedKeyPoint[1] = keyPoint2[1];

                    if (keyPoint2[1] < height1)
                    {
                        mergedKeyPoint[1] = height1;
                    }

                    height2 = keyPoint2[1];
                    secondSkyline.RemoveAt(0);
                }
                else
                {
                    mergedKeyPoint[0] = keyPoint2[0];
                    mergedKeyPoint[1] = Greater(keyPoint1[1], keyPoint2[1]);
                    height1 = keyPoint1[1];
                    height2 = keyPoint2[1];
                    firstSkyline.RemoveAt(0);
                    secondSkyline.RemoveAt(0);
                }

                mergedSkyline.Add(mergedKeyPoint);
            }

            if (firstSkyline.Count == 0)
            {
                while (secondSkyline.Count != 0)
                {
                    mergedSkyline.Add(secondSkyline[0]);
                    secondSkyline.RemoveAt(0);
                }
            }
            else
            {
                while (firstSkyline.Count != 0)
                {
                    mergedSkyline.Add(firstSkyline[0]);
                    firstSkyline.RemoveAt(0);
                }
            }

            // remove redundant key points from merged skyline
            int current = 0;
            while (current < mergedSkyline.Count)
            {
                bool dupeFound = true;
                int i = current + 1;
                while ((i < mergedSkyline.Count) && dupeFound)
                {
                    if (mergedSkyline.ElementAt(current)[1] == mergedSkyline.ElementAt(i)[1])
                    {
                        dupeFound = true;
                        mergedSkyline.RemoveAt(i);
                    }
                    else
                    {
                        dupeFound = false;
                    }
                }

                current += 1;
            }

            return mergedSkyline;
        }

        private int Greater(int first, int second)
        {
            if (first > second)
                return first;
            return second;
        }
    }
}