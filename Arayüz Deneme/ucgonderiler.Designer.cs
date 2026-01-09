namespace Arayüz_Deneme
{
    partial class ucgonderiler
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.button2 = new System.Windows.Forms.Button();
            this.nakliyeDBDataSet = new Arayüz_Deneme.NakliyeDBDataSet();
            this.sevkiyatlarBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.sevkiyatlarTableAdapter = new Arayüz_Deneme.NakliyeDBDataSetTableAdapters.SevkiyatlarTableAdapter();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nakliyeDBDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkiyatlarBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(69, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1603, 657);
            this.dataGridView1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1500, 31);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(172, 35);
            this.button2.TabIndex = 4;
            this.button2.Text = "Gönderi Sil";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // nakliyeDBDataSet
            // 
            this.nakliyeDBDataSet.DataSetName = "NakliyeDBDataSet";
            this.nakliyeDBDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // sevkiyatlarBindingSource
            // 
            this.sevkiyatlarBindingSource.DataMember = "Sevkiyatlar";
            this.sevkiyatlarBindingSource.DataSource = this.nakliyeDBDataSet;
            // 
            // sevkiyatlarTableAdapter
            // 
            this.sevkiyatlarTableAdapter.ClearBeforeFill = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(69, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 35);
            this.button1.TabIndex = 8;
            this.button1.Text = "Yenile";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1322, 31);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(172, 35);
            this.button3.TabIndex = 9;
            this.button3.Text = "Durum Güncelle";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // ucgonderiler
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Name = "ucgonderiler";
            this.Size = new System.Drawing.Size(1755, 843);
            this.Load += new System.EventHandler(this.ucgonderiler_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nakliyeDBDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sevkiyatlarBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.BindingSource sevkiyatlarBindingSource;
        private NakliyeDBDataSet nakliyeDBDataSet;
        private NakliyeDBDataSetTableAdapters.SevkiyatlarTableAdapter sevkiyatlarTableAdapter;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
    }
}
