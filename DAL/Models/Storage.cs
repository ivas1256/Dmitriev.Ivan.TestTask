using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dmitriev.Ivan.TestTask.DAL.Models
{
    public class Storage
    {
        public int StorageId { get; set; }
        public int PharmacyId { get; set; }
        public string Name { get; set; }

        public Storage()
        {
        }

        public static IEnumerable<Storage> GetAllEnities()
        {
            using (var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader("SElECT storage_id, pharmacy_id, Name FROM Storage"))
            {
                while (reader.Read())
                {
                    var prod = new Storage
                    {
                        StorageId = reader.GetInt32(0),
                        PharmacyId = reader.GetInt32(1),
                        Name = reader.GetString(2)
                    };

                    yield return prod;
                }
            }
        }

        public void Create()
        {
            DBConnectionSingleton.GetInstance().ExecSql(
                "INSERT INTO Storage (pharmacy_id, name) VALUES (@pharmacyId, @name)",
                new System.Data.SqlClient.SqlParameter("@name", Name),
                new System.Data.SqlClient.SqlParameter("@pharmacyId", PharmacyId)
                );
        }

        public void Delete()
        {
            var db = DBConnectionSingleton.GetInstance();
            using (var tran = db.GetTransaction())
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", StorageId)
                };

                try
                {
                    db.ExecSql("DELETE FROM Supply WHERE storage_id = @id", param, tran);
                    db.ExecSql("DELETE FROM Storage WHERE storage_id = @id", param, tran);

                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    throw;
                }
            }            
        }
    }
}
