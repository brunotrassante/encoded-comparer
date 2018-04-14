using EncodedComparer.Domain.Repository;
using EncodedComparer.Infra.DataContexts;
using System.Collections.Generic;
using System.Threading.Tasks;
using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Queries;
using Dapper;

namespace EncodedComparer.Infra.Repositories
{
    public class EncodedPairRepository : IEncodedPairRepository
    {
        private EncodedComparerContext _context;

        public EncodedPairRepository(EncodedComparerContext context)
        {
            _context = context;
        }

        public async Task CreateLeft(Base64Data encodedData)
        {
            await _context
                 .Connection
                 .ExecuteAsync(@"INSERT INTO LeftData 
                                (Id, Base64EncodedData)
                                VALUES (@Id,@Data)",
                new
                {
                    Id = encodedData.Id,
                    Data = encodedData.Data
                });
        }

        public async Task CreateRight(Base64Data encodedData)
        {
            await _context
                 .Connection
                 .ExecuteAsync(@"INSERT INTO RightData 
                                (Id, Base64EncodedData)
                                VALUES (@Id,@Data)",
                new
                {
                    Id = encodedData.Id,
                    Data = encodedData.Data
                });
        }

        public async Task<LeftRightSameIdQuery> GetLeftRightById(int id)
        {
            return await _context
            .Connection
            .QueryFirstOrDefaultAsync<LeftRightSameIdQuery>(@"SELECT CASE WHEN l.Id IS NOT NULL THEN l.Id 
			                                                                ELSE r.Id
			                                                                END as Id,
			                                                                l.Base64EncodedData as [Left], r.Base64EncodedData as [Right]
                                                                FROM LeftData l FULL OUTER JOIN 
                                                                     RightData r on r.Id = l.Id
                                                                WHERE l.Id = @id OR r.Id = @id",
                                             new { Id = id });
        }

        public async Task<bool> LeftExists(int id)
        {
            return await _context
               .Connection
               .QueryFirstOrDefaultAsync<bool>(@"SELECT CAST(COUNT(1) AS bit) 
                                                FROM LeftData 
                                                WHERE Id = @id",
                                                new { Id = id });
        }

        public async Task<bool> RightExists(int id)
        {
            return await _context
              .Connection
              .QueryFirstOrDefaultAsync<bool>(@"SELECT CAST(COUNT(1) AS bit) 
                                                FROM RightData 
                                                WHERE Id = @id",
                                                new { Id = id });
        }
    }
}
