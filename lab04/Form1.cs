using lab04.models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab04
{
    public partial class Form1 : Form
    {
        StudentContextDB context;
        public Form1()
        {
            InitializeComponent();
            context = new StudentContextDB();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                List<Facutly> listFacutlys = context.Facutlies.ToList();
                List<Student> liststudents = context.Students.ToList();
                FillFacultyCombobox(listFacutlys);
                BindGrid(liststudents);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }
        private void FillFacultyCombobox(List<Facutly> facutlys)
        {
            this.cmbFaculty.DataSource= facutlys;
            this.cmbFaculty.DisplayMember = "FacultyName";
            this.cmbFaculty.ValueMember = "FacultyID";
        }
        private void BindGrid(List<Student> students) {
            dgvStudent.Rows.Clear();
            foreach (var item in students) {
                int index = dgvStudent.Rows.Add();
                dgvStudent.Rows[index].Cells[0].Value = item.StudentID;
                dgvStudent.Rows[index].Cells[1].Value = item.FullName;
                dgvStudent.Rows[index].Cells[2].Value = item.AverageScore;
                dgvStudent.Rows[index].Cells[4].Value = item.Facutly.FacutlyName;
            }
        }

        private void dgvStudent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgvStudent.Rows[e.RowIndex];
            txtStudentID.Text = row.Cells[0].Value.ToString();
            txtFullName.Text = row.Cells[1].Value.ToString();
            txtAverageScore.Text = row.Cells[2].Value.ToString();
            cmbFaculty.Text= row.Cells[3].Value.ToString();
        }

        private bool RangBuoc()
        {
            if (txtStudentID.Text == "" || txtFullName.Text == "" || txtAverageScore.Text == "")
            {
                MessageBox.Show("Ban can phai dien day du thong tin!!");
                return false;
            }
            if (double.TryParse(txtAverageScore.Text, out double DTB) == false)
            {
                MessageBox.Show("Diem trung binh phai la so thuc");
                return false;
            }
            if (double.Parse(txtAverageScore.Text) < 0 || double.Parse(txtAverageScore.Text) > 10)
            {
                MessageBox.Show("Gia tri diem trung binh nam ngoai thang diem!!", "Thong Bao");
                return false;
            }
            return true;
        }
        private Student TimSV(string mssv)
        {
            return context.Students.FirstOrDefault(x => x.StudentID== mssv);
        }
        private void Reload()
        {
            List<Student> liststudents = context.Students.ToList();
            BindGrid(liststudents);
        }
        private void Clear()
        {
            txtStudentID.Text = " ";
            txtFullName.Text = " ";
            txtAverageScore.Text = " ";
            cmbFaculty.SelectedIndex = 0;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (RangBuoc() == false)
                return;
            if (TimSV(txtStudentID.Text) != null)
            {
                 MessageBox.Show("Sinh vien da ton tai!!");
            }
            Student sv = new Student()
            {
                StudentID = txtStudentID.Text,
                FullName = txtFullName.Text,
                AverageScore = double.Parse(txtAverageScore.Text),
                FacutlyID = int.Parse(cmbFaculty.SelectedValue.ToString()),
            };

            context.Students.Add(sv);
            context.SaveChanges();
            Reload();
            Clear();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            Student sv = new Student();
            if (RangBuoc() == false)
                return;
            if(TimSV(txtStudentID.Text) == null)
            {
                MessageBox.Show("Khong tim thay sinh vien de sua!!");
                return;
            }
            else
            {
                sv.StudentID = txtStudentID.Text;
                sv.FullName = txtFullName.Text;
                sv.AverageScore= double.Parse(txtAverageScore.Text);
                sv.FacutlyID =int.Parse(cmbFaculty.SelectedValue.ToString());
                MessageBox.Show("Cap nhat sinh vien thanh cong!!");

                context.SaveChanges();
                Reload();
                Clear();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Student sv=TimSV(txtStudentID.Text);
            if(sv==null)
            {
                MessageBox.Show("Khong tim thay sinh vien de xoa!!");
            }
            else
            {
                context.Students.Remove(sv);
                context.SaveChanges();
                Reload();
                Clear();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Ban co muon thoat!", "Thong Bao", MessageBoxButtons.OK);
            Close();
        }
    }
}
