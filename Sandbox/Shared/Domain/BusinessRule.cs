using System.Threading.Tasks;

namespace Sandbox.Shared.Domain
{
    public abstract class BusinessRule
    {
        protected BusinessRuleSuccess Ok() => new();
    }

    public abstract class BusinessRule<TArg> : BusinessRule
    {
        public abstract Task<IBusinessRuleResult> Check(TArg arg);
    }

    public abstract class BusinessRule<TArg1, TArg2> : BusinessRule
    {
        public abstract Task<IBusinessRuleResult> Check(TArg1 arg1, TArg2 arg2);
    }
    
    public abstract class BusinessRule<TArg1, TArg2, TArg3> : BusinessRule
    {
        public abstract Task<IBusinessRuleResult> Check(TArg1 arg1, TArg2 arg2, TArg3 arg3);
    }
}