using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestovoeZadaaniye_OrlovArtem
{
    public partial class TableEditor : Form
    {

        public string TableName;
        public List<TableField> Fields;
        public bool IsEditMode;

        public List<string> TableNames; //список существующих таблиц

        private void UpdateFieldsGrid() //Функция для обновления DataGridView
        {
            dataGridViewFields.Rows.Clear();

            DataGridViewComboBoxColumn comboBoxColumn = (DataGridViewComboBoxColumn)dataGridViewFields.Columns["Column2"];

            // Получаем все возможные значения комбобокса
            List<String> comboBoxValues = comboBoxColumn.Items.Cast<string>().ToList();

            foreach (TableField field in Fields)
            {
                int rowIndex = dataGridViewFields.Rows.Add();
                if (!comboBoxValues.Contains(field.Type))
                {
                    if (field.IsPrimaryKey)
                    {
                        MessageBox.Show($"Значение '{field.Type}' не найдено в комбобоксе. Оно будет заменено на 'int'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewFields.Rows[rowIndex].Cells["Column2"].Value = "int";
                    }
                    else
                    {
                        MessageBox.Show($"Значение '{field.Type}' не найдено в комбобоксе. Оно будет заменено на 'text'", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        dataGridViewFields.Rows[rowIndex].Cells["Column2"].Value = "text";
                    }
                }
                else
                    dataGridViewFields.Rows[rowIndex].Cells["Column2"].Value = field.Type;

                dataGridViewFields.Rows[rowIndex].Cells["Column1"].Value = field.Name;
                dataGridViewFields.Rows[rowIndex].Cells["Column3"].Value = field.IsPrimaryKey;
            }
        }

        public TableEditor(string tableName = null, List<TableField> fields = null, List<string> tableNames = null) //Конструктор класса
        {
            InitializeComponent();

            // Если переданы данные, это режим редактирования
            IsEditMode = false;
            if (tableName != null)
                IsEditMode = true;

            TableName = "";
            if (tableName != null)
                TableName = tableName;

            Fields = new List<TableField>();
            if (fields != null)
                Fields = fields;

            tabNameTxtBox.Text = TableName;
            UpdateFieldsGrid();

            TableNames = tableNames;

            //Убираем возможность редактирования первичного ключа при редактировании таблицы
            if (IsEditMode)
                dataGridViewFields.Columns["Column3"].ReadOnly = true;

        }

        private bool CheckTableData() //Проверка данных создаваемой или редактируемой таблицы
        {
            if (string.IsNullOrWhiteSpace(tabNameTxtBox.Text))
            {
                MessageBox.Show("Имя таблицы не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (!Regex.IsMatch(tabNameTxtBox.Text, @"^[a-zA-Z_][a-zA-Z0-9_]*$"))
            {
                MessageBox.Show("Имя таблицы содержит недопустимые символы. Допустимы только буквы, цифры и символ подчёркивания.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (TableNames.Contains(tabNameTxtBox.Text))
            {
                MessageBox.Show($"Таблица с именем '{tabNameTxtBox.Text}' уже существует.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }


            if (dataGridViewFields.Rows.Count == 0)
            {
                MessageBox.Show("Таблица должна содержать хотя бы одно поле.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            //Это условие добавлено потому что AllowUserToAddRows включено свойство и в таблице может быть строка с пустыми полями
            bool hasData = false;
            foreach (DataGridViewRow row in dataGridViewFields.Rows)
            {
                if (!row.IsNewRow)
                {
                    hasData = true;
                    break;
                }
            }

            if (!hasData)
            {
                MessageBox.Show("Таблица должна содержать хотя бы одно поле.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            List<string> fieldNames = new List<string>();
            bool hasPrimaryKey = false;
            foreach (DataGridViewRow row in dataGridViewFields.Rows)
            {
                if (!row.IsNewRow)
                {
                    string fieldName = "";
                    string fieldType = "";

                    if (row.Cells["Column1"].Value != null)
                        fieldName = row.Cells["Column1"].Value.ToString();

                    if (row.Cells["Column2"].Value != null)
                        fieldType = row.Cells["Column2"].Value.ToString();

                    if (!string.IsNullOrEmpty(fieldName) && !string.IsNullOrEmpty(fieldType))
                        fieldNames.Add(fieldName);
                    else
                    {
                        MessageBox.Show("Каждое поле должно иметь имя и тип данных.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }

                    if (row.Cells["Column3"].Value != null && (bool)row.Cells["Column3"].Value)
                    {
                        hasPrimaryKey = true;

                        if (fieldType.ToLower() != "int")
                        {
                            MessageBox.Show("Первичный ключ должен быть типа 'int'.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return false;
                        }
                    }
                }
            }

            if (!hasPrimaryKey)
            {
                MessageBox.Show("Таблица должна содержать хотя бы одно поле, помеченное как первичный ключ.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (fieldNames.Distinct().Count() != fieldNames.Count)
            {
                MessageBox.Show("Имена полей не должны повторяться.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void SaveButton_Click(object sender, EventArgs e) //Обработка кнопки "Cохранить"
        {
            if (CheckTableData())
            {
                TableName = tabNameTxtBox.Text;
                Fields = new List<TableField>();

                foreach (DataGridViewRow row in dataGridViewFields.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        string fieldName = "";
                        if (row.Cells["Column1"].Value != null)
                            fieldName = row.Cells["Column1"].Value.ToString();

                        string fieldType = "";
                        if (row.Cells["Column2"].Value != null)
                            fieldType = row.Cells["Column2"].Value.ToString();

                        var isPrimaryKey = row.Cells["Column3"].Value != null && (bool)row.Cells["Column3"].Value;

                        Fields.Add(new TableField
                        {
                            Name = fieldName,
                            Type = fieldType,
                            IsPrimaryKey = isPrimaryKey
                        });

                    }
                }

                DialogResult = DialogResult.OK;
                Close();
            }
        }

        private void CnlButton_Click(object sender, EventArgs e) //Обработка кнопки "Отмена"
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dataGridViewFields_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e) //Обработка события удаления строки, чтобы избежать удаления первичного ключа и запросить подтверждение от пользователя
        {
            var isPrimaryKeyCell = e.Row.Cells["Column3"];
            if (isPrimaryKeyCell != null && Convert.ToBoolean(isPrimaryKeyCell.Value))
            {
                MessageBox.Show("Удаление столбца, являющегося первичным ключом, невозможно. Это нарушит структуру таблицы", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Отменяем удаление строки
                e.Cancel = true;
            }
            else
            {

                // Подтверждаем удаление        
                DialogResult result = MessageBox.Show("Вы уверены, что хотите удалить выбранный столбец?", "Подтверждение удаления", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

    }
}
