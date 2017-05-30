using Capiche.ActiveStateMachine;

namespace Statemachine
{
    public class TransWorkStateMachine : ActiveStateMachine
    {

        private TransWorkStateMachineConfiguration _config;

        public TransWorkStateMachine(TransWorkStateMachineConfiguration configuration)
            : base(configuration.TransWorkStateMachineStateList, configuration.MaxEntries)
        {
            _config = configuration;

            configuration.DoEventMappings(this, _config.TransWorkActivities);
        }
    }
}
