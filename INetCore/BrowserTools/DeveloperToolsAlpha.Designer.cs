namespace INetCore.BrowserTools
{
    partial class DeveloperToolsAlpha
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
            this.devToolPages = new System.Windows.Forms.TabControl();
            this.elementsPage = new System.Windows.Forms.TabPage();
            this.htmlViewer = new System.Windows.Forms.TreeView();
            this.styleBox = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.devToolPages.SuspendLayout();
            this.elementsPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // devToolPages
            // 
            this.devToolPages.Controls.Add(this.elementsPage);
            this.devToolPages.Location = new System.Drawing.Point(2, 3);
            this.devToolPages.Name = "devToolPages";
            this.devToolPages.SelectedIndex = 0;
            this.devToolPages.Size = new System.Drawing.Size(1260, 542);
            this.devToolPages.TabIndex = 0;
            // 
            // elementsPage
            // 
            this.elementsPage.Controls.Add(this.button1);
            this.elementsPage.Controls.Add(this.styleBox);
            this.elementsPage.Controls.Add(this.htmlViewer);
            this.elementsPage.Location = new System.Drawing.Point(4, 25);
            this.elementsPage.Name = "elementsPage";
            this.elementsPage.Padding = new System.Windows.Forms.Padding(3);
            this.elementsPage.Size = new System.Drawing.Size(1252, 513);
            this.elementsPage.TabIndex = 0;
            this.elementsPage.Text = "Elements";
            this.elementsPage.UseVisualStyleBackColor = true;
            // 
            // htmlViewer
            // 
            this.htmlViewer.Location = new System.Drawing.Point(6, 6);
            this.htmlViewer.Name = "htmlViewer";
            this.htmlViewer.Size = new System.Drawing.Size(851, 501);
            this.htmlViewer.TabIndex = 0;
            // 
            // styleBox
            // 
            this.styleBox.FormattingEnabled = true;
            this.styleBox.ItemHeight = 16;
            this.styleBox.Location = new System.Drawing.Point(863, 6);
            this.styleBox.Name = "styleBox";
            this.styleBox.Size = new System.Drawing.Size(382, 436);
            this.styleBox.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(1170, 448);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // DeveloperToolsAlpha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1263, 548);
            this.Controls.Add(this.devToolPages);
            this.Name = "DeveloperToolsAlpha";
            this.Text = "DeveloperToolsAlpha";
            this.devToolPages.ResumeLayout(false);
            this.elementsPage.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl devToolPages;
        private System.Windows.Forms.TabPage elementsPage;
        private System.Windows.Forms.TreeView htmlViewer;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox styleBox;
    }
}