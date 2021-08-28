using System.Threading.Tasks;
using NUnit.Framework;
using Sandbox.Domain;
using Sandbox.Infrastructure;
using Sandbox.Shared.Application;

namespace Sandbox
{
    [TestFixture]
    public class Client
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly DomainInvoker _domainInvoker;
        
        public Client()
        {
            // objects normally created by DI container
            _companyRepository = new CompanyRepository();
            
            _domainInvoker = new DomainInvoker(
                _ => new NipCannotBeDuplicatedBusinessRule(_companyRepository));
        }
        
        [Test]
        public async Task Should_be_an_duplicated_nip_error()
        {
            var companyOne = new Company("Some company", "9785615346");
            var companyTwo = new Company("Some company", "7577261187");
            
            await _companyRepository.Create(companyOne);
            await _companyRepository.Create(companyTwo);
            
            var result = await _domainInvoker.Do(
                () => companyOne.SetNip("7577261187"));
            
            Assert.That(result, Is.TypeOf<DuplicatedNipError>());
        }
        
        [Test]
        public async Task Should_not_be_an_duplicated_nip_error()
        {
            var companyOne = new Company("Some company", "9785615346");
            var companyTwo = new Company("Some company", "7577261187");
            
            await _companyRepository.Create(companyOne);
            await _companyRepository.Create(companyTwo);
            
            var result = await _domainInvoker.Do(
                () => companyOne.SetNip("5248513155"));
            
            Assert.That(result.IsFulfilled, Is.True);
        }
    }
}