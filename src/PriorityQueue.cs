using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Threading;

namespace NEA___Maze_Wiz
{
    public class PriorityQueue<Coordinates, Priority> where Priority : IComparable<Priority> //ensures that TPriority values can get compared to other priority values by implementing Icomparable interface
    {
        private List<(Coordinates Point, Priority Priority)> points = new List<(Coordinates, Priority)>();

        public int Count => points.Count;

        public void Enqueue(Coordinates point, Priority priority)
        {
             points.Add((point, priority));
             points.Sort((x, y) => x.Priority.CompareTo(y.Priority)); //CompareTo can be used as the IComparable interface has been implemented
        }

        public Coordinates Dequeue()
        {
             Coordinates element = points[0].Point;
             points.RemoveAt(0);
             return element;
        }
    }
}
