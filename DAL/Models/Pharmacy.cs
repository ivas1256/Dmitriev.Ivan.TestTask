using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dmitriev.Ivan.TestTask.DAL.Models
{
    /// <summary>
    /// Классы моделей БД. Реализуют логику работы с базой и хранение данных
    /// </summary>
    public class Pharmacy
    {
        public int PharmacyId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public long Phone { get; set; }

        public Pharmacy()
        {    
        }

        public static IEnumerable<Pharmacy> GetAllEnities()
        {
            using (var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader("SElECT pharmacy_id, Name, Address, Phone FROM Pharmacy"))
            {
                while (reader.Read())
                {
                    var prod = new Pharmacy
                    {
                        PharmacyId = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        Address = reader.GetString(2),
                        Phone = reader.GetInt64(3)
                    };

                    yield return prod;
                }
            }
        }

        public void Create()
        {
            DBConnectionSingleton.GetInstance().ExecSql(
                "INSERT INTO Pharmacy (name, address, phone) VALUES (@name, @address, @phone)",
                new System.Data.SqlClient.SqlParameter("@name", Name),
                new System.Data.SqlClient.SqlParameter("@address", Address),
                new System.Data.SqlClient.SqlParameter("@phone", Phone)
                );
        }

        public void Delete()
        {
            var db = DBConnectionSingleton.GetInstance();
            using(var tran = db.GetTransaction())
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", PharmacyId)
                };

                try
                {
                    db.ExecSql("DELETE FROM Supply WHERE pharmacy_id = @id", param, tran);
                    db.ExecSql("DELETE FROM Storage WHERE pharmacy_id = @id", param, tran);
                    db.ExecSql("DELETE FROM Pharmacy WHERE pharmacy_id = @id", param, tran);

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
