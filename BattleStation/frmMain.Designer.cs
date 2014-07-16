namespace BattleStation
{
    partial class frmMain
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
            this.btnStart = new System.Windows.Forms.Button();
            this.pTable = new System.Windows.Forms.Panel();
            this.dgvSQLResults = new System.Windows.Forms.DataGridView();
            this.btnTestParse = new System.Windows.Forms.Button();
            this.pTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLResults)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(124, 78);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Run";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // pTable
            // 
            this.pTable.Controls.Add(this.dgvSQLResults);
            this.pTable.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pTable.Location = new System.Drawing.Point(0, 142);
            this.pTable.Name = "pTable";
            this.pTable.Size = new System.Drawing.Size(606, 267);
            this.pTable.TabIndex = 1;
            // 
            // dgvSQLResults
            // 
            this.dgvSQLResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSQLResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSQLResults.Location = new System.Drawing.Point(0, 0);
            this.dgvSQLResults.Name = "dgvSQLResults";
            this.dgvSQLResults.Size = new System.Drawing.Size(606, 267);
            this.dgvSQLResults.TabIndex = 0;
            // 
            // btnTestParse
            // 
            this.btnTestParse.Location = new System.Drawing.Point(152, 12);
            this.btnTestParse.Name = "btnTestParse";
            this.btnTestParse.Size = new System.Drawing.Size(75, 23);
            this.btnTestParse.TabIndex = 2;
            this.btnTestParse.Text = "Parse";
            this.btnTestParse.UseVisualStyleBackColor = true;
            this.btnTestParse.Click += new System.EventHandler(this.btnTestParse_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 409);
            this.Controls.Add(this.btnTestParse);
            this.Controls.Add(this.pTable);
            this.Controls.Add(this.btnStart);
            this.Name = "frmMain";
            this.Text = "Battle Station";
            this.pTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLResults)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Panel pTable;
        private System.Windows.Forms.DataGridView dgvSQLResults;
        private System.Windows.Forms.Button btnTestParse;
    }
}

