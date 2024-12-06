namespace TestovoeZadaaniye_OrlovArtem
{
    partial class TableEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            dataGridViewFields = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewComboBoxColumn();
            Column3 = new DataGridViewCheckBoxColumn();
            SaveButton = new Button();
            CnlButton = new Button();
            tabNameTxtBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            ((System.ComponentModel.ISupportInitialize)dataGridViewFields).BeginInit();
            SuspendLayout();
            // 
            // dataGridViewFields
            // 
            dataGridViewFields.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewFields.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3 });
            dataGridViewFields.Location = new Point(12, 97);
            dataGridViewFields.Name = "dataGridViewFields";
            dataGridViewFields.Size = new Size(347, 229);
            dataGridViewFields.TabIndex = 1;
            dataGridViewFields.UserDeletingRow += dataGridViewFields_UserDeletingRow;
            // 
            // Column1
            // 
            Column1.HeaderText = "Название поля";
            Column1.Name = "Column1";
            // 
            // Column2
            // 
            Column2.HeaderText = "Тип поля";
            Column2.Items.AddRange(new object[] { "int", "float", "text", "datetime" });
            Column2.Name = "Column2";
            // 
            // Column3
            // 
            Column3.HeaderText = "Первичный ключ";
            Column3.Name = "Column3";
            // 
            // SaveButton
            // 
            SaveButton.Location = new Point(12, 367);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(117, 41);
            SaveButton.TabIndex = 4;
            SaveButton.Text = "Сохранить";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // CnlButton
            // 
            CnlButton.Location = new Point(242, 367);
            CnlButton.Name = "CnlButton";
            CnlButton.Size = new Size(117, 41);
            CnlButton.TabIndex = 6;
            CnlButton.Text = "Отмена";
            CnlButton.UseVisualStyleBackColor = true;
            CnlButton.Click += CnlButton_Click;
            // 
            // tabNameTxtBox
            // 
            tabNameTxtBox.Location = new Point(12, 47);
            tabNameTxtBox.Name = "tabNameTxtBox";
            tabNameTxtBox.Size = new Size(168, 23);
            tabNameTxtBox.TabIndex = 7;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 79);
            label1.Name = "label1";
            label1.Size = new Size(87, 15);
            label1.TabIndex = 8;
            label1.Text = "Поля таблицы";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 29);
            label2.Name = "label2";
            label2.Size = new Size(110, 15);
            label2.TabIndex = 9;
            label2.Text = "Название таблицы";
            // 
            // TableEditor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(371, 418);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tabNameTxtBox);
            Controls.Add(CnlButton);
            Controls.Add(SaveButton);
            Controls.Add(dataGridViewFields);
            Name = "TableEditor";
            Text = "TableEditor";
            ((System.ComponentModel.ISupportInitialize)dataGridViewFields).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView dataGridViewFields;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewComboBoxColumn Column2;
        private DataGridViewCheckBoxColumn Column3;
        private Button SaveButton;
        private Button CnlButton;
        private TextBox tabNameTxtBox;
        private Label label1;
        private Label label2;
    }
}