using Sandbox.Shared.Domain;

namespace Sandbox.Domain
{
    public class DuplicatedNipError : IBusinessRuleResult
    {
        public bool IsFulfilled => false;
    }
}