using System.Threading.Tasks;
using Sandbox.Shared.Domain;

namespace Sandbox.Domain
{
    public class NipCannotBeDuplicatedBusinessRule : BusinessRule<string>
    {
        private readonly ICompanyRepository _companyRepository;

        public NipCannotBeDuplicatedBusinessRule(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        public override async Task<IBusinessRuleResult> Check(string nip)
        {
            var alreadyExists = await _companyRepository.NipExists(nip);
            
            if (alreadyExists)
            {
                return new DuplicatedNipError();
            }
            
            return Ok();
        }
    }
}