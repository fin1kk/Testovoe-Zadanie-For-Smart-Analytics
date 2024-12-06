using Microsoft.Data.SqlClient;
using Microsoft.VisualBasic.FileIO;
using System.Security.Cryptography;
using System.Text;

namespace TestovoeZadaaniye_OrlovArtem
{
    public partial class Form1 : Form
    {
        //���������� �������� ��� ���� ��
        private static string Server = "HOME-PC\\SQLEXPRESS";
        private static string Database = "CompanyDB";
        private static string User = "Artem";
        private static string Pass = "Pas111";

        private DataBase dataBase = new DataBase($"Server={Server};Database={Database};User Id={User};Password={Pass};TrustServerCertificate=True;");

        private void GetTableNames() //������� ��������� �������� ������ � ��
        {
            try
            {
                string query = "SELECT * FROM INFORMATION_SCHEMA.TABLES";
                using (SqlDataReader reader = dataBase.ExecuteQuery(query))
                {
                    tablesList.Items.Clear();
                    while (reader.Read())
                    {
                        tablesList.Items.Add(reader["TABLE_NAME"].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������: {ex.Message}");
                MessageBox.Show($"������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Form1()
        {
            InitializeComponent();

            editTabButton.Enabled = false;
            delTabButton.Enabled = false;

            GetTableNames();
        }

        private void tablesList_SelectedIndexChanged(object sender, EventArgs e) //���������� ���������� ������
        {
            bool isItemSelected = tablesList.SelectedItems.Count > 0;

            editTabButton.Enabled = isItemSelected;
            delTabButton.Enabled = isItemSelected; 
        }

        private string GenerateCreateTableQuery(string TableName, List<TableField> fields) //������� ��� ��������� ������� �� �������� �������
        {
            string query = $"CREATE TABLE {TableName} (";

            foreach (TableField field in fields)
            {
                query += $"{field.Name} {field.Type}";

                if (field.IsPrimaryKey)
                    query += " PRIMARY KEY";

                query += ", ";
            }

            query = query.TrimEnd(',', ' ') + ")";
            return query;
        }

        private void createTabButton_Click(object sender, EventArgs e) //��������� ������ "������� �������"
        {
            List<string> tableNames = new List<string>();

            foreach (ListViewItem item in tablesList.Items)
                tableNames.Add(item.Text);

            using (TableEditor tableEditor = new TableEditor(null, null,tableNames))
            {
                if (tableEditor.ShowDialog() == DialogResult.OK)
                {
                    // �������� ������ ����� �������
                    string TableName = tableEditor.TableName;
                    List<TableField> Fields = tableEditor.Fields;

                    string sqlQuery = GenerateCreateTableQuery(TableName, Fields);

                    try
                    {
                        // ��������� ������ �� �������� ������� ����� ExecuteNonQuery
                        dataBase.ExecuteNonQuery(sqlQuery);

                        MessageBox.Show("������� ������� �������!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"������ �������� �������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                GetTableNames();
            }
        }

        public List<TableField> GetTableFields(string tableName) //������� ��� ��������� ������ � �������� �������
        {
            List<TableField> fields = new List<TableField>();

            string query = $"""
                SELECT 
                    c.COLUMN_NAME,
                    c.DATA_TYPE,
                    CASE 
                        WHEN k.COLUMN_NAME IS NOT NULL THEN 1
                        ELSE 0
                    END AS IsPrimaryKey
                FROM 
                    INFORMATION_SCHEMA.COLUMNS c
                LEFT JOIN 
                    INFORMATION_SCHEMA.KEY_COLUMN_USAGE k
                    ON c.COLUMN_NAME = k.COLUMN_NAME
                    AND c.TABLE_NAME = k.TABLE_NAME
                LEFT JOIN 
                    INFORMATION_SCHEMA.TABLE_CONSTRAINTS t
                    ON k.CONSTRAINT_NAME = t.CONSTRAINT_NAME
                    AND t.CONSTRAINT_TYPE = 'PRIMARY KEY'
                WHERE 
                    c.TABLE_NAME = '{tableName}';
                """;

            try
            {
                using (SqlDataReader reader = dataBase.ExecuteQuery(query))
                {
                    tablesList.Items.Clear();
                    while (reader.Read())
                    {
                        string columnName = reader["COLUMN_NAME"].ToString();
                        string dataType = reader["DATA_TYPE"].ToString();
                        bool isPrimaryKey = Convert.ToBoolean(reader["IsPrimaryKey"]);

                        fields.Add(new TableField { Name = columnName, Type = dataType, IsPrimaryKey = isPrimaryKey });
                    }
                }
                dataBase.CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������: {ex.Message}");
                MessageBox.Show($"������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return fields;
        }

        private string GenerateAlterTableQuery(string oldTableName, string newTableName, List<TableField> fields) //������� ��� ��������� ������� �� ��������� �������
        {
            string query = "";

            if (oldTableName != newTableName)
                query+= $" EXEC sp_rename '{oldTableName}', '{newTableName}';";

            // �������� ������� ������� � �������
            List<TableField> existingColumns = GetTableFields(oldTableName);

            // ���������� ��� ���������� ������� ��� ��������� � �������������
            foreach (TableField field in fields)
            {
                // ���������, ���������� �� ������� � ����� �� ������ � �������
                TableField existingField = existingColumns.FirstOrDefault(x => x.Name == field.Name);

                if (existingField != null)
                {
                    // ���������, ���� ��� ������������� ������� ���������� �� �����������
                    if (existingField.Type != field.Type)
                        query += $" ALTER TABLE {newTableName} ALTER COLUMN {field.Name} {field.Type};";
                }
                else
                {
                    // �������� ����� ������� � ����� �� ����� ������, �� � ������ ������
                    TableField renamedField = existingColumns.FirstOrDefault(x => x.Type == field.Type && !fields.Any(f => f.Name == x.Name));
                    if (renamedField != null)
                    {
                        // ���� ������ ������� � ��� �� ����� ������, �������, ��� ��� ��� ��������������� �������
                        query += $" EXEC sp_rename '{newTableName}.{renamedField.Name}', '{field.Name}', 'COLUMN';";
                    }
                    else
                    {
                        // ���� ������� ������������� �����, ��������� ���
                        query += $" ALTER TABLE {newTableName} ADD {field.Name} {field.Type};";
                    }
                }
            }

            // ������� �������, ������� �� ������������ � ����� ������ �����, � �� ���� �������������
            foreach (TableField existingField in existingColumns)
            {
                // ���� ������� ����������� � ����� ������ � �� ��� ������������
                bool isRenamed = fields.Any(f => f.Name != existingField.Name && f.Type == existingField.Type && !existingColumns.Any(ec => ec.Name == f.Name));
                if (!fields.Any(x => x.Name == existingField.Name) && !isRenamed)
                {
                    // ������� �������, ���� �� �� ��� ������������
                    query += $" ALTER TABLE {newTableName} DROP COLUMN {existingField.Name};";
                }
            }

            return query;
        }
        
        private void editTabButton_Click(object sender, EventArgs e) //��������� ������ "������������� �������"
        {
            editTabButton.Enabled = false;
            delTabButton.Enabled = false;

            string selectedTableName = tablesList.SelectedItems[0].Text;
            List<TableField> fields = GetTableFields(selectedTableName);

            List<string> tableNames = new List<string>();
            foreach (ListViewItem item in tablesList.Items)
                if (item.Text!=selectedTableName)
                    tableNames.Add(item.Text);


            using (TableEditor tableEditor = new TableEditor(selectedTableName, fields, tableNames))
            {
                if (tableEditor.ShowDialog() == DialogResult.OK)
                {
                    // �������� ���������� ������ �������
                    string tableName = tableEditor.TableName;
                    List<TableField> Fields = tableEditor.Fields;

                    // ���������� SQL-������ ��� �������������� �������
                    string sqlQuery = GenerateAlterTableQuery(selectedTableName, tableName, Fields);

                    
                    try
                    {
                        // ��������� ������ ��� ��������� �������
                        if (!string.IsNullOrEmpty(sqlQuery))
                        {
                            dataBase.ExecuteNonQuery(sqlQuery);
                            MessageBox.Show($"������� {selectedTableName} ������� ���������������.", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"������ ��� �������������� �������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                GetTableNames();
            }

        }

        private void delTabButton_Click(object sender, EventArgs e) //��������� ������ "������� �������"
        {

            editTabButton.Enabled = false;
            delTabButton.Enabled = false;

            string selectedTableName = tablesList.SelectedItems[0].Text;

            // ������������ ��������
            DialogResult result = MessageBox.Show($"�� �������, ��� ������ ������� ������� {selectedTableName}?", "������������� ��������",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // ������� ������� �� ���� ������
                    string query = $"DROP TABLE {selectedTableName}";
                    dataBase.ExecuteNonQuery(query);

                    GetTableNames();

                    MessageBox.Show($"������� {selectedTableName} ������� �������.", "�����", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"������ ��� �������� �������: {ex.Message}", "������", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
