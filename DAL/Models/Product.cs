using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Dmitriev.Ivan.TestTask.DAL.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string Name { get; set; }

        public Product()
        {
        }

        public static Product GetById(int id)
        {
            using (var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader("SElECT product_id, name FROM Product WHERE product_id = @id",
                new System.Data.SqlClient.SqlParameter("@id", id)))
            {
                if (!reader.HasRows)
                    throw new Exception($"Товар с id = {id} не найден");

                reader.Read();
                var prod = new Product
                {
                    ProductId = reader.GetInt32(0),
                    Name = reader.GetString(1)
                };
                return prod;
            }
        }

        public static IEnumerable<Product> GetAllEnities()
        {
            using(var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader("SElECT product_id, name FROM Product"))
            {
                while (reader.Read())
                {
                    var prod = new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    };

                    yield return prod;
                }
            }
        }

        public void Create()
        {
            DBConnectionSingleton.GetInstance().ExecSql(
                "INSERT INTO Product (name) VALUES (@name)",
                new System.Data.SqlClient.SqlParameter("@name", Name)
                );
        }

        public void Delete()
        {
            var db = DBConnectionSingleton.GetInstance();
            using (var tran = db.GetTransaction())
            {
                var param = new List<SqlParameter>
                {
                    new SqlParameter("@id", ProductId)
                };

                try
                {
                    db.ExecSql("DELETE FROM Supply WHERE product_id = @id", param, tran);
                    db.ExecSql("DELETE FROM Product WHERE product_id = @id", param, tran);

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
