using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Shared.Domain;

namespace Shared.Application
{
    public class DomainInvoker
    {
        private readonly Func<Type, object> _businessRulesFactory;

        public DomainInvoker(Func<Type, object> businessRulesFactory)
        {
            _businessRulesFactory = businessRulesFactory;
        }

        public Task<IBusinessRuleResult> Do(Expression<Func<Task<IBusinessRuleResult>>> expression)
        {
            var expressionObject = ExtractExpressionObject(expression);

            if (expressionObject is not DomainEntity entity)
            {
                throw new ArgumentException(
                    $"specified method that is not called on {nameof(DomainEntity)} object");
            }
            
            entity.SetupCheckRulesAction(
                (rulesToCheck) => rulesToCheck.Count == 0 
                    ? Task.FromResult((IBusinessRuleResult)new BusinessRuleSuccess()) 
                    : CheckRules(rulesToCheck));
            
            var expressionCompiled = expression.Compile();
            return expressionCompiled();
        }

        private async Task<IBusinessRuleResult> CheckRules(IEnumerable<RuleInfo> ruleInfos)
        {
            foreach (var ruleInfo in ruleInfos)
            {
                var rule = _businessRulesFactory(ruleInfo.Type);
                
                const string? methodName = nameof(BusinessRule<object>.Check);
                var checkMethod = ruleInfo.Type.GetMethod(methodName)!;
                
                var task = checkMethod.Invoke(rule, ruleInfo.Args);
                if (task is null)
                {
                    throw new InvalidOperationException(
                        $"{nameof(methodName)} in class {ruleInfo.Type.Name} cannot return null");
                }
                
                var taskTyped = (Task<IBusinessRuleResult>)task;
                
                var ruleResult = await taskTyped;
                
                if (!ruleResult.IsFulfilled)
                {
                    return ruleResult;
                }
            }

            return new BusinessRuleSuccess();
        }

        private static object ExtractExpressionObject(Expression<Func<Task<IBusinessRuleResult>>> expression)
        {
            var body = (MethodCallExpression)expression.Body;

            if (body.Object is null)
            {
                throw new ArgumentException(
                    "specified invalid expression", nameof(expression));
            }

            var memberExpression = (MemberExpression)body.Object;

            if (memberExpression.Expression is null)
            {
                throw new ArgumentException(
                    "specified invalid expression", nameof(expression));
            }

            var constantExpression = (ConstantExpression)memberExpression.Expression;

            if (constantExpression.Value is null)
            {
                throw new ArgumentException(
                    "specified invalid expression", nameof(expression));
            }

            var fieldInfo = constantExpression.Value
                .GetType()
                .GetField(
                    name: memberExpression.Member.Name,
                    bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            if (fieldInfo is null)
            {
                throw new ArgumentException(
                    "specified invalid expression", nameof(expression));
            }

            var expressionObject = fieldInfo.GetValue(constantExpression.Value)!;
            
            return expressionObject;
        }
    }
}