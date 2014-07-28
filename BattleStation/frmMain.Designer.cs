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
            this.components = new System.ComponentModel.Container();
            this.pTable = new System.Windows.Forms.Panel();
            this.dgvSQLResults = new System.Windows.Forms.DataGridView();
            this.btnBatchParse = new System.Windows.Forms.Button();
            this.txtParseCount = new System.Windows.Forms.TextBox();
            this.btnGenerateHandHistories = new System.Windows.Forms.Button();
            this.txtHandGenerationAmount = new System.Windows.Forms.TextBox();
            this.txtGenerationPace = new System.Windows.Forms.TextBox();
            this.timerGenerateHands = new System.Windows.Forms.Timer(this.components);
            this.pTable.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLResults)).BeginInit();
            this.SuspendLayout();
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
            // btnBatchParse
            // 
            this.btnBatchParse.Location = new System.Drawing.Point(93, 7);
            this.btnBatchParse.Name = "btnBatchParse";
            this.btnBatchParse.Size = new System.Drawing.Size(75, 24);
            this.btnBatchParse.TabIndex = 3;
            this.btnBatchParse.Text = "Batch Parse";
            this.btnBatchParse.UseVisualStyleBackColor = true;
            this.btnBatchParse.Click += new System.EventHandler(this.btnBatchParse_Click);
            // 
            // txtParseCount
            // 
            this.txtParseCount.Location = new System.Drawing.Point(12, 10);
            this.txtParseCount.Name = "txtParseCount";
            this.txtParseCount.Size = new System.Drawing.Size(75, 20);
            this.txtParseCount.TabIndex = 4;
            this.txtParseCount.Text = "1000";
            // 
            // btnGenerateHandHistories
            // 
            this.btnGenerateHandHistories.Location = new System.Drawing.Point(122, 37);
            this.btnGenerateHandHistories.Name = "btnGenerateHandHistories";
            this.btnGenerateHandHistories.Size = new System.Drawing.Size(77, 25);
            this.btnGenerateHandHistories.TabIndex = 5;
            this.btnGenerateHandHistories.Text = "Generate";
            this.btnGenerateHandHistories.UseVisualStyleBackColor = true;
            this.btnGenerateHandHistories.Click += new System.EventHandler(this.btnGenerateHandHistories_Click);
            // 
            // txtHandGenerationAmount
            // 
            this.txtHandGenerationAmount.Location = new System.Drawing.Point(12, 40);
            this.txtHandGenerationAmount.Name = "txtHandGenerationAmount";
            this.txtHandGenerationAmount.Size = new System.Drawing.Size(75, 20);
            this.txtHandGenerationAmount.TabIndex = 6;
            this.txtHandGenerationAmount.Text = "100";
            // 
            // txtGenerationPace
            // 
            this.txtGenerationPace.Location = new System.Drawing.Point(93, 40);
            this.txtGenerationPace.Name = "txtGenerationPace";
            this.txtGenerationPace.Size = new System.Drawing.Size(23, 20);
            this.txtGenerationPace.TabIndex = 7;
            this.txtGenerationPace.Text = "10";
            // 
            // timerGenerateHands
            // 
            this.timerGenerateHands.Tick += new System.EventHandler(this.timerGenerateHands_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 409);
            this.Controls.Add(this.txtGenerationPace);
            this.Controls.Add(this.txtHandGenerationAmount);
            this.Controls.Add(this.btnGenerateHandHistories);
            this.Controls.Add(this.txtParseCount);
            this.Controls.Add(this.btnBatchParse);
            this.Controls.Add(this.pTable);
            this.Name = "frmMain";
            this.Text = "Battle Station";
            this.pTable.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSQLResults)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pTable;
        private System.Windows.Forms.DataGridView dgvSQLResults;
        private System.Windows.Forms.Button btnBatchParse;
        private System.Windows.Forms.TextBox txtParseCount;
        private System.Windows.Forms.Button btnGenerateHandHistories;
        private System.Windows.Forms.TextBox txtHandGenerationAmount;
        private System.Windows.Forms.TextBox txtGenerationPace;
        private System.Windows.Forms.Timer timerGenerateHands;
    }
}

