using System.Threading.Tasks;

namespace Sandbox.Domain
{
    public interface ICompanyRepository
    {
        Task<Company> Get(string name);
        Task Create(Company company);
        Task<bool> NipExists(string nip);
    }
}