using System;
using System.Collections.Generic;
using Dmitriev.Ivan.TestTask.DAL;
using Dmitriev.Ivan.TestTask.DAL.Models;

namespace Dmitriev.Ivan.TestTask.Reports
{
    public class TotalProductAmount_Report
    {
        int pharmacyId;

        public TotalProductAmount_Report(int pharmacyId)
        {
            this.pharmacyId = pharmacyId;
        }

        public Dictionary<Product, int> GetResult()
        {
            var sql = @"
                    SELECT
                        p.product_id, p.name,
                        SUM(s.quantity)
                    FROM Supply s
                        JOIN Storage st ON st.storage_id = s.storage_id
                            and st.pharmacy_id = @pharmId
                        JOIN Product p on p.product_id = s.product_id
                    GROUP BY p.product_id, p.name";

            var dict = new Dictionary<Product, int>();
            using (var reader = DBConnectionSingleton.GetInstance().ExecSqlWithReader(sql, new System.Data.SqlClient.SqlParameter("@pharmId", pharmacyId)))
            {
                while (reader.Read())
                {
                    dict.Add(new Product
                    {
                        ProductId = reader.GetInt32(0),
                        Name = reader.GetString(1)
                    }, reader.GetInt32(2));
                }
            }

            return dict;
        }
    }
}
