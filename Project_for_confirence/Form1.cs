using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Linq;

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
        DataGridViewRow getParrRow(int N, int M, int a, int b, int k = 500, int pc = 100, int pm = 100, int kpovt = 5, int Niter = 2)
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

            // Создаем список задач
            List<Task<double[]>> tasks = new List<Task<double[]>>();

            for (int i = 0; i < Niter; i++)
            {
                // Создаем экземпляр класса GoldbergAlg и Method
                GoldbergAlg alg = new GoldbergAlg(N, M, a, b, k, pc, pm, kpovt);
                Method meth = new Method(N, M, a, b);

                // Создаем новую задачу и запускаем ее на выполнение
                Task<double[]> task = Task.Factory.StartNew(() =>
                {
                    double[] results = new double[5];
                    results[0] = alg.SolveByT(meth);
                    results[1] = alg.SolveWithKrone(meth, 499, 1, 0);
                    results[2] = alg.SolveWithKrone(meth, 499, 0, 1);
                    results[3] = alg.SolveWithKrone(meth, 498, 2, 0);
                    results[4] = alg.SolveWithKrone(meth, 498, 0, 2);
                    return results;
                }, TaskCreationOptions.LongRunning);

                // Добавляем задачу в список задач
                tasks.Add(task);
            }

            // Ожидаем завершения всех задач и суммируем результаты
            foreach (var task in tasks)
            {
                task.Wait();
                double[] results = task.Result;
                result0 += results[0];
                result1 += results[1];
                result2 += results[2];
                result3 += results[3];
                result4 += results[4];
            }

            // Вычисляем средние значения и заполняем строку DataGridView
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{N}x{M}";
            row.Cells[1].Value = result0 / Niter;
            row.Cells[2].Value = result1 / Niter;
            row.Cells[3].Value = result2 / Niter;
            row.Cells[4].Value = result3 / Niter;
            row.Cells[5].Value = result4 / Niter;

            return row;
        }
        DataGridViewRow getRow(int N, int M, int a, int b,int k=500,int pc=100,int pm = 100, int kpovt=5, int Niter = 2)
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
           

            for (int i = 0; i < Niter; i++)
            {
                GoldbergAlg alg = new GoldbergAlg(N, M, a, b, k, pc, pm, kpovt);
                Method meth = new Method(N, M, a, b);

                result0 += alg.SolveByT(meth);

                result1 += alg.SolveWithKrone(meth, 499, 1, 0);

                result2 += alg.SolveWithKrone(meth, 499, 0, 1);
                result3 += alg.SolveWithKrone(meth, 498, 2, 0);
                result4 += alg.SolveWithKrone(meth, 498, 0, 2);

               
            }
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{N}x{M}";
            row.Cells[1].Value = result0 / Niter;
            row.Cells[2].Value = result1 / Niter;
            row.Cells[3].Value = result2 / Niter;
            row.Cells[4].Value = result3 / Niter;
            row.Cells[5].Value = result4 / Niter;
            
            return row;

        }
        DataGridViewRow getResult(int N, int M, int a, int b, int k = 500, int pc = 100, int pm = 100, int kpovt = 5,
            string nameOfGen="Алгоритм Крона",
            string nameOfAlg="Алгоритм Голдберга",
            int Niter = 2)
        {
            double result0 = 0;
            
           

            GoldbergAlg alg = new GoldbergAlg(N, M, a, b, k, pc, pm, kpovt);
                Method meth = new Method(N, M, a, b);
                if(nameOfGen=="Алгоритм Крона")
                {
                    if (nameOfAlg == "Алгоритм Крона")
                    {
                        result0 += meth.SolveDoubleKrone();
                    }
                    else
                    {
                        result0 += alg.SolveWithKrone(meth, k / 2, k / 4, k / 4);
                    }
                    
                }
                else if (nameOfGen== "Случайное (классическое)")
                {
                    result0 += alg.SolveByT(meth);
                }
                else if (nameOfGen == "Алгоритм Критического пути")
                {
                    result0 += alg.SolveWithCM(meth,k/2,k/2*2/3, k / 2 * 2 / 3, k / 2 * 2 / 3);
                }
            DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
            row.Cells[0].Value = $"{N}x{M}";
            row.Cells[1].Value = result0 / Niter;
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
            List<int> Nlist = new List<int>();
            List<int> Mlist = new List<int>();
            try
            {
                //    if (textBox1.Text.Contains(","))
                //    {
                //        var Narray = textBox1.Text.Replace(" ", "").Split(','); ;
                //        foreach (var item in Narray)
                //        {
                //            Nlist.Add(Convert.ToInt16(item));
                //        }

                //    }
                //    else
                //    {
                //        var Narray = new List<int> { Convert.ToInt32(textBox1.Text) };
                //        foreach (var item in Narray)
                //        {
                //            Nlist.Add(Convert.ToInt16(item));
                //        }
                //    }
                //    if (textBox2.Text.Contains(","))
                //    {
                //        var Marray = textBox2.Text.Replace(" ","").Split(',');
                //        foreach (var item in Marray)
                //        {
                //            Mlist.Add(Convert.ToInt16(item));
                //        }
                //    }
                //    else
                //    {
                //        var Marray = new List<int> { Convert.ToInt32(textBox2.Text) };
                //        foreach (var item in Marray)
                //        {
                //            Mlist.Add(Convert.ToInt16(item));
                //        }
                //    }





                //int a = Convert.ToInt16(textBox3.Text);
                //int b = Convert.ToInt16(textBox4.Text);
                //string nameOfAlg = comboBox1.SelectedItem.ToString();
                //string nameOfGen = comboBox2.SelectedItem.ToString();
                //int pc = 0;
                //int pm = 0;
                //int k = 0;
                //int kpovt = 0;
                //if (nameOfAlg!="Алгоритм Крона")
                //{
                //    pc = Convert.ToInt16(textBox5.Text);
                //    pm = Convert.ToInt16(textBox6.Text);
                //    k = Convert.ToInt16(textBox7.Text);
                //    kpovt = Convert.ToInt16(textBox8.Text);
                //}

                int a = 10;
                int b = 30;

                DataGridViewRow row = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                row.Cells[0].Value = $"{a} - {b}";
                row.Cells[1].Value = "Голдберг случ";
                row.Cells[2].Value = "Голдберг пол случ, пол КП";
                row.Cells[3].Value = "Голдберг все КП";
                row.Cells[4].Value = "Голдберг все КП";
                row.Cells[5].Value = "Двухфазный Крон";
                row.Cells[6].Value = "Трёхфазный Крон с Коробком";
                dataGridView1.Rows.Add(row);
                List<DataGridViewRow> rows = new List<DataGridViewRow>();

                foreach (var N in Nlist)
                {
                    foreach (var M in Mlist)
                    {
                        rows.Add(getParrRow(N, M, a, b, 500, 100, 100, 40, 4));
                    }
                }

                Task.WaitAll(rows.Select(row1 => Task.Run(() => dataGridView1.Rows.Add(row1))).ToArray());
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
            catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
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
