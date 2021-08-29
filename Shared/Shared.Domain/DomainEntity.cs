using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared.Domain
{
    public abstract class DomainEntity
    {
        private readonly List<RuleInfo> _rulesToCheck = new();
        private Func<IReadOnlyList<RuleInfo>, Task<IBusinessRuleResult>>? _checkRulesAction;
        private IBusinessRuleResult? _ruleResult;
        
        public void SetupCheckRulesAction(Func<IReadOnlyList<RuleInfo>, Task<IBusinessRuleResult>> action) => 
            _checkRulesAction = action;

        protected void CheckRule<TRule, TArg>(TArg arg) 
            where TRule : BusinessRule<TArg> 
            where TArg : notnull
        {
            var ruleInfo = new RuleInfo(typeof(TRule), new object[] { arg });
            _rulesToCheck.Add(ruleInfo);
        }
        
        protected void CheckRule<TRule, TArg1, TArg2>(TArg1 arg1, TArg2 arg2) 
            where TRule : BusinessRule<TArg1, TArg2> 
            where TArg1 : notnull
            where TArg2 : notnull
        {
            var ruleInfo = new RuleInfo(typeof(TRule), new object[] { arg1, arg2 });
            _rulesToCheck.Add(ruleInfo);
        }
        
        protected void CheckRule<TRule, TArg1, TArg2, TArg3>(TArg1 arg1, TArg2 arg2, TArg3 arg3) 
            where TRule : BusinessRule<TArg1, TArg2, TArg3> 
            where TArg1 : notnull
            where TArg2 : notnull
            where TArg3 : notnull
        {
            var ruleInfo = new RuleInfo(typeof(TRule), new object[] { arg1, arg2, arg3 });
            _rulesToCheck.Add(ruleInfo);
        }
        
        protected async Task<bool> IsAnyRuleBroken()
        {
            if (_checkRulesAction is null)
            {
                throw new InvalidOperationException(
                    $"method has not been called with DomainInvoker");
            }
            
            _ruleResult = await _checkRulesAction(_rulesToCheck);
            
            return !_ruleResult.IsFulfilled;
        }
        
        protected IBusinessRuleResult GetRuleResult()
        {
            if (_ruleResult is null)
            {
                throw new InvalidOperationException(
                    $"rule result is not ready, {nameof(IsAnyRuleBroken)} was not called or completed");
            }
            
            var result = _ruleResult;
            _ruleResult = null;
            
            return result;
        }
        
        protected IBusinessRuleResult Ok() => new BusinessRuleSuccess();
    }
}