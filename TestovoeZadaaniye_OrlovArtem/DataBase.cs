using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TestovoeZadaaniye_OrlovArtem
{
    internal class DataBase
    {
        private SqlConnection sqlConnection;

        // Конструктор с возможностью передать строку подключения
        public DataBase(string connectionString)
        {
            sqlConnection = new SqlConnection(connectionString);
        }

        // Открытие соединения
        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
                sqlConnection.Open();
        }

        // Закрытие соединения
        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
                sqlConnection.Close();
        }

        // Выполнение SQL-запроса, который возвращает данные
        public SqlDataReader ExecuteQuery(string query)
        {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            SqlDataReader reader = command.ExecuteReader();
            return reader;
        }

        // Выполнение команды для изменения структуры БД
        public void ExecuteNonQuery(string query)
        {
            OpenConnection();
            SqlCommand command = new SqlCommand(query, sqlConnection);
            command.ExecuteNonQuery();
            CloseConnection();
        }

    }
}
