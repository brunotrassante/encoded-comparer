using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Queries;
using System.Threading.Tasks;

namespace EncodedComparer.Domain.Repository
{
    public interface IEncodedPairRepository
    {
        Task<bool> LeftExists(int id);
        Task<bool> RightExists(int id);
        Task CreateLeft(Base64Data encodedData);
        Task CreateRight(Base64Data encodedData);
        Task<LeftRightSameIdQuery> GetLeftRightById(int id);
        Task DeleteById(int id);
    }
}
