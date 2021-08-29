using System.Threading.Tasks;
using Shared.Domain;

namespace Companies.Domain
{
    public class Company : DomainEntity
    {
        public Company(string name, string nip)
        {
            Name = name;
            Nip = nip;
        }

        public string Name { get; }
        
        public string Nip { get; private set; }
        
        public async Task<IBusinessRuleResult> SetNip(string nip)
        {
            CheckRule<NipCannotBeDuplicatedBusinessRule, string>(nip);
            // ...
            // other rules
            
            if (await IsAnyRuleBroken())
            {
                return GetRuleResult();
            }
            
            Nip = nip;
            
            return Ok();
        }

        public override int GetHashCode() => Name.GetHashCode();
    }
}