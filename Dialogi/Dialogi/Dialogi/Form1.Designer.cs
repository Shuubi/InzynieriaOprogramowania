namespace Dialogi
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.textView = new System.Windows.Forms.Panel();
            this.Dialog = new System.Windows.Forms.Label();
            this.textView.SuspendLayout();
            this.SuspendLayout();
            // 
            // textView
            // 
            this.textView.Controls.Add(this.Dialog);
            this.textView.Location = new System.Drawing.Point(71, 244);
            this.textView.Name = "textView";
            this.textView.Size = new System.Drawing.Size(670, 174);
            this.textView.TabIndex = 0;
            // 
            // Dialog
            // 
            this.Dialog.AutoSize = true;
            this.Dialog.Location = new System.Drawing.Point(40, 34);
            this.Dialog.Name = "Dialog";
            this.Dialog.Size = new System.Drawing.Size(0, 13);
            this.Dialog.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.textView);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Click += new System.EventHandler(this.clickAction);
            this.textView.ResumeLayout(false);
            this.textView.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel textView;
        private System.Windows.Forms.Label Dialog;
    }
}

