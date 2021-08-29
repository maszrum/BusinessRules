using Shared.Domain;

namespace Companies.Domain
{
    public class DuplicatedNipError : IBusinessRuleResult
    {
        public bool IsFulfilled => false;
    }
}