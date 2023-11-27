using LoginDemo.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.WebPages;

namespace LoginDemo.Services
{
    public class Repository 
    {
        string connectionstring = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public async Task<ResponseModel<string>> login(LoginViewModel user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
               // string md5_password = md5_string(user.Password);

                using (SqlConnection conn = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = string.Format("Select * FROM tbl_Users WHERE Email = '{0}' and Password='{1}'", user.Email,user.Password);
                    cmd.Connection = conn;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    await Task.Run(()=> da.Fill(dt));

                    if (dt.Rows.Count > 0)
                    {
                        response.Data = JsonConvert.SerializeObject(dt);
                        response.resultCode = 200;
                    }
                    else
                    {
                        response.message = "User Not Found!";
                        response.resultCode = 500;
                    }

                   
                }
            }
            return response;
        }

        public async Task<ResponseModel<string>> Register(User user)
        {
            ResponseModel<string> response = new ResponseModel<string>();

            if (user != null)
            {
                //if (user.FileUpload != null)
                //{
                //    string strpath = Path.GetExtension(user.FileUpload.);
                //    if (strpath != ".jpg" && strpath != ".jpeg" && strpath != ".gif" && strpath != ".png")
                //    {
                //        response.message = "Not Allowed!";

                //    }
                //    else
                //    {
                //        response.message = "Saved!";

                //    }
                //    string fileimg = Path.GetFileName(user.FileUpload.FileName);

                //}
                using (SqlConnection conn = new SqlConnection(connectionstring))
                    {

                        string mainconn = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
                        SqlConnection sqlconn = new SqlConnection(mainconn);
                        string sqlquery = "insert into tbl_Users(UserName,Email,Password,FileUpload,DateField) values(@UserName,@Email,@Password,@FileUpload,@DateField)";

                        SqlCommand sqlcomm = new SqlCommand(sqlquery, sqlconn);
                        sqlconn.Open();
                        sqlcomm.Parameters.AddWithValue("@UserName", user.UserName);
                        sqlcomm.Parameters.AddWithValue("@Email", user.Email);
                        sqlcomm.Parameters.AddWithValue("@Password", user.Password);
                        //sqlcomm.Parameters.AddWithValue("@FileUpload", "~/Files /" + fileimg);

                    sqlcomm.Parameters.AddWithValue("@FileUpload", user.FileUpload);
                    sqlcomm.Parameters.AddWithValue("@DateField", user.DateField);

                        //string filename = null;
                        //if (user.FileUpload != null)
                        //{
                        //     filename = Path.GetFileName(user.FileUpload);
                        //   // string filepath = Path.Combine(ImageMapEventArgs("~/Files/"), user.FileUpload);
                        //}
                        sqlcomm.ExecuteNonQuery();
                        sqlconn.Close();
                        response.message = "User has been registered!";
                        response.resultCode = 200;

                        //
                        //SqlCommand cmd = new SqlCommand();
                        //cmd.CommandText = string.Format("Insert INTO tbl_Users(UserName,Email,Password,FileUpload,DateField) VALUES" +
                        //    "('{0}','{1}','{2}','{3}','{4}')",
                        //    user.UserName, user.Email, user.Password, user.FileUpload,DateTime.Now);

                        ////cmd.CommandText = string.Format("Insert INTO tbl_Users(UserName,Email,Password,FileUpload,DateField) VALUES" +
                        ////  "('{0}','{1}','{2}','{3}','{4}')",
                        ////  user.UserName, user.Email, user.Password, user.FileUpload, user.DateField);
                        //cmd.Connection = conn;

                        //conn.Open();
                        //var result = await cmd.ExecuteNonQueryAsync();
                        //conn.Close();//
                        //if (result == 1) //row changes in the database - successfull
                        //{
                        //    response.message = "User has been registered!";
                        //    response.resultCode = 200;
                        //}
                        //else
                        //{
                        //    response.message = "Unable to register User!";
                        //    response.resultCode = 500;
                        //}

                    }
                }
            
            return response;
        }
       
    }
}