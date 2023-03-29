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

            List<int> Nlist = new List<int> { 3,4,5,6,7,8,9,10,11,15};
            List<int> Mlist = new List<int> { 49, 249, 649, 1049 };
            int a = 15;
            int b = 25;
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{a} - {b}";
            row.Cells[1].Value = "Двухшаговый Крон";
            row.Cells[2].Value = "Двухшаговый Крон с КП убыв";
            row.Cells[3].Value = "Трёхшаговый Крон";
            row.Cells[4].Value = "Трёхшаговый Крон с КП по убыв";
            row.Cells[5].Value = "Двухшаговый Крон с Пашкеевым";
            dataGridView1.Rows.Add(row);
            foreach (var N in Nlist)
            {
                foreach (var M in Mlist)
                {

                    dataGridView1.Rows.Add(getRow(N, M, a, b));
                }
            }


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
        private void copyAlltoClipboard()
        {
            dataGridView1.SelectAll();
            DataObject dataObj = dataGridView1.GetClipboardContent();
            if (dataObj != null)
                Clipboard.SetDataObject(dataObj);
        }
        DataGridViewRow getRow(int N, int M, int a, int b, int Niter = 100)
        {
            int result1 = 0;
            int result2 = 0;
            int result3 = 0;
            int result4 = 0;
            int result5 = 0;
            int counter1 = 0;
            int counter2 = 0;
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
                Method meth = new Method(N, M, a, b);

                result1 += meth.SolveDoubleKrone();
                result2 += meth.SolveDoubleKroneWithCM1();
                //result3 += meth.SolveDoubleKroneWithCM3();
                //result4 += meth.SolveDoubleKroneWithCM2();
                //result5 += meth.SolveDoubleKroneWithPashkeev();

                var tmp = meth.SolveTripleKrone();
                result3 += tmp.Item1;
                counter1 += tmp.Item2;
                tmp = meth.SolveTripleKroneWithCM2();
                result4 += tmp.Item1;
                counter2 += tmp.Item2;
            }
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{N}x{M}";
            row.Cells[1].Value = result1 / Niter;
            row.Cells[2].Value = result2 / Niter;
            row.Cells[3].Value = $"{result3 / Niter}|{counter1}";
            row.Cells[4].Value = $"{result4 / Niter}|{counter2}";
            //row.Cells[5].Value = result5/ Niter;
            //MessageBox.Show($"{result1 / Niter}, {result2 / Niter}, {result3 / Niter}, {result4 / Niter}, {result5 / Niter}");
            //Console.ReadLine();
            return row;

        }

    }
}
