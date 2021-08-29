using System.Threading.Tasks;
using Companies.Domain;
using Shared.Application;

namespace Companies.Application
{
    public class CompanyService // or command-handler
    {
        private readonly DomainInvoker _invoker;
        private readonly ICompanyRepository _companyRepository;

        public CompanyService(
            DomainInvoker invoker, 
            ICompanyRepository companyRepository)
        {
            _invoker = invoker;
            _companyRepository = companyRepository;
        }

        public async Task SetCompanyNip(SetCompanyNipDto dto)
        {
            var company = await _companyRepository.Get(dto.Name);
            
            var result = await _invoker.Do(
                () => company.SetNip(dto.Nip));
            
            if (!result.IsFulfilled)
            {
                // error handling
            }
            
            // success handling
        }
    }
}