using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Project_for_confirence
{
    public partial class Form1 : Form
    {


        public Form1()
        {
            InitializeComponent();



        }
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        DataGridViewRow getRow(int N, int M, int a, int b, int Niter = 2)
        {
            double result0 = 0;
            double result1 = 0;
            double result2 = 0;
            double result3 = 0;
            double result4 = 0;
            double result5 = 0;
            int counter1 = 0;
            int counter2 = 0;
            int counter3 = 0;
            int counter4 = 0;
            int counter5 = 0;
            //var par_res = Parallel.For(
            //    0, Niter, (int x) =>
            //{
            //    Method meth = new Method(N, M, a, b);

            //    result1 += meth.SolveDoubleKrone();
            //    result2 += meth.SolveCM2();
            //    result3 += meth.SolveCM3();
            //    result4 += meth.SolvePashkeev();
            //    //result5 += meth.SolvePashkeev();

            //});

            for (int i = 0; i < Niter; i++)
            {
                GoldbergAlg alg = new GoldbergAlg(N, M, a, b, 500, 100, 100, 100);
                Method meth = new Method(N, M, a, b);

                result0 += alg.SolveByT(meth);

                result1 += alg.SolveWithCM(meth, 250, 83, 83, 84);

                result2 += alg.SolveWithCM(meth, 0, 166, 166, 168);
                //result3 += meth.SolveCM2();
                //result4 += meth.SolveDoubleKrone();

                //var tmp = meth.SolveTripleKroneWithCM2();
                //result0 += tmp.Item1;
                //counter1 += tmp.Item2;

                //result3 += meth.SolveDoubleKroneWithCM3();
                //result4 += meth.SolveDoubleKroneWithCM2();
                //result5 += meth.SolveDoubleKroneWithPashkeev();

                //tmp = meth.SolveTripleKroneWithCM3();
                //result1 += tmp.Item1;
                //counter2 += tmp.Item2;
                //tmp = meth.SolveTripleKroneWithPashkeev();
                //result2 += tmp.Item1;
                //counter3 += tmp.Item2;
                //tmp = meth.SolveTripleKroneWithKobak();
                //result3 += tmp.Item1;
                //counter4 += tmp.Item2;

                //tmp = meth.SolveTripleKrone();
                //result4 += tmp.Item1;
                //counter5 += tmp.Item2;
            }
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{N}x{M}";
            row.Cells[1].Value = result0 / Niter;
            row.Cells[2].Value = result1 / Niter;
            row.Cells[3].Value = result2 / Niter;
            row.Cells[4].Value = result3 / Niter;
            row.Cells[5].Value = result4 / Niter;
            //row.Cells[0].Value = $"{N}x{M}";
            //row.Cells[1].Value = $"{result0 / Niter}|{counter1}";
            //row.Cells[2].Value = $"{result1 / Niter}|{counter2}";
            //row.Cells[3].Value = $"{result2 / Niter}|{counter3}";
            //row.Cells[4].Value = $"{result3 / Niter}|{counter4}";
            //row.Cells[5].Value = $"{result4 / Niter}|{counter5}";
            //row.Cells[6].Value = $"{result5 / Niter}|{counter4}";
            //MessageBox.Show($"{result1 / Niter}, {result2 / Niter}, {result3 / Niter}, {result4 / Niter}, {result5 / Niter}");
            //Console.ReadLine();
            return row;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //GoldbergAlg goldbergAlg = new GoldbergAlg(3, 49, 10, 30, 3, 30, 30);
            //goldbergAlg.SolveRandom();
            //MessageBox.Show("4");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            List<int> Nlist = new List<int> { 3,4,5 };
            List<int> Mlist = new List<int> { 53, 353 };
            int a = 10;
            int b = 30;
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{a} - {b}";
            row.Cells[1].Value = "Голдберг случайный";
            row.Cells[2].Value = "Голдберг пол случ, пол КП";
            row.Cells[3].Value = "Голдберг все КП";
            //row.Cells[4].Value = "Голдберг все КП";
            //row.Cells[5].Value = "Двухфазный Крон";
            //row.Cells[6].Value = "Трёхфазный Крон с Коробком";
            dataGridView1.Rows.Add(row);
            foreach (var N in Nlist)
            {
                foreach (var M in Mlist)
                {
                    dataGridView1.Rows.Add(getRow(N, M, a, b, 3));
                }
            }
            try
            {
                copyAlltoClipboard();
                Microsoft.Office.Interop.Excel.Application xlexcel;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;
                xlexcel = new Microsoft.Office.Interop.Excel.Application();
                xlexcel.Visible = true;
                xlWorkBook = xlexcel.Workbooks.Add(misValue);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
                CR.Select();
                xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }
    }
}
