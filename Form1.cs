using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QLCHOTHUENHA
{
    public partial class frmNha : Form
    {
        public frmNha()
        {
            InitializeComponent();
            Load += new EventHandler(Form_Load);
            btnThem.Click += new EventHandler(Them);
            btnCapNhat.Click += new EventHandler(CapNhat);
            btnXoa.Click += new EventHandler(Xoa);
            btnLamMoi.Click += new EventHandler(btnLamMoi_Click);
            dataGridView.CellClick += new DataGridViewCellEventHandler(Data_Click);
        }

        //Buộc dữ liệu từ dataGridView lên các control
        private void Data_Click(object sender, DataGridViewCellEventArgs e)
        {
            int i = dataGridView.CurrentCell.RowIndex;//dòng được chọn trên dataGridView
            txtMaNha.Text = dataGridView.Rows[i].Cells[0].Value.ToString();
            txtTenChuNha.Text = dataGridView.Rows[i].Cells[1].Value.ToString();
            txtGiaThue.Text = dataGridView.Rows[i].Cells[2].Value.ToString();
            string dct = dataGridView.Rows[i].Cells[3].Value.ToString();
            if (dct.Equals("True"))
                rdDaThue.Checked = true;
            else
                rdChuaThue.Checked = true;

        }

        private void Xoa(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("Bạn có muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (dr == DialogResult.Yes)
            {
                int i = dataGridView.CurrentCell.RowIndex;
                int ma = Convert.ToInt32(dataGridView.Rows[i].Cells[0].Value.ToString());
                string sql = string.Format("DELETE FROM NHA WHERE MaNha ='{0}'", ma);
                DataProvider.moKetNoi();
                DataProvider.updateData(sql);
                DataProvider.dongKetNoi();
                MessageBox.Show("Xóa thành công");
                loadNha();

            }
        }
        private void CapNhat(object sender, EventArgs e)
        {
            string sql = string.Format("UPDATE NHA SET MaNha=@manha,TenChuNha=@tencn,GiaThue=@gt," +
                "DaChoThue=@dct " +
                "WHERE manha ='{0}'", txtMaNha.Text);
            string[] name = { "@manha", "@tencn", "@gt", "@dct" };

            bool dct = true;
            if (rdDaThue.Checked == true)
                dct = false;
            object[] value = { txtMaNha.Text, txtTenChuNha.Text, float.Parse(txtGiaThue.Text), dct };

            DataProvider.moKetNoi();
            DataProvider.updateData(sql, value, name);
            MessageBox.Show("Sửa thành công");
            DataProvider.dongKetNoi();
            loadNha();
        }

        #region xác định tính hợp lệ của dữ liệu
        public bool isNumber(string value)
        {
            bool ktra;
            float result;
            ktra = float.TryParse(value, out result);
            //result = value sau khi chuyển đổi hoặc result =0 nếu chuyển đổi kiểu không hợp lệ
            return ktra;
        }

        #endregion
        private void Them(object sender, EventArgs e)
        {
            // string sql1 = string.Format("SELECT COUNT(*) FROM NhanVien WHERE maNV='{0}'",txtMaNV.Text);
            //  if ((DataProvider.checkData(sql1)==0) && (isNumber(txtHSL.Text)))

            // Kiểm tra mã nhà đã tồn tại chưa
            string sqlCheck = $"SELECT COUNT(*) FROM NHA WHERE MaNha = '{txtMaNha.Text}'";
            DataProvider.moKetNoi();
            if (DataProvider.checkData(sqlCheck) > 0)
            {
                MessageBox.Show("Mã nhà đã tồn tại. Vui lòng nhập mã khác.");
                DataProvider.dongKetNoi();
                return;
            }
            DataProvider.dongKetNoi();
              
            if ((isNumber(txtGiaThue.Text)))
            {
                string sql = "INSERT INTO NHA(MaNha,TenChuNha,GiaThue,DaChoThue) " +
                    "VALUES(@manha, @tencn, @gt, @dct)";
            string[] name = { "@manha", "@tencn", "@gt", "@dct" };

                bool dct = true;
                if (rdDaThue.Checked == true)
                    dct = false;
                object[] value = { txtMaNha.Text, txtTenChuNha.Text, float.Parse(txtGiaThue.Text), dct };

                DataProvider.moKetNoi();
                DataProvider.updateData(sql, value, name);
                DataProvider.dongKetNoi();
                loadNha();
            }
        }

        private void btnLamMoi_Click(object sender, EventArgs e)
        {
            txtMaNha.Clear();
            txtTenChuNha.Clear();
            txtGiaThue.Clear();
            rdDaThue.Checked = false;
            rdChuaThue.Checked = true; // Mặc định là "Chưa thuê"
        }

        #region Load dữ liệu
        
        //Load dữ liệu DSNV
        public void loadNha()
        {
            string sql = "SELECT * FROM NHA";
            dataGridView.DataSource = DataProvider.getTable(sql);

            DataTable dt = (DataTable)dataGridView.DataSource;
        }
        #endregion
        private void Form_Load(object sender, EventArgs e)
        {
            DataProvider.moKetNoi();
            loadNha();
            DataProvider.dongKetNoi();
        }

    }
}
