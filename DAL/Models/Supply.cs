using System;
using System.Collections.Generic;

namespace Dmitriev.Ivan.TestTask.DAL.Models
{
    public class Supply
    {
        public int SupplyId { get; set; }
        public int ProductId { get; set; }
        public int StorageId { get; set; }
        public int Quantity { get; set; }

        public string ProductName
        {
            get
            {
                return Product.GetById(ProductId).Name;
            }
        }

        public Supply()
        {
        }

        public static IEnumerable<Supply> GetAllEnities()
        {
            using (var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader("SElECT supply_id, product_id, storage_id, quantity FROM Supply"))
            {
                while (reader.Read())
                {
                    var prod = new Supply
                    {
                        SupplyId = reader.GetInt32(0),
                        ProductId = reader.GetInt32(1),
                        StorageId = reader.GetInt32(2),
                        Quantity = reader.GetInt32(3)
                    };

                    yield return prod;
                }
            }
        }

        public void Create()
        {
            DBConnectionSingleton.GetInstance().ExecSql(
                "INSERT INTO Supply (product_id, storage_id, quantity) VALUES (@productId, @storageId, @quantity)",
                new System.Data.SqlClient.SqlParameter("@productId", ProductId),
                new System.Data.SqlClient.SqlParameter("@storageId", StorageId),
                new System.Data.SqlClient.SqlParameter("@quantity", Quantity)
                );
        }

        public void Delete()
        {
            DBConnectionSingleton.GetInstance().ExecSql(
                "DELETE FROM Supply WHERE supplyId = @id",
                new System.Data.SqlClient.SqlParameter("@id", SupplyId)
                );
        }
    }
}
