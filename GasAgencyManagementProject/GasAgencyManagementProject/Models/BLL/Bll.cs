using GasAgencyManagementProject.Models.DLL;
using GasAgencyManagementProject.Models.Master;
using GasAgencyManagementProject.Models.Transaction;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace GasAgencyManagementProject.Models.BLL
{
    public class Bll
    {
        #region Global User
        public string GetEncriptText(string PlaneText)
        {
            string EncriptText = ClsEngine.Encrypt(PlaneText, true);
            return EncriptText;
        }

        private string GetDecriptText(string EncryptText)
        {
            string DecriptText = ClsEngine.Decrypt(EncryptText, true);
            return DecriptText;
        }

        public static List<T> BindList<T>(DataTable dt)
        {
            // Example 1:
            // Get private fields + non properties
            var fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

            // Example 2: Your case
            // Get all public fields
            // var fields = typeof(T).GetFields();

            List<T> lst = new List<T>();

            foreach (DataRow dr in dt.Rows)
            {
                // Create the object of T
                var ob = Activator.CreateInstance<T>();

                foreach (var fieldInfo in fields)
                {
                    var fieldinfo1 = fieldInfo.Name.Split('>')[0].Split('<')[1].ToString();
                    foreach (DataColumn dc in dt.Columns)
                    {
                        // Matching the columns with fields
                        if (fieldinfo1 == dc.ColumnName)
                        {
                            // Get the value from the datatable cell
                            object value = dr[dc.ColumnName];
                            if (value == DBNull.Value)
                            {
                                value = null;
                            }
                            //if (value == null)
                            //{
                            //    value = "";
                            //}
                            if (value != null)
                            {
                                Type t = value.GetType();

                                if (t.Equals(typeof(int)))
                                {
                                    fieldInfo.SetValue(ob, value);
                                }
                                else if (t.Equals(typeof(long)))
                                {
                                    fieldInfo.SetValue(ob, Convert.ToInt32(value));

                                }
                                else if (t.Equals(typeof(UInt32)))
                                {
                                    fieldInfo.SetValue(ob, value);

                                }
                                else if (t.Equals(typeof(UInt64)))
                                {
                                    fieldInfo.SetValue(ob, value);

                                }
                                else if (t.Equals(typeof(decimal)))
                                { fieldInfo.SetValue(ob, value); }
                                else if (t.Equals(typeof(Byte[])))
                                { fieldInfo.SetValue(ob, value); }
                                else if (t.Equals(typeof(double)))
                                { fieldInfo.SetValue(ob, value); }
                                else
                                {
                                    // Set the value into the object
                                    fieldInfo.SetValue(ob, value.ToString().Trim());
                                }
                            }
                            break;
                        }
                    }
                }
                lst.Add(ob);
            }
            return lst;
        }

        public T BindData<T>(DataTable dt)
        {
            DataRow dr = dt.Rows[0];
            // Get all columns' name
            List<string> columns = new List<string>();
            foreach (DataColumn dc in dt.Columns)
            {
                columns.Add(dc.ColumnName);
            }

            // Create object
            var ob = Activator.CreateInstance<T>();

            // Get all fields
            var fields = typeof(T).GetFields();
            foreach (var fieldInfo in fields)
            {
                if (columns.Contains(fieldInfo.Name))
                {
                    // Fill the data into the field
                    fieldInfo.SetValue(ob, dr[fieldInfo.Name]);
                }
            }

            //Get all properties
           var properties = typeof(T).GetProperties();
            foreach (var propertyInfo in properties)
            {
                if (columns.Contains(propertyInfo.Name))
                {
                    // Fill the data into the property
                    if (string.IsNullOrEmpty(dr[propertyInfo.Name].ToString()))
                    {
                        propertyInfo.SetValue(ob, "");
                    }
                    else { propertyInfo.SetValue(ob, dr[propertyInfo.Name]); }
                }
            }
            return ob;

            //var properties = typeof(T).GetProperties();
            //foreach (var propertyInfo in properties)
            //{
            //    if (columns.Contains(propertyInfo.Name))
            //    {
            //        var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
            //        if (propertyType == typeof(string))
            //        {
            //            propertyInfo.SetValue(ob, dr[propertyInfo.Name].ToString());
            //        }
            //        else if (propertyType == typeof(int))
            //        {
            //            if (string.IsNullOrEmpty(dr[propertyInfo.Name].ToString()))
            //            {
            //                propertyInfo.SetValue(ob, 0); // Set to 0 for integers
            //            }
            //            else
            //            {
            //                propertyInfo.SetValue(ob, Convert.ToInt32(dr[propertyInfo.Name]));
            //            }
            //        }
            //    }
            //}
            //return ob;
        }

        public DataTable ToDataTable<T>(List<T> items)
        {

            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties

            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {

                //Setting column names as Property names

                dataTable.Columns.Add(prop.Name);

            }

            foreach (T item in items)
            {

                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {

                    //inserting property values to datatable rows

                    values[i] = Props[i].GetValue(item, null);

                }

                dataTable.Rows.Add(values);

            }

            //put a breakpoint here and check datatable

            return dataTable;

        }

        #region Converting dd/mm/yyyy Format to mm/dd/yyyy

        public string SetDateFormat(string ddmmyyy)//Converting dd/mm/yyyy Format to mm/dd/yyyy
        {
            string mmddyyy = "";
            if (ddmmyyy != null && ddmmyyy != "")
            {
                if (ddmmyyy.Contains("/"))
                {
                    string dd = ddmmyyy.Split('/')[0];
                    string mm = ddmmyyy.Split('/')[1];
                    string yyyy = ddmmyyy.Split('/')[2];
                    mmddyyy = mm + "/" + dd + "/" + yyyy;
                }
            }
            return mmddyyy;
        }
        #endregion

        public string ConvertNumbertoWords(int number)
        {
            if (number == 0)
                return "Zero";
            if (number < 0)
                return "minus " + ConvertNumbertoWords(Math.Abs(number));
            string words = "";
            if ((number / 10000000) > 0)
            {
                words += ConvertNumbertoWords(number / 10000000) + " Crore ";
                number %= 10000000;
            }
            if ((number / 100000) > 0)
            {
                words += ConvertNumbertoWords(number / 100000) + " Lakh ";
                number %= 100000;
            }
            if ((number / 1000) > 0)
            {
                words += ConvertNumbertoWords(number / 1000) + " Thousand ";
                number %= 1000;
            }
            if ((number / 100) > 0)
            {
                words += ConvertNumbertoWords(number / 100) + " Hundred ";
                number %= 100;
            }
            if (number > 0)
            {
                if (words != "")
                    words += "And ";
                var unitsMap = new[] { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
                var tensMap = new[] { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += " " + unitsMap[number % 10];
                }
            }
            return words;
        }

        #region Calculation
        private static String ones(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = "";
            switch (_Number)
            {

                case 1:
                    name = "One";
                    break;
                case 2:
                    name = "Two";
                    break;
                case 3:
                    name = "Three";
                    break;
                case 4:
                    name = "Four";
                    break;
                case 5:
                    name = "Five";
                    break;
                case 6:
                    name = "Six";
                    break;
                case 7:
                    name = "Seven";
                    break;
                case 8:
                    name = "Eight";
                    break;
                case 9:
                    name = "Nine";
                    break;
            }
            return name;
        }
        private static String tens(String Number)
        {
            int _Number = Convert.ToInt32(Number);
            String name = null;
            switch (_Number)
            {
                case 10:
                    name = "Ten";
                    break;
                case 11:
                    name = "Eleven";
                    break;
                case 12:
                    name = "Twelve";
                    break;
                case 13:
                    name = "Thirteen";
                    break;
                case 14:
                    name = "Fourteen";
                    break;
                case 15:
                    name = "Fifteen";
                    break;
                case 16:
                    name = "Sixteen";
                    break;
                case 17:
                    name = "Seventeen";
                    break;
                case 18:
                    name = "Eighteen";
                    break;
                case 19:
                    name = "Nineteen";
                    break;
                case 20:
                    name = "Twenty";
                    break;
                case 30:
                    name = "Thirty";
                    break;
                case 40:
                    name = "Fourty";
                    break;
                case 50:
                    name = "Fifty";
                    break;
                case 60:
                    name = "Sixty";
                    break;
                case 70:
                    name = "Seventy";
                    break;
                case 80:
                    name = "Eighty";
                    break;
                case 90:
                    name = "Ninety";
                    break;
                default:
                    if (_Number > 0)
                    {
                        name = tens(Number.Substring(0, 1) + "0") + " " + ones(Number.Substring(1));
                    }
                    break;
            }
            return name;
        }
        private static String ConvertWholeNumber(String Number)
        {
            string word = "";
            try
            {
                bool beginsZero = false;//tests for 0XX   
                bool isDone = false;//test if already translated   
                double dblAmt = (Convert.ToDouble(Number));
                //if ((dblAmt > 0) && number.StartsWith("0"))   
                if (dblAmt > 0)
                {//test for zero or digit zero in a nuemric   
                    beginsZero = Number.StartsWith("0");

                    int numDigits = Number.Length;
                    int pos = 0;//store digit grouping   
                    String place = "";//digit grouping name:hundres,thousand,etc...   
                    switch (numDigits)
                    {
                        case 1://ones' range   

                            word = ones(Number);
                            isDone = true;
                            break;
                        case 2://tens' range   
                            word = tens(Number);
                            isDone = true;
                            break;
                        case 3://hundreds' range   
                            pos = (numDigits % 3) + 1;
                            place = " Hundred ";
                            break;
                        case 4://thousands' range   
                        case 5:
                        case 6:
                            pos = (numDigits % 4) + 1;
                            place = " Thousand ";
                            break;
                        case 7://millions' range   
                        case 8:
                        case 9:
                            pos = (numDigits % 7) + 1;
                            place = " Million ";
                            break;
                        case 10://Billions's range   
                        case 11:
                        case 12:

                            pos = (numDigits % 10) + 1;
                            place = " Billion ";
                            break;
                        //add extra case options for anything above Billion...   
                        default:
                            isDone = true;
                            break;
                    }
                    if (!isDone)
                    {//if transalation is not done, continue...(Recursion comes in now!!)   
                        if (Number.Substring(0, pos) != "0" && Number.Substring(pos) != "0")
                        {
                            try
                            {
                                word = ConvertWholeNumber(Number.Substring(0, pos)) + place + ConvertWholeNumber(Number.Substring(pos));
                            }
                            catch { }
                        }
                        else
                        {
                            word = ConvertWholeNumber(Number.Substring(0, pos)) + ConvertWholeNumber(Number.Substring(pos));
                        }


                    }
                    //ignore digit grouping names   
                    if (word.Trim().Equals(place.Trim())) word = "";
                }
            }
            catch { }
            return word.Trim();
        }
        public string changeToWords(String numb)
        {
            String val = "", wholeNo = numb, points = "", andStr = "", pointStr = "";
            String endStr = "Only";
            try
            {
                int decimalPlace = numb.IndexOf(".");
                if (decimalPlace > 0)
                {
                    wholeNo = numb.Substring(0, decimalPlace);
                    points = numb.Substring(decimalPlace + 1);
                    if (Convert.ToInt32(points) > 0)
                    {
                        andStr = "and";// just to separate whole numbers from points/cents   
                        endStr = "Paisa " + endStr;//Cents   
                        pointStr = ConvertDecimals(points);
                    }
                }
                val = String.Format("{0} {1}{2} {3}", ConvertWholeNumber(wholeNo).Trim(), andStr, pointStr, endStr);
            }
            catch { }
            return val;
        }
        private static String ConvertDecimals(String number)
        {
            String cd = "", digit = "", engOne = "";
            for (int i = 0; i < number.Length; i++)
            {
                digit = number[i].ToString();
                if (digit.Equals("0"))
                {
                    engOne = "Zero";
                }
                else
                {
                    engOne = ones(digit);
                }
                cd += " " + engOne;
            }
            return cd;
        }
        #endregion Calculation
        #endregion

        #region Validate Login
        public Tbl_Mstr_User BLLCheckLogin(Tbl_Mstr_User Login)
        {
            Tbl_Mstr_User objLoginModels = new Tbl_Mstr_User();
            string uemail, pwd = "";
            uemail = Login.User_Email;
            pwd = GetEncriptText(Login.User_Pwd);
            objLoginModels.msg = new Dll().ExecuteProcedure("USP_Tbl_Mstr_User_ValidateLogin", new string[] { "@User_Email", "@User_Pwd" }, new object[] { uemail, pwd }, "@msg");
            if (objLoginModels.msg.Equals("valid"))
            {
                DataTable dt = new DataTable();
                dt = new Dll().FillDataTable("USP_Tbl_Mstr_User_DtlByUnamePwd", new string[] { "@Action", "@User_Email", "@User_Pwd" }, new object[] { "GET", uemail, pwd });
                string logId = dt.Rows[0]["User_Id"].ToString();
                string password = GetDecriptText(dt.Rows[0]["User_Pwd"].ToString());
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
                {
                    objLoginModels.User_Id = int.Parse(dt.Rows[0]["User_Id"].ToString());
                    objLoginModels.User_Name = dt.Rows[0]["User_Name"].ToString();
                    objLoginModels.User_Type = dt.Rows[0]["User_Type"].ToString();
                    objLoginModels.User_Email = uemail;
                    objLoginModels.User_Pwd = pwd;
                    objLoginModels.User_PhNo = dt.Rows[0]["User_PhNo"].ToString();
                }
            }
            return objLoginModels;
        }
        #endregion

        #region changepassword
        public string BLL_ChangePassword(ChangePasswordModel objChangePass)
        {
            return new Dll().ExecuteProcedure("USP_ChangePassword", new string[] { "@UserId", "@NewPwd", "@User_Pwd" },
                new object[] { objChangePass.UserId, GetEncriptText(objChangePass.NewPwd), GetEncriptText(objChangePass.Pwd) }, "@msg");
        }
        #endregion

        #region Master

        #region Organisation Master
        public Tbl_Mstr_Organization BLLOrganizationDtl()
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Organization obj = new Tbl_Mstr_Organization();
            dt = new Dll().FillDataTable("SELECT  [Org_Id] ,[Org_Name] ,[Org_Srt_Name] ,[Org_Addrs] ,[Org_PhNo] ,[Org_FaxNo] ,[Org_Email],[Org_Website] ,[Org_TinNo] ,[Org_CSTNo] ,[Org_PanNo] ,[Org_SrvcTaxNo],[Org_RegNo],[Org_TanNo],[Org_Logo],[Created_By],[Modified_By] FROM [dbo].[Tbl_Mstr_Organization]");
            if (dt != null)
            {
                obj = BindData<Tbl_Mstr_Organization>(dt);
            }
            return obj;
        }



        public string BLLSaveOrganisation(Tbl_Mstr_Organization obj)
        {
            return new Dll().ExecuteProcedure
                (
                  "[dbo].[USP_Tbl_Mstr_Organization_IU]",
                  new string[]{"@Org_Id","@Org_Name","@Org_Srt_Name","@Org_Addrs","@Org_PhNo","@Org_FaxNo","@Org_Email","@Org_Website","@Org_TinNo","@Org_CSTNo","@Org_PanNo",
                  "@Org_SrvcTaxNo","@Org_RegNo","@Org_TanNo","@Org_Logo","@Created_By","@Modified_By","@Action"},


                  new object[] { obj.Org_Id, obj.Org_Name, obj.Org_Srt_Name, obj.Org_Addrs, obj.Org_PhNo, obj.Org_FaxNo, obj.Org_Email, obj.Org_Website, obj.Org_TinNo, obj.Org_CSTNo, obj.Org_PanNo, obj.Org_SrvcTaxNo, obj.Org_RegNo, obj.Org_TanNo, obj.Org_Logo, obj.Created_By, obj.Modified_By, obj.Action }, "@msg"
                );
        }

        public string BLLSaveImage(string hdnimage, string hdnext, string Filepath)
        {
            string ImgName = string.Empty;

            string filePath = string.Empty;
            if (hdnimage.Length > 0)
            {
                byte[] imageBytes = Convert.FromBase64String(hdnimage.ToString().Split(',')[1].ToString());
                MemoryStream ms = new MemoryStream();//imageBytes, 0, imageBytes.Length);
                ms.Read(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image objImage = System.Drawing.Image.FromStream(ms, true);
                // System.Drawing.Image SaveImage = ScaleImageClassified(objImage, 600);

                ImgName = DateTime.Now.ToString("ddMMyyyhhmmssffff") + "." + hdnext;
                //filePath = "GobellWebservices/Documents/Classifieds/" + ImgName;
                filePath = Filepath + ImgName;
                objImage.Save(HttpContext.Current.Server.MapPath("~/" + filePath));
            }
            return ImgName;
        }
        #endregion

        #region User


        public string BLLMstrUserIUD(Tbl_Mstr_User obj)
        {

            return new Dll().ExecuteProcedure
                (
                   //procedure attribute
                   "USP_Tbl_Mstr_User_IUD",
                   new string[] { "@User_Id", "@User_Pwd", "@User_Name", "@User_Type", "@User_PhNo", "@User_Email", "@Created_By", "@Action" },

                   //module attribute
                   new object[] { obj.User_Id, GetEncriptText(obj.User_Pwd), obj.User_Name, obj.User_Type, obj.User_PhNo, obj.User_Email,obj.Created_By, obj.Action },
                   "@msg"
                );
        }
        public List<Tbl_Mstr_User> BllAllUser()
        {
            List<Tbl_Mstr_User> lstUser = new List<Tbl_Mstr_User>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_User_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstUser = BindList<Tbl_Mstr_User>(dt);
            }
            return lstUser;
        }

        public UserDetail BLLUserDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_User> listTbl_Mstr_User = new List<Tbl_Mstr_User>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_User_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listTbl_Mstr_User = BindList<Tbl_Mstr_User>(dt);

            }

            UserDetail UserDetail = new UserDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_User] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listTbl_Mstr_User[0].TotalCount),
                aaData = listTbl_Mstr_User
            };
            return UserDetail;
        }

        //public Tbl_Mstr_User BLLUserById(string Id)
        //{
        //    DataTable dt = new DataTable();
        //    Tbl_Mstr_User Tbl_Mstr_User = new Tbl_Mstr_User();
        //    dt = new Dll().FillDataTable("USP_Tbl_Mstr_User_View", new string[] { "@Id", "@Action" },
        //        new object[] { Id, "AllById" });
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        Tbl_Mstr_User = BindData<Tbl_Mstr_User>(dt);

        //    }
        //    return Tbl_Mstr_User;
        //}

        public Tbl_Mstr_User BLLUserById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_User Tbl_Mstr_User = new Tbl_Mstr_User();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_User_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });

            if (dt != null && dt.Rows.Count > 0)
            {
                Tbl_Mstr_User = BindData<Tbl_Mstr_User>(dt);

                // Decrypt the password if it's encrypted
                if (!string.IsNullOrEmpty(Tbl_Mstr_User.User_Pwd))
                {
                    //Tbl_Mstr_User.User_Pwd = ClsEngine.Decrypt(Tbl_Mstr_User.User_Pwd, true);
                    Tbl_Mstr_User.User_Pwd =GetDecriptText(Tbl_Mstr_User.User_Pwd);

                }
            }

            return Tbl_Mstr_User;
        }


        #endregion

        #region UserAccess

        // there is no procedure is created for Tbl_Mstr_UserAccess Table


        public List<Tbl_Mstr_Page> BLL_CheckAccess(int Uid)
        {

            DataTable dt = new DataTable();
            List<Tbl_Mstr_Page> LIST = new List<Tbl_Mstr_Page>();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@Action", "@User_ID" }, new object[] { "CHKREAD", Uid });
            if (dt != null)
            {
                LIST = BindList<Tbl_Mstr_Page>(dt);
            }
            return LIST;
        }

        public List<Tbl_Mstr_User> BLL_GetUserName(string uid)
        {
            List<Tbl_Mstr_User> lsttblMUserAccess = new List<Tbl_Mstr_User>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@Action", "@User_ID" }, new object[] { "UserName", int.Parse(uid) });
            if (dt != null)
            {
                lsttblMUserAccess = BindList<Tbl_Mstr_User>(dt);
            }
            return lsttblMUserAccess;
        }

        public List<Tbl_Mstr_Module> BLL_GetModuleName()
        {
            List<Tbl_Mstr_Module> lsttblModuleAccess = new List<Tbl_Mstr_Module>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@Action" }, new object[] { "ModuleName" });
            if (dt != null)
            {
                lsttblModuleAccess = BindList<Tbl_Mstr_Module>(dt);
            }
            return lsttblModuleAccess;
        }

        public List<Tbl_Mstr_Page> BLLGet_Page_Details(int mid)
        {
            List<Tbl_Mstr_Page> page = new List<Tbl_Mstr_Page>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@MID", "@ACTION" }, new object[] { mid, "GETPAGELIST" });
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
            {
                page = BindList<Tbl_Mstr_Page>(dt);
            }
            return page;
        }

        public string BLLSaveUserAccess(string LIST, int UID, int UPDATEDBY)
        {
            try
            {
                return new Dll().ExecuteProcedure
                 (
                 "USP_Tbl_Mstr_UserAccess",
                 new string[] { "@LIST", "@UID", "@UPDATEDBY" },
                 new object[] { LIST, UID, UPDATEDBY },
                 "@msg"
                 );
            }
            catch
            {
                return "Something went wrong";
            }
        }

        public List<Tbl_Mstr_Page> BLLGet_Page_Details_acc(int uid, int mid)
        {
            DataTable dt = new DataTable();
            List<Tbl_Mstr_Page> page = new List<Tbl_Mstr_Page>();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@User_ID", "@MID", "@ACTION", }, new object[] { uid, mid, "SLTACC" });
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "")
            {
                page = BindList<Tbl_Mstr_Page>(dt);
            }
            return page;
        }

        public Tbl_Mstr_Page BLL_CHK_RWD_Access(int Uid, int pid, string type)
        {
            DataTable dt = new DataTable();
            List<Tbl_Mstr_Page> LIST = new List<Tbl_Mstr_Page>();
            Tbl_Mstr_Page M = new Tbl_Mstr_Page();
            dt = new Dll().FillDataTable("USP_GetPageList", new string[] { "@ACTION", "@User_ID", "@PID", "@TYPE" }, new object[] { "CHKRWDACCESS", Uid, pid, type });
            if (dt != null)
            {
                foreach (DataRow drr in dt.Rows)
                {
                    if (drr["PERMISSION"].ToString() == "1")
                    {
                        M.PERMISSION = true;
                    }
                    else { M.PERMISSION = false; }
                }
            }
            return M;
        }
        #endregion

        #region Supplier
        public string BLLMstrSupplierIUD(Tbl_Mstr_Supplier obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Supplier_IUD",
                   new string[] { "@Suplr_Id", "@Suplr_Name", "@Suplr_Status", "@Created_By", "@Action" },
                   new object[] { obj.Suplr_Id, obj.Suplr_Name, obj.Suplr_Status,obj.Created_By, obj.Action },
                   "@msg"
                );
        }

        public List<Tbl_Mstr_Supplier> BllAllSupplier()
        {
            List<Tbl_Mstr_Supplier> lstSupplier = new List<Tbl_Mstr_Supplier>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Supplier_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstSupplier = BindList<Tbl_Mstr_Supplier>(dt);
            }
            return lstSupplier;
        }

        public SupplierDetail BLLSupplierDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Supplier> listSupplier = new List<Tbl_Mstr_Supplier>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Supplier_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listSupplier = BindList<Tbl_Mstr_Supplier>(dt);

            }

            SupplierDetail supplierDetail = new SupplierDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Supplier] where D_flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listSupplier[0].TotalCount),
                aaData = listSupplier
            };
            return supplierDetail;
        }
        public Tbl_Mstr_Supplier BllSupplierById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Supplier area = new Tbl_Mstr_Supplier();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Supplier_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                area = BindData<Tbl_Mstr_Supplier>(dt);

            }
            return area;
        }
        #endregion

        #region Master Area


        public string BLLMstrAreaIUD(Tbl_Mstr_Area obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Area_IUD",
                   new string[] { "@Area_Id", "@Area_Info", "@Area_Details", "@Created_By", "@Action" },
                   new object[] { obj.Area_Id, obj.Area_Info, obj.Area_Details,obj.Created_By, obj.Action },
                   "@msg"
                );
        }


        public List<Tbl_Mstr_Area> BllAllArea()
        {
            List<Tbl_Mstr_Area> lstArea = new List<Tbl_Mstr_Area>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Area_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstArea = BindList<Tbl_Mstr_Area>(dt);
            }
            return lstArea;
        }

        //view//

        public AreaDetail BLLAreaDetail(int DisplayLength, int DisplayStart, int SortCol, string SortDir, string Search)
        {
            int displayLength = DisplayLength;
            int displayStart = DisplayStart;
            int sortCol = SortCol;
            string sortDir = SortDir;
            string search = Search;

            List<Tbl_Mstr_Area> listArea = new List<Tbl_Mstr_Area>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Area_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listArea = BindList<Tbl_Mstr_Area>(dt);

            }

            AreaDetail AreaDetail = new AreaDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Area] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listArea[0].TotalCount),
                aaData = listArea
            };
            return AreaDetail;
        }

        public Tbl_Mstr_Area BllAreaById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Area area = new Tbl_Mstr_Area();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Area_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                area = BindData<Tbl_Mstr_Area>(dt);

            }
            return area;
        }

        #endregion

        #region Category
        public string BLLMstrCategoryIUD(Tbl_Mstr_Category obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Category_IUD",
                   new string[] { "@Catgry_Id", "@Catgry_Name", "@Catgry_Status", "@Created_By", "@Action" },
                   new object[] { obj.Catgry_Id, obj.Catgry_Name, obj.Catgry_Status,obj.Created_By, obj.Action },
                   "@msg"
                );
        }
        //Bind the data in drop down list used by product master in category name attribute
        public List<Tbl_Mstr_Category> BllAllCategory()
        {
            List<Tbl_Mstr_Category> lstCatgry = new List<Tbl_Mstr_Category>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Category_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstCatgry = BindList<Tbl_Mstr_Category>(dt);
            }
            return lstCatgry;
        }

        public CategoryDetail BllCategoryDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Category> listCategory = new List<Tbl_Mstr_Category>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Category_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listCategory = BindList<Tbl_Mstr_Category>(dt);

            }

            CategoryDetail categoryDetail = new CategoryDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Category] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listCategory[0].TotalCount),
                aaData = listCategory
            };
            return categoryDetail;
        }
        public Tbl_Mstr_Category BllCategoryById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Category category = new Tbl_Mstr_Category();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Category_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                category = BindData<Tbl_Mstr_Category>(dt);

            }
            return category;
        }

        public Tbl_Mstr_Category casBllCategoryById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Category category = new Tbl_Mstr_Category();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Category_View", new string[] { "@Product_Id", "@Action" },
                new object[] { Id, "CascadingAllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                category = BindData<Tbl_Mstr_Category>(dt);

            }
            return category;
        }
        #endregion

        #region Product
        public string BLLMstrProductIUD(Tbl_Mstr_Product obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Product_IUD",
                   new string[] { "@Product_Id","@Product_No","@Product_Name", "@Product_Type", "@Catgry_Id", "@Product_Status", "@Created_By", "@Action" },
                   new object[] { obj.Product_Id, obj.Product_No, obj.Product_Name,obj.Product_Type ,obj.Catgry_Id, obj.Product_Status,obj.Created_By, obj.Action },
                   "@msg"
                );
        }

        public List<Tbl_Mstr_Product> BllAllProduct()
        {
            List<Tbl_Mstr_Product> lstProduct = new List<Tbl_Mstr_Product>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Product_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstProduct = BindList<Tbl_Mstr_Product>(dt);
            }
            return lstProduct;
        }

        public ProductDetail BllProductDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Product> listProduct = new List<Tbl_Mstr_Product>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Product_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listProduct = BindList<Tbl_Mstr_Product>(dt);

            }

            ProductDetail productDetail = new ProductDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Product] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listProduct[0].TotalCount),
                aaData = listProduct
            };
            return productDetail;
        }
        public Tbl_Mstr_Product BllProductById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Product product = new Tbl_Mstr_Product();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Product_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                product = BindData<Tbl_Mstr_Product>(dt);

            }
            return product;
        }

        #endregion

        #region Delivery Partner
        public string BLLMstrDeliveryPartnerIUD(Tbl_Mstr_DeliveryPartner obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_DeliveryPartner_IUD",
                   new string[] { "@Dp_Id", "@Dp_Name", "@Dp_PhNo", "@Dp_Email", "@Gender", "@Area_Id", "@Addrs", "@Join_Date", "@Created_By", "@Action" },
                   new object[] { obj.Dp_Id, obj.Dp_Name, obj.Dp_PhNo, obj.Dp_Email, obj.Gender, obj.Area_Id, obj.Addrs, SetDateFormat(obj.Join_Date),obj.Created_By, obj.Action },
                   "@msg"
                );
        }
        public Tbl_Mstr_DeliveryPartnerDetail BLLDeliveryPartnerDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_DeliveryPartner> listDeliveryPartner = new List<Tbl_Mstr_DeliveryPartner>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_DeliveryPartner_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listDeliveryPartner = BindList<Tbl_Mstr_DeliveryPartner>(dt);

            }
            Tbl_Mstr_DeliveryPartnerDetail deliveryPartner = new Tbl_Mstr_DeliveryPartnerDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_DeliveryPartner] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listDeliveryPartner[0].TotalCount),
                aaData = listDeliveryPartner
            };
            return deliveryPartner;
        }

        public Tbl_Mstr_DeliveryPartner BLLDeliveryPartnerById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_DeliveryPartner deliveryPartner = new Tbl_Mstr_DeliveryPartner();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_DeliveryPartner_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                deliveryPartner = BindData<Tbl_Mstr_DeliveryPartner>(dt);

            }
            return deliveryPartner;
        }

        #endregion

        #region Consumer
        public string BLLMstrConsumerIUD(Tbl_Mstr_Consumer obj)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Consumer_IUD",
                   new string[] { "@Cnsmr_Id", "@Cnsmr_No", "@Cnsmr_Name", "@Catgry_Id", "@Gender", "@Cnsmr_PhNo ", "@Cnsmr_Email",
                "@Area_Id", "@Addrs", "@Photo","@Id_Proof_Type","@Id_Proof_No","@Id_Proof_Doc","@Status","@Cnsmr_Date","@Created_By","@Action" },
                   new object[] { obj.Cnsmr_Id,obj.Cnsmr_No, obj.Cnsmr_Name,
                obj.Catgry_Id, obj.Gender,obj.Cnsmr_PhNo, obj.Cnsmr_Email, obj.Area_Id, obj.Addrs,obj.Photo,obj.Id_Proof_Type,
                obj.Id_Proof_No,obj.Id_Proof_Doc,obj.Status_,SetDateFormat(obj.Cnsmr_Date),
              obj.Created_By, obj.Action },
                   "@msg"
                );



        }
        public string BLLSaveImage1(string hdnimage, string hdnext, string Filepath)
        {
            string ImgName = string.Empty;

            string filePath = string.Empty;
            if (hdnimage.Length > 0)
            {
                byte[] imageBytes = Convert.FromBase64String(hdnimage.ToString().Split(',')[1].ToString());
                MemoryStream ms = new MemoryStream();//imageBytes, 0, imageBytes.Length);
                ms.Read(imageBytes, 0, imageBytes.Length);
                ms.Write(imageBytes, 0, imageBytes.Length);
                System.Drawing.Image objImage = System.Drawing.Image.FromStream(ms, true);
                // System.Drawing.Image SaveImage = ScaleImageClassified(objImage, 600);

                ImgName = DateTime.Now.ToString("ddMMyyyhhmmssffff") + "." + hdnext;
                //filePath = "GobellWebservices/Documents/Classifieds/" + ImgName;
                filePath = Filepath + ImgName;
                objImage.Save(HttpContext.Current.Server.MapPath("~/" + filePath));
            }
            return ImgName;
        }



        public string BLLSaveFile(string hdnFile, string hdnext, string Filepath)
        {
            string fileName = string.Empty;

            if (hdnFile.Length > 0)
            {
                byte[] fileBytes = Convert.FromBase64String(hdnFile.Split(',')[1].ToString());

                // Generate a unique file name using timestamp
                fileName = DateTime.Now.ToString("ddMMyyyhhmmssffff") + "." + hdnext;

                // Combine the file path and name
                string filePath = Path.Combine(Filepath, fileName);

                // Save the file to the server
                File.WriteAllBytes(HttpContext.Current.Server.MapPath("~/" + filePath), fileBytes);
            }

            return fileName;
        }


        public Tbl_Mstr_ConsumerDetail BLLConsumerDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Consumer> listConsumer = new List<Tbl_Mstr_Consumer>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Consumer_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listConsumer = BindList<Tbl_Mstr_Consumer>(dt);

            }
            Tbl_Mstr_ConsumerDetail Consumer = new Tbl_Mstr_ConsumerDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Consumer] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listConsumer[0].TotalCount),
                aaData = listConsumer
            };
            return Consumer;
        }

        public Tbl_Mstr_Consumer BLLConsumerById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Consumer Consumer = new Tbl_Mstr_Consumer();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Consumer_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Consumer = BindData<Tbl_Mstr_Consumer>(dt);

            }
            return Consumer;
        }


        #endregion

        #region StockIUD & StockDetailsview
        public string BLLMstrStockIUD(Tbl_Mstr_Stock obj,DataTable dt)
        {
            return new Dll().ExecuteProcedure
                (
                   "USP_Tbl_Mstr_Stock_IUD",
                   new string[] 
                   { 
                       "@Stock_Id", "@Stock_No","@Product_Id", "@Suplr_Id", "@Catgry_Id", "@Unit_Type", "@Quantity", "@Stock_Price", "@Selling_Price","@DT_DT_Stock",
                         "@Entry_Date","@_Expiry_Date", "@Created_By", "@Action" 
                   },
                   new object[] 
                   {
                       obj.Stock_Id,obj.Stock_No, obj.Product_Id, obj.Suplr_Id, obj.Catgry_Id,
                       obj.Unit_Type, obj.Quantity, obj.Stock_Price, obj.Selling_Price,dt,SetDateFormat(obj.Entry_Date),SetDateFormat(obj._Expiry_Date),
                       obj.Created_By, obj.Action 
                   },
                   "@msg"
              );
        }
        public StockpriceDetail BLLStockDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Stock_DTL> Liststock = new List<Tbl_Mstr_Stock_DTL>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_DTL_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Liststock = BindList<Tbl_Mstr_Stock_DTL>(dt);
            }
            StockpriceDetail stock = new StockpriceDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Stock_DTL] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : Liststock[0].TotalCount),
                aaData = Liststock
            };
            return stock;
        }

        public Tbl_Mstr_Stock_DTL BLLStockPriceById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Stock_DTL Stock = new Tbl_Mstr_Stock_DTL();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_DTL_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Stock = BindData<Tbl_Mstr_Stock_DTL>(dt);

            }
            return Stock;
        }

        public List<DT_DT_Stock> BLLStockPriceDTById(string Id)
        {
            DataTable dt = new DataTable();
            List<DT_DT_Stock> Stock = new List<DT_DT_Stock>();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_DTL_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Stock = BindList<DT_DT_Stock>(dt);

            }
            return Stock;
        }

        //public List<DT_DT_Stock> BLLStockDetails(int stockId)
        //{
        //    // Code for retrieving detailed stock information based on Stock_Id
        //    DataTable dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_View", new string[] { "@Stock_Id", "@Action" },
        //        new object[] { stockId, "GroupDetail" });

        //    List<DT_DT_Stock> stockDetailsList = new List<DT_DT_Stock>();

        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        stockDetailsList = BindList<DT_DT_Stock>(dt);
        //    }


        //    return stockDetailsList;
        //}


        public List<Tbl_Mstr_Product> BllAllStock()
        {
            List<Tbl_Mstr_Product> lstProduct = new List<Tbl_Mstr_Product>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_DTL_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllByproduct" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstProduct = BindList<Tbl_Mstr_Product>(dt);
            }
            return lstProduct;
        }
        #endregion

        #region Stockprice

        public Tbl_Mstr_StockDetail BLLStock(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Mstr_Stock> Liststock = new List<Tbl_Mstr_Stock>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Liststock = BindList<Tbl_Mstr_Stock>(dt);

            }
            Tbl_Mstr_StockDetail stock = new Tbl_Mstr_StockDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Mstr_Stock] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : Liststock[0].TotalCount),
                aaData = Liststock
            };
            return stock;
        }
        public List<DT_DT_Stock> BLLStockDetails(int stockId)
        {
            // Code for retrieving detailed stock information based on Stock_Id
            DataTable dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_View", new string[] { "@Stock_Id", "@Action" },
                new object[] { stockId, "GroupDetail" });

            List<DT_DT_Stock> stockDetailsList = new List<DT_DT_Stock>();

            if (dt != null && dt.Rows.Count > 0)
            {
                stockDetailsList = BindList<DT_DT_Stock>(dt);
            }


            return stockDetailsList;
        }
        public Tbl_Mstr_Stock BLLStockById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Stock Stock = new Tbl_Mstr_Stock();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_View", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                Stock = BindData<Tbl_Mstr_Stock>(dt);

            }
            return Stock;
        }

        #endregion

        #endregion

        #region TransactionSales
        public string BLLTransSalesIUD(Tbl_Trns_Sales obj, DataTable dt)
        {
            return new Dll().ExecuteProcedure
            (
                  "USP_Tbl_Trns_Sales_IUD",
                   new string[]
                   {
                       "@Sales_Id", "@Invoice_No","@Booking_Date", "@Approximate_Delivery_Date",
                       "@Consumer_No","@Consumer_Id", "@Phone_No","@Catgry_Name","@Quantity","@Product_Id",
                       "@Area_Info","@DT_DT_Product","@Total_Amount","@Delivery_Partner_Id", "@Payment_Status","@Intial_Payment", "@Intial_Payment_Mode",
                       "@D_Flag", "@Created_By","@Action"
                   },
                   new object[]
                   {
                       obj.Sales_Id,obj.Invoice_No,SetDateFormat(obj.Booking_Date), SetDateFormat(obj.Approximate_Delivery_Date), obj.Consumer_No, obj.Consumer_Id,
                       obj.Phone_No, obj.Catgry_Name,obj.Quantity,obj.Product_Id,obj.Area_Info,dt, obj.Total_Amount, obj.Delivery_Partner_Id, obj.Payment_Status,
                       obj.Intial_Payment, obj.Intial_Payment_Mode, obj.D_Flag,obj.Created_By, obj.Action
                   },
                   "@msg"
         );
        }

        //public List<Tbl_Trns_Sales> BllAllSales()
        //{
        //    List<Tbl_Trns_Sales> lstSales = new List<Tbl_Trns_Sales>();
        //    DataTable dt = new DataTable();
        //    dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_IUD", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        lstSales = BindList<Tbl_Trns_Sales>(dt);
        //    }
        //    return lstSales;
        //}

        public SalesDetail BllSalesDetail(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Trns_Sales> listSales = new List<Tbl_Trns_Sales>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                listSales = BindList<Tbl_Trns_Sales>(dt);
            }

            SalesDetail salesDetail = new SalesDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Trns_Sales] where D_Flag=0")),
               // iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : listSales[0].TotalCount),
                aaData = listSales
            };
            return salesDetail;
        }
        public Tbl_Trns_Sales BllSalesById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Trns_Sales sales = new Tbl_Trns_Sales();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_IUD", new string[] { "@Id", "@Action" },
                new object[] { Id, "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                sales = BindData<Tbl_Trns_Sales>(dt);
            }
            return sales;
        }
        public Tbl_Trns_Sales casBllSalesById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Trns_Sales sales = new Tbl_Trns_Sales();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_View", new string[] { "@Sales_Id", "@Action" },
                new object[] { Id, "CascadingAllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                sales = BindData<Tbl_Trns_Sales>(dt);

            }
            return sales;
        }
        public Tbl_Mstr_Consumer casBllConsumerById(string Id)
        {
           
                DataTable dt = new DataTable();
                Tbl_Mstr_Consumer consumer = new Tbl_Mstr_Consumer();
                dt = new Dll().FillDataTable("USP_Tbl_Mstr_Consumer_View", new string[] { "@Cnsmr_Id", "@Action" },
                    new object[] { Id, "CascadingAllById" });
                if (dt != null && dt.Rows.Count > 0)
                {
                    consumer = BindData<Tbl_Mstr_Consumer>(dt);

                }
                else
                {
                    Console.WriteLine("No data returned from the stored procedure.");
                }
                return consumer;
           

        }
        public Tbl_Mstr_Stock casBllStockById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Mstr_Stock stock = new Tbl_Mstr_Stock();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Stock_View", new string[] { "@Product_Id", "@Action" },
                new object[] { Id, "CascadingAllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                stock = BindData<Tbl_Mstr_Stock>(dt);
            }
            else
            {
                Console.WriteLine("No data returned from the stored procedure.");
            }
            return stock;
        }
        public List<Tbl_Mstr_Consumer> BllAllConsumer()
        {
            List<Tbl_Mstr_Consumer> lstsales = new List<Tbl_Mstr_Consumer>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_Consumer_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstsales = BindList<Tbl_Mstr_Consumer>(dt);
            }
            return lstsales;
        }
        public List<Tbl_Mstr_DeliveryPartner> BllAllDeliveryPartner()
        {
            List<Tbl_Mstr_DeliveryPartner> lstDeliveryPartner = new List<Tbl_Mstr_DeliveryPartner>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_DeliveryPartner_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstDeliveryPartner = BindList<Tbl_Mstr_DeliveryPartner>(dt);
            }
            return lstDeliveryPartner;
        }
        //data fetch according to area name//

        public List<Tbl_Mstr_DeliveryPartner> casBllDeliveryById(string Id)
        {
            DataTable dt = new DataTable();
            List<Tbl_Mstr_DeliveryPartner> delivery = new List<Tbl_Mstr_DeliveryPartner>();
            dt = new Dll().FillDataTable("USP_Tbl_Mstr_DeliveryPartner_View", new string[] { "@Area", "@Action" },
                new object[] { Id, "CascadingAllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                delivery = BindList<Tbl_Mstr_DeliveryPartner>(dt);

            }
            return delivery;
        }
        #endregion

        #region  FinalSettlement
     
        public string BLLTransFinalSettlementIUD(Tbl_Trns_Final_Settlement obj, DataTable dt)
        {
            return new Dll().ExecuteProcedure
            (
                "USP_Tbl_Trns_Final_Settlement_IUD",
                new string[]
                {
              "@Delivery_Id","@Delivery_Status", "@Invoice_No", "@Consumer_Name", "@Phone_No",
              "@Area_Info", "@Catgry_Name", "@Quantity", "@MRP", "@Discount", "@Rate", "@Amount", "@Product_Id",
              "@Actual_Cost", "@Delivery_Name", "@Payment_Status", "@Intial_Payment", "@Intial_Payment_Mode",
             "@Total_Amount", "@Final_Payment", "@Final_Payment_Mode", "@DT_DT_FinalSettlement", "@D_Flag", "@Created_By",
               "@Action"
                },
                new object[]
                {
       obj.Delivery_Id,obj.Delivery_Status, obj.Invoice_No, obj.Consumer_Name, obj.Phone_No,
       obj.Area_Info, obj.Catgry_Name, obj.Quantity, obj.MRP, obj.Discount, obj.Rate, obj.Amount, obj.Product_Id,
       obj.Actual_Cost, obj.Dp_Name, obj.Payment_Status, obj.Intial_Payment, obj.Intial_Payment_Mode,
          obj.Total_Amount,obj.Final_Payment, obj.Final_Payment_Mode, dt, obj.D_Flag, obj.Created_By, obj.Action
                },
                "@msg" 
            );
        }
     
        public List<Tbl_Trns_Sales> BllAllSales()
        {
            List<Tbl_Trns_Sales> lstsales = new List<Tbl_Trns_Sales>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllInvoice" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstsales = BindList<Tbl_Trns_Sales>(dt);
            }
            return lstsales;
        }
        //public Tbl_Trns_Sales casBllFinalSettlementById(string Id)
        //{
        //    DataTable dt = new DataTable();
        //    Tbl_Trns_Sales sales = new Tbl_Trns_Sales();
        //    dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_View", new string[] { "@Sales_Id", "@Action" },
        //        new object[] { Id, "CascadingAllById" });
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        sales = BindData<Tbl_Trns_Sales>(dt);
        //    }
        //    else
        //    {
        //        Console.WriteLine("No data returned from the stored procedure.");
        //    }
        //    return sales;
        //}
        public Tbl_Trns_Sales casBllFinalSettlementById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Trns_Sales sales = new Tbl_Trns_Sales();

            dt = new Dll().FillDataTable("USP_Tbl_Trns_Sales_View", new string[] { "@Sales_Id", "@Action" },
                new object[] { Id, "CascadingAllById" });

            if (dt != null && dt.Rows.Count > 0)
            {
                // Assuming BindData<T> is a method that maps DataRow to object
                sales = BindData<Tbl_Trns_Sales>(dt);

                // Populate DT_Product separately based on the Sales_Id
                sales.DT_Product = BindDataInternal<DT_DT_Product>(dt);
            }
            else
            {
                Console.WriteLine("No data returned from the stored procedure.");
            }

            return sales;
        }

        private List<DT_DT_Product> BindDataInternal<DT_DT_Product>(DataTable dt)
        {
            List<DT_DT_Product> list = new List<DT_DT_Product>();

            // Assuming DT_DT_Product is a reference type
            if (dt != null && dt.Rows.Count > 0)
            {
                // Assuming DT_DT_Product has a parameterless constructor
                var properties = typeof(DT_DT_Product).GetProperties();

                foreach (DataRow row in dt.Rows)
                {
                    DT_DT_Product obj = Activator.CreateInstance<DT_DT_Product>();

                    foreach (var property in properties)
                    {
                        if (dt.Columns.Contains(property.Name) && row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(obj, row[property.Name]);
                        }
                    }

                    list.Add(obj);
                }
            }

            return list;
        }



        #endregion

        #region  Return Sales
        public string BLLTransReturnSalesIUD(Tbl_Trns_Return_Sales obj, DataTable dt)
        {
            return new Dll().ExecuteProcedure
            (
                
                "USP_Tbl_Trns_Return_Sales_IUD",
                new string[]
                {
                        "@Delivery_Id","@Delivery_Status", "@Invoice_No", "@Consumer_Name", "@Phone_No",
                      "@Area_Info",  "@Quantity", "@MRP", "@Discount", "@Rate", "@Amount", "@Product_Id",
                      "@Actual_Cost", "@Delivery_Name", "@Intial_Payment", "@Intial_Payment_Mode",
                        "@Total_Amount", "@Final_Payment", "@Final_Payment_Mode", "@DT_DT_ReturnSales", "@D_Flag", "@Created_By",
                        "@Action"
                },
                new object[]
                {
                         obj.Delivery_Id,obj.Delivery_Status, obj.Invoice_No, obj.Consumer_Name, obj.Phone_No,
                          obj.Area_Info,  obj.Quantity, obj.MRP, obj.Discount, obj.Rate, obj.Amount, obj.Product_Id,
                          obj.Actual_Cost, obj.Dp_Name,  obj.Intial_Payment, obj.Intial_Payment_Mode,
                         obj.Total_Amount,obj.Final_Payment, obj.Final_Payment_Mode, dt, obj.D_Flag, obj.Created_By, obj.Action
                },
                "@msg"
            );
        }
        public List<Tbl_Trns_Final_Settlement> BllAllReturn()
        {
            List<Tbl_Trns_Final_Settlement> lstsales = new List<Tbl_Trns_Final_Settlement>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Return_Sales_View", new string[] { "@Id", "@Action" }, new object[] { "0", "AllById" });
            if (dt != null && dt.Rows.Count > 0)
            {
                lstsales = BindList<Tbl_Trns_Final_Settlement>(dt);
            }
            return lstsales;
        }

        public Tbl_Trns_Final_Settlement casBllReturnsalesById(string Id)
        {
            DataTable dt = new DataTable();
            Tbl_Trns_Final_Settlement sales = new Tbl_Trns_Final_Settlement();

            dt = new Dll().FillDataTable("USP_Tbl_Trns_Return_Sales_View", new string[] { "@Delivery_Id", "@Action" },
                new object[] { Id, "CascadingAllById" });

            if (dt != null && dt.Rows.Count > 0)
            {
                // Assuming BindData<T> is a method that maps DataRow to object
                sales = BindData<Tbl_Trns_Final_Settlement>(dt);

                // Populate DT_Product separately based on the Sales_Id
                sales.DT_FinalSettlement = BindDataReturnInternal<DT_DT_FinalSettlement>(dt);
            }
            else
            {
                Console.WriteLine("No data returned from the stored procedure.");
            }

            return sales;
        }

        private List<DT_DT_FinalSettlement> BindDataReturnInternal<DT_DT_FinalSettlement>(DataTable dt)
        {
            List<DT_DT_FinalSettlement> list = new List<DT_DT_FinalSettlement>();

            // Assuming DT_DT_Product is a reference type
            if (dt != null && dt.Rows.Count > 0)
            {
                // Assuming DT_DT_Product has a parameterless constructor
                var properties = typeof(DT_DT_FinalSettlement).GetProperties();

                foreach (DataRow row in dt.Rows)
                {
                    DT_DT_FinalSettlement obj = Activator.CreateInstance<DT_DT_FinalSettlement>();

                    foreach (var property in properties)
                    {
                        if (dt.Columns.Contains(property.Name) && row[property.Name] != DBNull.Value)
                        {
                            property.SetValue(obj, row[property.Name]);
                        }
                    }

                    list.Add(obj);
                }
            }

            return list;
        }
        #endregion

        #region SalesStatus

        public Tbl_Trns_Final_SettlementDetail BLLFinalSettlement(int iDisplayLength, int iDisplayStart, int iSortCol_0, string sSortDir_0, string sSearch)
        {
            int displayLength = iDisplayLength;
            int displayStart = iDisplayStart;
            int sortCol = iSortCol_0;
            string sortDir = sSortDir_0;
            string search = sSearch;

            List<Tbl_Trns_Final_Settlement> ListFinalSettlement = new List<Tbl_Trns_Final_Settlement>();
            DataTable dt = new DataTable();
            dt = new Dll().FillDataTable("USP_Tbl_Trns_Return_Sales_View", new string[] { "@DisplayLength", "@DisplayStart", "@SortCol", "@SortDir", "@Search", "@Action" },
                new object[] { displayLength, displayStart, sortCol, sortDir, search, "All" });
            if (dt != null && dt.Rows.Count > 0)
            {
                ListFinalSettlement = BindList<Tbl_Trns_Final_Settlement>(dt);
            }

            Tbl_Trns_Final_SettlementDetail finalSettlement = new Tbl_Trns_Final_SettlementDetail()
            {
                iTotalRecords = Convert.ToInt32(new Dll().ExecuteScalar("select count(*) from [dbo].[Tbl_Trns_Final_Settlement] where D_Flag=0")),
                iTotalDisplayRecords = (dt.Rows.Count == 0 || dt == null ? 0 : ListFinalSettlement[0].TotalCount),
                aaData = ListFinalSettlement
            };
            return finalSettlement;
        }

        #endregion
    }
}