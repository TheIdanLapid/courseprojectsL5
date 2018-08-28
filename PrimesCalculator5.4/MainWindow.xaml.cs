using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrimesCalculator5._4
{
    public partial class MainWindow
    {
        private CancellationTokenSource _cts;
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            _cts = new CancellationTokenSource();
            int a = int.Parse(textBox1.Text);
            int b = int.Parse(textBox2.Text);

            progressBar1.Value = a;
            progressBar1.Minimum = a;
            progressBar1.Maximum = b;

            IProgress<int> progress = new Progress<int>(n =>
            {
                progressBar1.Value = n;
            });

            int count = await CountPrimesAsync(a, b,_cts.Token, progress);
            label1.Text = count.ToString();
        }

        public async Task<int> CountPrimesAsync(int a, int b,CancellationToken cancellationToken,IProgress<int> progress)
        {
            int count = 0;

            for (int i = a; i <= b && !cancellationToken.IsCancellationRequested; i++)
            {
                progress.Report(i);
                if (await IsPrime(i))
                {
                    count++;
                }
            }

            return count;
        }

        public Task<bool> IsPrime(int number)
        {
            TaskCompletionSource<bool> a = new TaskCompletionSource<bool>();

            if (number == 0 || number == 1)
            {
                a.SetResult(false);
                return a.Task;
            }

            int k = (int)Math.Sqrt(number);
            for (int i = 2; i <= k; i++)
            {
                if (number % i == 0)
                {
                    a.SetResult(false);
                    return a.Task;
                }
            }

            a.SetResult(true);
            return a.Task;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _cts?.Cancel();
        }
        
        private static IEnumerable<int> DoCalc(int start, int end) {
            List<int> primes = new List<int>();
            for(int n = start; n <= end; n++) {
                bool prime = true;
                int limit = (int)Math.Sqrt(n);
                for(int i = 2; i <= limit; i++)
                    if(n % i == 0) {
                        prime = false;
                        break;
                    }
                if(prime)
                    primes.Add(n);
            }
            return primes;
        }
    }
}