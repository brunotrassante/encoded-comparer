﻿using Dapper;
using EncodedComparer.Domain.Entities;
using EncodedComparer.Domain.Queries;
using EncodedComparer.Domain.Repository;
using EncodedComparer.Infra.DataContexts;
using System.Threading.Tasks;

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
                                VALUES (@id,@data)",
                new
                {
                    id = encodedData.Id,
                    data = encodedData.Data
                });
        }

        public async Task CreateRight(Base64Data encodedData)
        {
            await _context
                 .Connection
                 .ExecuteAsync(@"INSERT INTO RightData 
                                (Id, Base64EncodedData)
                                VALUES (@id,@data)",
                new
                {
                    id = encodedData.Id,
                    data = encodedData.Data
                });
        }

        public async Task<LeftRightSameIdQuery> GetLeftRightById(int id)
        {
            return await _context
            .Connection
            .QueryFirstOrDefaultAsync<LeftRightSameIdQuery>(@"SELECT l.Id, l.Base64EncodedData as [Left], r.Base64EncodedData as [Right]
                                                                FROM LeftData l 
                                                                    LEFT JOIN RightData r on r.Id = l.Id
                                                                WHERE l.Id = @id
                                                                UNION
                                                                SELECT r.Id, l.Base64EncodedData as [Left], r.Base64EncodedData as [Right]
                                                                FROM RightData r  
                                                                    LEFT JOIN LeftData l on r.Id = l.Id
                                                                WHERE r.Id = @id",
                                             new { id = id });
        }

        public async Task<bool> LeftExists(int id)
        {
            return await _context
               .Connection
               .QueryFirstOrDefaultAsync<bool>(@"SELECT CAST(COUNT(1) AS bit) 
                                                FROM LeftData 
                                                WHERE Id = @id",
                                                new { id = id });
        }

        public async Task<bool> RightExists(int id)
        {
            return await _context
              .Connection
              .QueryFirstOrDefaultAsync<bool>(@"SELECT CAST(COUNT(1) AS bit) 
                                                FROM RightData 
                                                WHERE Id = @id",
                                                new { id = id });
        }

        public async Task DeleteById(int id)
        {
            await _context
             .Connection
             .ExecuteAsync(@"DELETE FROM LeftData WHERE Id = @id;
                             DELETE FROM RightData WHERE Id = @id;",
                             new { id = id });
        }
    }
}
