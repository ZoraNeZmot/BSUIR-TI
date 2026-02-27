namespace Cipher1
{
    partial class CipherFont
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
            this.label2 = new System.Windows.Forms.Label();
            this.textKey = new System.Windows.Forms.TextBox();
            this.textFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnFile = new System.Windows.Forms.Button();
            this.textIn = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textOut = new System.Windows.Forms.TextBox();
            this.btnCipher = new System.Windows.Forms.Button();
            this.btnDecipher = new System.Windows.Forms.Button();
            this.comboAlgorithm = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbSave = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(21, 96);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Ключ";
            // 
            // textKey
            // 
            this.textKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textKey.Location = new System.Drawing.Point(187, 92);
            this.textKey.Name = "textKey";
            this.textKey.Size = new System.Drawing.Size(360, 34);
            this.textKey.TabIndex = 2;
            // 
            // textFile
            // 
            this.textFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textFile.Location = new System.Drawing.Point(182, 151);
            this.textFile.Name = "textFile";
            this.textFile.Size = new System.Drawing.Size(489, 34);
            this.textFile.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(21, 155);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(116, 30);
            this.label3.TabIndex = 3;
            this.label3.Text = "Файл";
            // 
            // btnFile
            // 
            this.btnFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnFile.Location = new System.Drawing.Point(701, 151);
            this.btnFile.Name = "btnFile";
            this.btnFile.Size = new System.Drawing.Size(116, 34);
            this.btnFile.TabIndex = 5;
            this.btnFile.Text = "Выбрать";
            this.btnFile.UseVisualStyleBackColor = true;
            this.btnFile.Click += new System.EventHandler(this.btnFile_Click);
            // 
            // textIn
            // 
            this.textIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textIn.Location = new System.Drawing.Point(26, 231);
            this.textIn.Multiline = true;
            this.textIn.Name = "textIn";
            this.textIn.Size = new System.Drawing.Size(1286, 200);
            this.textIn.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(21, 198);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(116, 30);
            this.label4.TabIndex = 10;
            this.label4.Text = "Ввод:";
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(21, 434);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(116, 30);
            this.label7.TabIndex = 11;
            this.label7.Text = "Вывод:";
            // 
            // textOut
            // 
            this.textOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textOut.Location = new System.Drawing.Point(26, 467);
            this.textOut.Multiline = true;
            this.textOut.Name = "textOut";
            this.textOut.Size = new System.Drawing.Size(1286, 200);
            this.textOut.TabIndex = 12;
            // 
            // btnCipher
            // 
            this.btnCipher.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnCipher.Location = new System.Drawing.Point(848, 151);
            this.btnCipher.Name = "btnCipher";
            this.btnCipher.Size = new System.Drawing.Size(195, 39);
            this.btnCipher.TabIndex = 14;
            this.btnCipher.Text = "Зашифровать";
            this.btnCipher.UseVisualStyleBackColor = true;
            this.btnCipher.Click += new System.EventHandler(this.btnCipher_Click);
            // 
            // btnDecipher
            // 
            this.btnDecipher.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btnDecipher.Location = new System.Drawing.Point(1077, 151);
            this.btnDecipher.Name = "btnDecipher";
            this.btnDecipher.Size = new System.Drawing.Size(205, 39);
            this.btnDecipher.TabIndex = 15;
            this.btnDecipher.Text = "Расшифровать";
            this.btnDecipher.UseVisualStyleBackColor = true;
            this.btnDecipher.Click += new System.EventHandler(this.btnDecipher_Click);
            // 
            // comboAlgorithm
            // 
            this.comboAlgorithm.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboAlgorithm.FormattingEnabled = true;
            this.comboAlgorithm.Items.AddRange(new object[] {
            "«столбцовый метод» с одним ключевым словом",
            "алгоритм Виженера, самогенерирующийся ключ"});
            this.comboAlgorithm.Location = new System.Drawing.Point(187, 33);
            this.comboAlgorithm.Name = "comboAlgorithm";
            this.comboAlgorithm.Size = new System.Drawing.Size(589, 28);
            this.comboAlgorithm.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(21, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(135, 30);
            this.label1.TabIndex = 18;
            this.label1.Text = "Алгоритм";
            // 
            // cbSave
            // 
            this.cbSave.AutoSize = true;
            this.cbSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cbSave.Location = new System.Drawing.Point(173, 440);
            this.cbSave.Name = "cbSave";
            this.cbSave.Size = new System.Drawing.Size(213, 24);
            this.cbSave.TabIndex = 20;
            this.cbSave.Text = "Сохранить в файл?";
            this.cbSave.UseVisualStyleBackColor = true;
            // 
            // CipherFont
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1316, 679);
            this.Controls.Add(this.cbSave);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboAlgorithm);
            this.Controls.Add(this.btnDecipher);
            this.Controls.Add(this.btnCipher);
            this.Controls.Add(this.textOut);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textIn);
            this.Controls.Add(this.btnFile);
            this.Controls.Add(this.textFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textKey);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CipherFont";
            this.Text = "Cipher";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textKey;
        private System.Windows.Forms.TextBox textFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnFile;
        private System.Windows.Forms.TextBox textIn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textOut;
        private System.Windows.Forms.Button btnCipher;
        private System.Windows.Forms.Button btnDecipher;
        private System.Windows.Forms.ComboBox comboAlgorithm;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbSave;
    }
}

