namespace Architecture.ECS
{
    public class MEntity
    {
        internal int index;
        internal MContext _context;

        public MContext Context => _context;
        public int Index => index;
    }
}