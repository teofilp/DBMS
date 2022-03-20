using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace UniversitiesManagementSystem
{
    public partial class UniversitiesForm : Form
    {
        SqlConnection Connection;
        SqlDataAdapter UniversitiesAdapter, FacultiesAdapter;
        DataSet DataSet;
        BindingSource UniversitiesSource, FacultiesSource;
        SqlCommandBuilder SqlCommandBuilder;

        private void updateButton_Click(object sender, EventArgs e)
        {
            FacultiesAdapter.Update(DataSet, Constants.FacultiesTableName);
        }

        public UniversitiesForm()
        {
            InitializeComponent();
        }

        private void connectButton_Click(object sender, EventArgs e)
        {
            Connection = new SqlConnection(Constants.ConnectionString);
            DataSet = new DataSet();

            SetupAdapters();
            LoadData();

            AddFacultyUniversityRelation();
            SetupBindingSource();
            BindDataGridViews();
        }

        private void BindDataGridViews()
        {
            universitiesDataGrid.DataSource = UniversitiesSource;
            facultiesDataGrid.DataSource = FacultiesSource;
        }

        private void SetupBindingSource()
        {
            UniversitiesSource = new BindingSource();
            FacultiesSource = new BindingSource();

            UniversitiesSource.DataSource = DataSet;
            UniversitiesSource.DataMember = Constants.UniversitiesTableName;

            FacultiesSource.DataSource = UniversitiesSource;
            FacultiesSource.DataMember = Constants.FacultiesUniveristiesForeignKey;
        }

        private void AddFacultyUniversityRelation()
        {
            DataRelation relation = new DataRelation(Constants.FacultiesUniveristiesForeignKey, DataSet.Tables[Constants.UniversitiesTableName].Columns["Id"],
                            DataSet.Tables[Constants.FacultiesTableName].Columns["UniversityId"]);

            DataSet.Relations.Add(relation);
        }

        private void LoadData()
        {
            UniversitiesAdapter.Fill(DataSet, Constants.UniversitiesTableName);
            FacultiesAdapter.Fill(DataSet, Constants.FacultiesTableName);
        }

        private void SetupAdapters()
        {
            UniversitiesAdapter = new SqlDataAdapter(Constants.GetSelectFromQueryString(Constants.UniversitiesTableName), Connection);
            FacultiesAdapter = new SqlDataAdapter(Constants.GetSelectFromQueryString(Constants.FacultiesTableName), Connection);

            SqlCommandBuilder = new SqlCommandBuilder(FacultiesAdapter);
        }
    }
}
