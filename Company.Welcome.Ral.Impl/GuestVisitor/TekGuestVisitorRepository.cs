using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Storage;
using SQLite.Net;
using SQLite.Net.Platform.WinRT;
using Company.Welcome.Entities.GuestVisitor;

namespace Company.Welcome.Ral.GuestVisitor
{
    public class TekGuestVisitorRepository : ITekGuestVisitorRepository<VisitorEntity>
    {
        private static SQLiteConnection DbConnection => new SQLiteConnection(new SQLitePlatformWinRT(),
            Path.Combine(ApplicationData.Current.LocalFolder.Path, "Storage.sqlite"));

        public VisitorEntity GetbyId(Guid id)
        {
            using (var db = DbConnection)
            {
                var visitorsTable = db.CreateTable<VisitorEntity>();
                var visitors = (from p in db.Table<VisitorEntity>() select p).ToList();
                return visitors.FirstOrDefault(v => v.Id == id);
            }
        }

        public IEnumerable<VisitorEntity> GetAll()
        {
            using (var db = DbConnection)
            {
                var visitorsTable = db.CreateTable<VisitorEntity>();
                var visitors = (from p in db.Table<VisitorEntity>() select p).ToList();
                return visitors;
            }
        }

        public void Delete(Guid id)
        {
            var visitortoDelete = GetbyId(id);
            if (visitortoDelete == null) return;
            using (var db = DbConnection)
            {
                var visitorsTable = db.CreateTable<VisitorEntity>();
                db.Delete<Visitor>(visitortoDelete);
            }
        }

        public void InsertOrUpdate(VisitorEntity entity)
        {
            var existingentity = GetbyId(entity.Id);
            if (existingentity == null)
            {
                using (var db = DbConnection)
                {
                    var visitorsTable = db.CreateTable<VisitorEntity>();
                    db.InsertOrReplace(entity);
                }
            }
            else
            {
                using (var db = DbConnection)
                {
                    var visitorsTable = db.CreateTable<VisitorEntity>();
                    db.Update(entity);
                }
            }
        }
    }
}