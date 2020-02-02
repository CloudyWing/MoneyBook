using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using MoneyBook.Repositories;

namespace MoneyBook.Services.RecordModel {

    public class RecordService : ServiceBase, IRecordService {
        private readonly IRepository<Record> recordRepository;

        public RecordService(IRepository<Record> recordRepository) => this.recordRepository = recordRepository ?? throw new ArgumentNullException(nameof(recordRepository));

        public void Create(string userId, RecordDto instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(RecordDto));
            }

            DateTime now = DateTime.Now;

            Record record = Mapper.Map<Record>(instance);
            record.Id = Guid.NewGuid();
            record.CreatedTime = now;
            record.ModifiedTime = now;
            recordRepository.Create(record);

            recordRepository.SaveChanges();
        }

        public void Update(string userId, Guid id, RecordDto instance) {
            if (instance == null) {
                throw new ArgumentNullException(nameof(RecordDto));
            }

            Record record = Mapper.Map(instance, Get(userId, id));
            record.ModifiedTime = DateTime.Now;
            recordRepository.Update(record);

            recordRepository.SaveChanges();
        }

        public void Delete(string userId, Guid id) {
            Record record = Get(userId, id);
            record.DeletedTime = DateTime.Now;
            recordRepository.Update(record);

            recordRepository.SaveChanges();
        }

        public Record Get(string userId, Guid id) {
            return CreateSingleQueryable(userId, id).Single();
        }

        public IQueryable<Record> Read(Expression<Func<Record, bool>> predicate = null) {
            IQueryable<Record> records = recordRepository.Read();
            if (predicate != null) {
                records = records.Where(predicate);
            }
            return records;
        }

        public bool VerifyPermission(string userId, Guid id)
            => CreateSingleQueryable(userId, id).Any();

        private IQueryable<Record> CreateSingleQueryable(string userId, Guid id) {
            return recordRepository.Read()
                .Include(x => x.CategoryItem)
                .Include(x => x.CategoryItem.Category)
                .SetNonDeleted()
                .Where(x => x.CategoryItem.Category.UserId == userId && x.Id == id);
        }
    }
}