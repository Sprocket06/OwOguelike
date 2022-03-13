namespace OwOguelike.Collision
{
    static class cute_c2
    {
        internal const string LibraryName = "cute_c2";

        [StructLayout(LayoutKind.Sequential)]
        public struct c2v
        {
            public c2v(float x, float y)
            {
                this.x = x;
                this.y = y;
            }
            public static implicit operator c2v(Vector2 other) => new(other.X, other.Y);
            public static implicit operator Vector2(c2v other) => new(other.x, other.y);
            public float x;
            public float y;
        }
        

        [StructLayout(LayoutKind.Sequential)]
        public struct c2Circle
        {
            public c2Circle(c2v p, float r)
            {
                this.p = p;
                this.r = r;
            }
            public c2v p;
            public float r;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct c2AABB
        {
            public c2AABB(c2v min, c2v max)
            {
                this.min = min;
                this.max = max;
            }
            public c2v min;
            public c2v max;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct c2Manifold
        {
            public int count;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 2)]
            public float[] depths;

            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.Struct, SizeConst = 2)]
            public c2v[] contact_points;

            public c2v n;
        }

        //Boolean Collision Detection
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool c2AABBtoAABB(c2AABB a, c2AABB b);
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool c2CircletoAABB(c2Circle a, c2AABB b);
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool c2CircletoCircle(c2Circle a, c2Circle b);

        //Collision Detection w/ Manifold Generation
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void c2CircletoCircleManifold(c2Circle A, c2Circle B, ref c2Manifold manifold);
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void c2CircletoAABBManifold(c2Circle A, c2AABB B, ref c2Manifold manifold);
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void c2AABBtoAABBManifold(c2AABB A, c2AABB B, ref c2Manifold manifold);
    }
}