using System.Collections.Generic;

namespace Architecture.ECS
{
    public class Systems : IExecuteSystem
    {
        private readonly MContext _context;
        private readonly List<IExecuteSystem> _executeSystems;

        public Systems(MContext context, List<IExecuteSystem> executeSystems)
        {
            _context = context;
            _executeSystems = executeSystems;
        }

        public void Execute()
        {
            _context.Update();
            foreach (var executeSystem in _executeSystems)
            {
                executeSystem.Execute();
            }
            _context.Complete();
        }
    }
}