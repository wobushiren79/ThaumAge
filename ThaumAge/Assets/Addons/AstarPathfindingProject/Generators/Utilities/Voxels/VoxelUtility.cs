using UnityEngine;
using System.Collections.Generic;
using Unity.Jobs;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Burst;

namespace Pathfinding.Voxels {
	using Pathfinding.Util;

	/// <summary>Various utilities for voxel rasterization.</summary>
	public class Utility {
		public static float Min (float a, float b, float c) {
			a = a < b ? a : b;
			return a < c ? a : c;
		}

		public static float Max (float a, float b, float c) {
			a = a > b ? a : b;
			return a > c ? a : c;
		}

		/// <summary>
		/// Removes duplicate vertices from the array and updates the triangle array.
		/// Returns: The new array of vertices
		/// </summary>
		public static Int3[] RemoveDuplicateVertices (Int3[] vertices, int[] triangles) {
			for (int i = 0; i < triangles.Length; i++) {
				if (triangles[i] >= vertices.Length) {
					Debug.Log("Out of range triangle " + triangles[i] + " >= " + vertices.Length);
				}
			}
			// Get a dictionary from an object pool to avoid allocating a new one
			var firstVerts = ObjectPoolSimple<Dictionary<Int3, int> >.Claim();

			firstVerts.Clear();

			// Remove duplicate vertices
			var compressedPointers = new int[vertices.Length];

			int count = 0;
			for (int i = 0; i < vertices.Length; i++) {
				if (!firstVerts.ContainsKey(vertices[i])) {
					firstVerts.Add(vertices[i], count);
					compressedPointers[i] = count;
					vertices[count] = vertices[i];
					count++;
				} else {
					// There are some cases, rare but still there, that vertices are identical
					compressedPointers[i] = firstVerts[vertices[i]];
				}
			}

			firstVerts.Clear();
			ObjectPoolSimple<Dictionary<Int3, int> >.Release(ref firstVerts);

			for (int i = 0; i < triangles.Length; i++) {
				triangles[i] = compressedPointers[triangles[i]];
			}

			var compressed = new Int3[count];
			for (int i = 0; i < count; i++) compressed[i] = vertices[i];
			return compressed;
		}

		/// <summary>Removes duplicate vertices from the array and updates the triangle array.</summary>
		[BurstCompile]
		public struct JobRemoveDuplicateVertices : IJob {
			[ReadOnly]
			public NativeList<Int3> vertices;
			[ReadOnly]
			public NativeList<int> triangles;
			public unsafe UnsafeAppendBuffer* outputVertices; // Element Type Int3
			public unsafe UnsafeAppendBuffer* outputTriangles; // Element Type int

			public void Execute () {
				unsafe {
					outputVertices->Reset();
					outputTriangles->Reset();

					var firstVerts = new NativeHashMap<Int3, int>(vertices.Length, Allocator.Temp);

					// Remove duplicate vertices
					var compressedPointers = new NativeArray<int>(vertices.Length, Allocator.Temp, NativeArrayOptions.UninitializedMemory);

					int count = 0;

					for (int i = 0; i < vertices.Length; i++) {
						if (!firstVerts.ContainsKey(vertices[i])) {
							firstVerts.Add(vertices[i], count);
							compressedPointers[i] = count;
							outputVertices->Add(vertices[i]);
							count++;
						} else {
							// There are some cases, rare but still there, that vertices are identical
							compressedPointers[i] = firstVerts[vertices[i]];
						}
					}

					for (int i = 0; i < triangles.Length; i++) {
						outputTriangles->Add(compressedPointers[triangles[i]]);
					}
				}
			}
		}

		[BurstCompile(FloatMode = FloatMode.Fast)]
		public struct JobTransformTileCoordinates : IJob {
			// Element type Int3
			public unsafe UnsafeAppendBuffer* vertices;
			// Element type Int3
			public unsafe UnsafeAppendBuffer* verticesInGraphSpace;
			public Matrix4x4 voxelToGraphSpace;
			public Matrix4x4 voxelToWorldSpace;

			public void Execute () {
				unsafe {
					verticesInGraphSpace->ResizeUninitialized(vertices->Length);
					int vertexCount = vertices->Length / UnsafeUtility.SizeOf<Int3>();
					for (int i = 0; i < vertexCount; i++) {
						// Transform from voxel indices to a proper Int3 coordinate, then convert it to a Vector3 float coordinate
						var vPtr1 = (Int3*)vertices->Ptr + i;
						var vPtr2 = (Int3*)verticesInGraphSpace->Ptr + i;
						var p = (Vector3)((*vPtr1) * Int3.Precision);
						*vPtr1 = (Int3)voxelToWorldSpace.MultiplyPoint3x4(p);
						*vPtr2 = (Int3)voxelToGraphSpace.MultiplyPoint3x4(p);
					}
				}
			}
		}
	}
}
