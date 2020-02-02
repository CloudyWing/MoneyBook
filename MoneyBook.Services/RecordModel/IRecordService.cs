using System;
using System.Linq;
using System.Linq.Expressions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.RecordModel {
    public interface IRecordService {
        void Create(string userId, RecordDto instance);
        void Delete(string userId, Guid id);
        Record Get(string userId, Guid id);
        IQueryable<Record> Read(Expression<Func<Record, bool>> predicate = null);
        void Update(string userId, Guid id, RecordDto instance);
        bool VerifyPermission(string userId, Guid id);
    }
}