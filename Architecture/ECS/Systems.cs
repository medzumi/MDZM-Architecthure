using System.Collections.Generic;

namespace Architecture.ECS
{
    public class Systems : IExecuteSystem
    {
        private readonly MContext _context;
        private readonly List<IExecuteSystem> _executeSystems;
        private readonly List<ICleanupSystem> _cleanupSystems;

        public Systems(MContext context, List<IExecuteSystem> executeSystems, List<ICleanupSystem> cleanupSystems)
        {
            _context = context;
            _executeSystems = executeSystems;
            _cleanupSystems = cleanupSystems;
        }

        public void Execute()
        {
            foreach (var executeSystem in _executeSystems)
            {
                executeSystem.Execute();
            }

            foreach (var cleanupSystem in _cleanupSystems)
            {
                cleanupSystem.Cleanup();
            }
        }
    }
}