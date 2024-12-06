namespace TestovoeZadaaniye_OrlovArtem
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            createTabButton = new Button();
            tablesList = new ListView();
            tableName = new ColumnHeader();
            editTabButton = new Button();
            delTabButton = new Button();
            SuspendLayout();
            // 
            // createTabButton
            // 
            createTabButton.Location = new Point(222, 51);
            createTabButton.Name = "createTabButton";
            createTabButton.Size = new Size(117, 41);
            createTabButton.TabIndex = 3;
            createTabButton.Text = "Создать таблицу";
            createTabButton.UseVisualStyleBackColor = true;
            createTabButton.Click += createTabButton_Click;
            // 
            // tablesList
            // 
            tablesList.Columns.AddRange(new ColumnHeader[] { tableName });
            tablesList.FullRowSelect = true;
            tablesList.GridLines = true;
            tablesList.Location = new Point(21, 51);
            tablesList.MultiSelect = false;
            tablesList.Name = "tablesList";
            tablesList.Size = new Size(174, 293);
            tablesList.TabIndex = 4;
            tablesList.UseCompatibleStateImageBehavior = false;
            tablesList.View = View.Details;
            tablesList.SelectedIndexChanged += tablesList_SelectedIndexChanged;
            // 
            // tableName
            // 
            tableName.Text = "Пользовательские таблицы";
            tableName.Width = 200;
            // 
            // editTabButton
            // 
            editTabButton.Location = new Point(222, 118);
            editTabButton.Name = "editTabButton";
            editTabButton.Size = new Size(117, 41);
            editTabButton.TabIndex = 5;
            editTabButton.Text = "Редактировать таблицу";
            editTabButton.UseVisualStyleBackColor = true;
            editTabButton.Click += editTabButton_Click;
            // 
            // delTabButton
            // 
            delTabButton.Location = new Point(222, 188);
            delTabButton.Name = "delTabButton";
            delTabButton.Size = new Size(117, 41);
            delTabButton.TabIndex = 6;
            delTabButton.Text = "Удалить таблицу";
            delTabButton.UseVisualStyleBackColor = true;
            delTabButton.Click += delTabButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(355, 360);
            Controls.Add(delTabButton);
            Controls.Add(editTabButton);
            Controls.Add(tablesList);
            Controls.Add(createTabButton);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button createTabButton;
        private ListView tablesList;
        private ColumnHeader tableName;
        private Button editTabButton;
        private Button delTabButton;
    }
}
