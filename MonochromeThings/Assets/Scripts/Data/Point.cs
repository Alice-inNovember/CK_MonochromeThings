using System;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data
{
	[System.Serializable]
	public struct Point
	{
		public int x;
		public int y;

		public Point(int x, int y)
		{
			this.x = x;
			this.y = y;
		}

		public static float Dist(Point p1, Point p2)
		{
			return math.sqrt(math.pow(p1.x - p2.x, 2) + math.pow(p1.y - p2.y, 2));
		}

		public Vector2 ToVector2(Point p)
		{
			return new Vector2(p.x, p.y);
		}

		public Vector2 ToVector2()
		{
			return new Vector2(x, y);
		}

		public Vector3 ToVector3()
		{
			return new Vector3(x, y, 0);
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			return obj.GetType() == this.GetType() && Equals((Point)obj);
		}
		private bool Equals(Point other)
		{
			return x == other.x && y == other.y;
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			return HashCode.Combine(x, y);
		}

		public static bool operator ==(Point p1, Point p2)
			=> ((p1.x == p2.x) && (p1.y == p2.y));

		public static bool operator !=(Point p1, Point p2)
			=> !((p1.x == p2.x) && (p1.y == p2.y));

		public static Point operator +(Point p1, Point p2)
			=> new Point(p1.x + p2.x, p1.y + p2.y);
		
		public static Point operator -(Point p1, Point p2)
			=> new Point(p1.x - p2.x, p1.y - p2.y);
	}
}