using System.Threading.Tasks;

namespace Companies.Domain
{
    public interface ICompanyRepository
    {
        Task<Company> Get(string name);
        Task Create(Company company);
        Task<bool> NipExists(string nip);
    }
}