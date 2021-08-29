using System;

namespace Shared.Domain
{
    public class RuleInfo
    {
        public RuleInfo(Type type, object[] args)
        {
            Type = type;
            Args = args;
        }

        public Type Type { get; }
        
        public object[] Args { get; }
    }
}