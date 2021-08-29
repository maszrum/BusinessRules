using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Companies.Domain;

namespace Companies.Infrastructure
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly HashSet<Company> _companies = new();

        public async Task Add(Company company)
        {
            await Task.Delay(10);
            
            _companies.Add(company);
        }
        
        public async Task<Company> Get(string name)
        {
            await Task.Delay(10);
            
            var company = _companies.SingleOrDefault(c => c.Name == name);
            if (company is null)
            {
                throw new InvalidOperationException(
                    "company with specified name was not found");
            }
            
            return company;
        }

        public async Task Create(Company company)
        {
            await Task.Delay(10);
            
            _companies.Add(company);
        }

        public async Task<bool> NipExists(string nip)
        {
            await Task.Delay(10);
            
            return _companies.Any(c => c.Nip == nip);
        }
    }
}