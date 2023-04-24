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

        //    List<int> Nlist = new List<int> { 3,4,5,6,7,8,9,10,11,15};
        //    List<int> Mlist = new List<int> { 49, 249, 649, 1049 };
        //    int a = 10;
        //    int b = 30;
        //    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
        //    row.Cells[0].Value = $"{a} - {b}";
        //    row.Cells[1].Value = "Двухшаговый Крон";
        //    row.Cells[2].Value = "Двухшаговый Крон с КП убыв";
        //    row.Cells[3].Value = "Трёхшаговый Крон";
        //    row.Cells[4].Value = "Трёхшаговый Крон с КП по убыв";
        //    row.Cells[5].Value = "Трёхшаговый Крон с Пашкеевым";
        //    row.Cells[6].Value = "Трёхшаговый Крон с Коробком";
        //    dataGridView1.Rows.Add(row);
        //    foreach (var N in Nlist)
        //    {
        //        foreach (var M in Mlist)
        //        {

        //            dataGridView1.Rows.Add(getRow(N, M, a, b)); 
        //        }
        //    }


        //    copyAlltoClipboard();
        //    Microsoft.Office.Interop.Excel.Application xlexcel;
        //    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        //    object misValue = System.Reflection.Missing.Value;
        //    xlexcel = new Microsoft.Office.Interop.Excel.Application();
        //    xlexcel.Visible = true;
        //    xlWorkBook = xlexcel.Workbooks.Add(misValue);
        //    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
        //    Microsoft.Office.Interop.Excel.Range CR = (Microsoft.Office.Interop.Excel.Range)xlWorkSheet.Cells[1, 1];
        //    CR.Select();
        //    xlWorkSheet.PasteSpecial(CR, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, true);
        //}
        //private void copyAlltoClipboard()
        //{
        //    dataGridView1.SelectAll();
        //    DataObject dataObj = dataGridView1.GetClipboardContent();
        //    if (dataObj != null)
        //        Clipboard.SetDataObject(dataObj);
        //}
        //DataGridViewRow getRow(int N, int M, int a, int b, int Niter = 500)
        //{
        //    double result0 = 0;
        //    double result1 = 0;
        //    double result2 = 0;
        //    double result3 = 0;
        //    double result4 = 0;
        //    double result5 = 0;
        //    int counter1 = 0;
        //    int counter2 = 0;
        //    int counter3 = 0;
        //    int counter4 = 0;
        //    //var par_res = Parallel.For(
        //    //    0, Niter, (int x) =>
        //    //{
        //    //    Method meth = new Method(N, M, a, b);

        //    //    result1 += meth.SolveDoubleKrone();
        //    //    result2 += meth.SolveCM2();
        //    //    result3 += meth.SolveCM3();
        //    //    result4 += meth.SolvePashkeev();
        //    //    //result5 += meth.SolvePashkeev();

        //    //});

        //    for (int i = 0; i < Niter; i++)
        //    {
        //        Method meth = new Method(N, M, a, b);
        //        result0 += meth.SolveDoubleKrone();
        //        result1 += meth.SolveDoubleKroneWithCM2();
        //        var tmp = meth.SolveTripleKrone();
        //        result2 += tmp.Item1;
        //        counter1 += tmp.Item2;
                
        //        //result3 += meth.SolveDoubleKroneWithCM3();
        //        //result4 += meth.SolveDoubleKroneWithCM2();
        //        //result5 += meth.SolveDoubleKroneWithPashkeev();

        //        tmp = meth.SolveTripleKroneWithCM2();
        //        result3 += tmp.Item1;
        //        counter2 += tmp.Item2;
        //        tmp = meth.SolveTripleKroneWithPashkeev();
        //        result4 += tmp.Item1;
        //        counter3 += tmp.Item2;
        //        tmp = meth.SolveTripleKroneWithKobak();
        //        result5 += tmp.Item1;
        //        counter4 += tmp.Item2;
        //    }
        //    DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
        //    row.Cells[0].Value = $"{N}x{M}";
        //    row.Cells[1].Value = result0 / Niter;
        //    row.Cells[2].Value = result1 / Niter;
        //    row.Cells[3].Value =  $"{result2 / Niter}|{counter1}";
        //    row.Cells[4].Value = $"{result3 / Niter}|{counter2}";
        //    row.Cells[5].Value = $"{result4 / Niter}|{counter3}";
        //    row.Cells[6].Value = $"{result5 / Niter}|{counter4}";
        //    //MessageBox.Show($"{result1 / Niter}, {result2 / Niter}, {result3 / Niter}, {result4 / Niter}, {result5 / Niter}");
        //    //Console.ReadLine();
        //    return row;

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            GoldbergAlg goldbergAlg = new GoldbergAlg(3,40,10,30,500,100,50,5);
            goldbergAlg.SolveRandom();
        }
    }
}
