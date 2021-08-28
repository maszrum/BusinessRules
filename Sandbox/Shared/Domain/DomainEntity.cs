using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Sandbox.Shared.Domain
{
    public abstract class DomainEntity
    {
        private readonly List<(Type, object[])> _rulesToCheck = new();
        private Func<DomainEntity, Task<IBusinessRuleResult>>? _checkRulesAction;
        private IBusinessRuleResult? _ruleResult;
        
        public IReadOnlyList<(Type, object[])> RulesToCheck => _rulesToCheck;
        
        public void SetupCheckRulesAction(Func<DomainEntity, Task<IBusinessRuleResult>> action)
        {
            _checkRulesAction = action;
        }
        
        protected void CheckRule<TRule, TArg>(TArg arg) 
            where TRule : BusinessRule<TArg> 
            where TArg : notnull
        {
            _rulesToCheck.Add((typeof(TRule), new object[] { arg }));
        }
        
        protected void CheckRule<TRule, TArg1, TArg2>(TArg1 arg1, TArg2 arg2) 
            where TRule : BusinessRule<TArg1, TArg2> 
            where TArg1 : notnull
            where TArg2 : notnull
        {
            _rulesToCheck.Add((typeof(TRule), new object[] { arg1, arg2 }));
        }
        
        protected void CheckRule<TRule, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3) 
            where TRule : BusinessRule<TArg1, TArg2, TArg3> 
            where TArg1 : notnull
            where TArg2 : notnull
            where TArg3 : notnull
        {
            _rulesToCheck.Add((typeof(TRule), new object[] { arg1, arg2, arg3 }));
        }
        
        protected async Task<bool> IsAnyRuleBroken()
        {
            if (_checkRulesAction is null)
            {
                throw new InvalidOperationException(
                    $"method has not been called with DomainInvoker");
            }
            
            _ruleResult = await _checkRulesAction(this);
            
            return !_ruleResult.IsFulfilled;
        }
        
        protected IBusinessRuleResult GetRuleResult()
        {
            if (_ruleResult is null)
            {
                throw new InvalidOperationException(
                    $"rule result is not ready, {nameof(IsAnyRuleBroken)} was not called or completed");
            }
            return _ruleResult;
        }
        
        protected IBusinessRuleResult Ok() => new BusinessRuleSuccess();
    }
}