using System.Collections.Generic;

namespace Architecture.ViewModel
{
    public class ProxyCommand : ICommand
    {
        private readonly List<ICommand> _commands = new List<ICommand>();
        private readonly List<ICommand> _cacheExecutedCommands = new List<ICommand>();

        public void AddCommand(ICommand command)
        {
            _commands.Add(command);
        }

        public void RemoveCommand(ICommand command)
        {
            _commands.Remove(command);
        }

        public void Clear()
        {
            _commands.Clear();
        }
    
        public void Execute()
        {
            _cacheExecutedCommands.AddRange(_commands);
            foreach (var VARIABLE in _cacheExecutedCommands)
            {
                VARIABLE.Execute();
            }
            _cacheExecutedCommands.Clear();
        }
    }
}
