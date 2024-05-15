using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Net.Mail;
using System.IO;
using GasAgencyManagementProject.Models.Master;
using GasAgencyManagementProject.Models.BLL;

namespace GasAgencyManagementProject.Models.DLL
{
    public class Dll
    {
        private string _ConnectionString;
        private string _ProviderName;
        private string _Output = "";
        private SqlConnection _objConnection;
        private SqlCommand _objCommand;
        private DataTable objDataTable;
        SqlDataAdapter _objDataAdapter;
        private SqlParameter _objParameter;
        public string Query = "";
        public string Query1 = "";
        public string Query2 = "";
        public object Lquery = "";
        public object Lquery1 = "";
        public object Lquery2 = "";
        public Dll(string ConnectionString)
        {
            _ConnectionString = WebConfigurationManager.ConnectionStrings[ConnectionString].ConnectionString;
            _ProviderName = WebConfigurationManager.ConnectionStrings[ConnectionString].ProviderName;
            _Output = "";
        }
        public Dll()
        {
            _ProviderName = "";
            _ConnectionString = "CONN";
            _Output = "";
        }
        public string ConnectionString
        {
            get
            {
                return _ConnectionString;
            }
            set
            {
                _ConnectionString = WebConfigurationManager.ConnectionStrings["CONN"].ConnectionString;
                _ProviderName = WebConfigurationManager.ConnectionStrings["CONN"].ProviderName;
            }
        }
        public string Provider
        {
            get
            {
                return _ProviderName;
            }
        }
        string Constrinf = WebConfigurationManager.ConnectionStrings["CONN"].ConnectionString;
        public SqlConnection GetConnection()
        {
            _objConnection = new SqlConnection(Constrinf);
            return _objConnection;
        }
        public string ExecuteNonQuery(string Query)
        {
            _objConnection = this.GetConnection();
            _objCommand = new SqlCommand(Query, _objConnection);
            _objConnection.Open();
            _Output = Convert.ToString(_objCommand.ExecuteNonQuery());
            _objConnection.Close();
            return _Output;
        }
        public string ExecuteProcedure(string SPName, string[] objParams, object[] objValues, string objOutParameter)
        {
            bool Flag = false;
            try
            {
                if (objParams.Length != objValues.Length)
                    _Output = "Incorrect no. of Parameters or Value to the Stored Procedure";
                else
                {
                    _objConnection = this.GetConnection();
                    _objCommand = new SqlCommand(SPName, _objConnection);
                    _objCommand.CommandTimeout = 80000;
                    _objCommand.CommandType = CommandType.StoredProcedure;
                    for (int i = 0; i < objParams.Length; i++)
                    {
                        _objParameter = new SqlParameter(objParams[i], objValues[i]);
                        _objCommand.Parameters.Add(_objParameter);
                    }
                    if (objOutParameter != "")
                    {
                        Flag = true;
                        _objParameter = new SqlParameter(objOutParameter, SqlDbType.VarChar, 8000);
                        _objParameter.Direction = ParameterDirection.Output;
                        _objCommand.Parameters.Add(_objParameter);
                    }
                    _objConnection.Open();
                    _objCommand.ExecuteNonQuery();
                    if (Flag)
                        _Output = (string)_objCommand.Parameters[objOutParameter].Value;

                    _objConnection.Close();
                }
            }
            catch (SqlException ex)
            {
                _Output = ex.Message;
                _Output = "An error occurs for accessing database please try again !";
            }
            catch (Exception ex)
            {
                _Output = ex.Message;
                _Output = "An internal error occurs  please try again !";
            }
            return _Output;
        }
        public DataTable FillDataTable(string SPName, string[] objParams, object[] objValues)
        {
            _objConnection = this.GetConnection();
            _objCommand = new SqlCommand(SPName, _objConnection);
            _objCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < objParams.Length; i++)
            {
                _objParameter = new SqlParameter(objParams[i], objValues[i]);
                _objCommand.Parameters.Add(_objParameter);
            }
            _objConnection.Open();
            SqlDataAdapter objDataAdapter1 = new SqlDataAdapter(_objCommand);
            objDataTable = new DataTable();
            objDataAdapter1.Fill(objDataTable);
            _objConnection.Close();
            return objDataTable;
        }
        public DataSet FillDataSet(string SPName, string[] objParams, object[] objValues)
        {
            _objConnection = this.GetConnection();
            _objCommand = new SqlCommand(SPName, _objConnection);
            _objCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < objParams.Length; i++)
            {
                _objParameter = new SqlParameter(objParams[i], objValues[i]);
                _objCommand.Parameters.Add(_objParameter);
            }
            _objConnection.Open();
            SqlDataAdapter objDataAdapter1 = new SqlDataAdapter(_objCommand);
            DataSet ds = new DataSet();
            objDataAdapter1.Fill(ds);
            _objConnection.Close();
            return ds;
        }
        public DataTable FillDataTable(string Query)
        {
            _objConnection = this.GetConnection();
            _objDataAdapter = new SqlDataAdapter(Query, _objConnection);
            objDataTable = new DataTable();
            _objDataAdapter.Fill(objDataTable);
            return objDataTable;
        }
        public string ExecuteScalar(string Query)
        {
            _objConnection = this.GetConnection();
            _objCommand = new SqlCommand(Query, _objConnection);
            //_objCommand.CommandTimeout = 120;
            _objConnection.Open();
            _Output = Convert.ToString(_objCommand.ExecuteScalar());
            _objConnection.Close();
            return _Output;
        }
        public string ExecuteScalar(string SPName, string[] objParams, object[] objValues)
        {
            _objConnection = this.GetConnection();
            _objCommand = new SqlCommand(SPName, _objConnection);
            _objCommand.CommandType = CommandType.StoredProcedure;
            for (int i = 0; i < objParams.Length; i++)
            {
                _objParameter = new SqlParameter(objParams[i], objValues[i]);
                _objCommand.Parameters.Add(_objParameter);
            }
            _objConnection.Open();
            _Output = Convert.ToString(_objCommand.ExecuteScalar());
            _objConnection.Close();
            return _Output;
        }   
    }
}