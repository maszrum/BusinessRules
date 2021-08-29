namespace Shared.Domain
{
    public class BusinessRuleSuccess : IBusinessRuleResult
    {
        public bool IsFulfilled => true;
    }
}