using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fBill : Form
    {
        public fBill(int idBill, string ACC,double subTotal,int discount,double total)
        {
            InitializeComponent();
            lbACC.Text = ACC;
            DateTime time = DateTime.Now;
            lbIdBill.Text = "SHD" + idBill.ToString();
            lbSubTotal.Text = subTotal.ToString();
            lbDiscount.Text = discount.ToString();
            lbTotal.Text = total.ToString();          
            lbDate.Text = ("" + time.Day + ":" + time.Month + ":" + time.Year);
            lbTime.Text = ("" + time.Hour + ":" + time.Minute + ":" + time.Second);
            DataTable dt = DAO.BillInfoDAO.Instance.LoadBillLast(idBill);
            grvPrintBill.DataSource = dt;
        }

        private void fBill_Load(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            BaseFont b = BaseFont.CreateFont(BaseFont.TIMES_ROMAN, BaseFont.CP1257, BaseFont.EMBEDDED);
            PdfPTable pdfTable = new PdfPTable(grvPrintBill.ColumnCount);
            pdfTable.DefaultCell.Padding = 3;
            pdfTable.WidthPercentage = 100;
            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;
            pdfTable.DefaultCell.BorderWidth = 1;
            iTextSharp.text.Font text = new iTextSharp.text.Font(b, 10, iTextSharp.text.Font.NORMAL);
            iTextSharp.text.Font text1 = new iTextSharp.text.Font(b, 15, iTextSharp.text.Font.BOLD);

            foreach (DataGridViewColumn column in grvPrintBill.Columns)
            {
                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText))
                {
                    BackgroundColor = new BaseColor(240, 240, 240)
                };
                pdfTable.AddCell(cell);
            }
            foreach (DataGridViewRow row in grvPrintBill.Rows)
            {
                foreach (DataGridViewCell cell in row.Cells)
                {
                    pdfTable.AddCell(cell.Value?.ToString());
                }
            }
            string name = string.Format("                   Cafe 9999");
            string address = string.Format("        Cum 6 – Tan Lap – Dan Phuong – Ha Noi");
            string tel = string.Format("            TEL: 123456789 - 987654321");
            string id = string.Format("So hoa don: " + lbIdBill.Text + "\n");
            string subtotal = string.Format("Tong tam tinh: " + lbSubTotal.Text);
            string discount = string.Format("Giam gia: " + lbDiscount.Text + " %");
            string total = string.Format("Thanh tien: " + lbTotal.Text);
            string date = string.Format("Ngay: " + lbDate.Text + "     Gio: " + lbTime.Text);
            string lbacc = string.Format("                                      Nhan vien");
            string acc = string.Format("                                        " + lbACC.Text);
            string ft = string.Format("     Xin Cam On Quy Khach. Hen Gap Lai!");
            var path = "..\\..\\HoaDon\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            using (FileStream stream = new FileStream($"{path}HoaDon-{DateTime.Now:yyyyMMddHHmmss}.pdf", FileMode.Create))
            {
                Document pdfDoc = new Document(PageSize.A7, 10f, 10f, 10f, 10f);
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                pdfDoc.Add(new Paragraph(name, text1));
                pdfDoc.Add(new Paragraph(address, text));
                pdfDoc.Add(new Paragraph(tel, text));
                pdfDoc.Add(new Paragraph(id, text));
                pdfDoc.Add(new Paragraph(" ", text));
                pdfDoc.Add(pdfTable);
                pdfDoc.Add(new Paragraph(subtotal, text));
                pdfDoc.Add(new Paragraph(discount, text));
                pdfDoc.Add(new Paragraph(total, text));
                pdfDoc.Add(new Paragraph(date, text));
                pdfDoc.Add(new Paragraph(lbacc, text));
                pdfDoc.Add(new Paragraph(acc, text));
                pdfDoc.Add(new Paragraph(ft, text));
                pdfDoc.Close();
            }
            MessageBox.Show("Hóa đơn đã được in");
            System.Diagnostics.Process prc = new System.Diagnostics.Process();
            prc.StartInfo.FileName = "..\\..\\HoaDon\\";
            prc.Start();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
