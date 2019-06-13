using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;

public partial class LoadingControl : System.Web.UI.UserControl
{
    CommonClass ccObj = new CommonClass();
    UserClass ucObj = new UserClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        { 
            this.labValid.Text = ccObj.RandomNum(4);//generate random verification code
            if (Session["UserID"] != null)
            {
                //check if login
                this.tabLoad.Visible = true;  //show welcome board
                this.tabLoading.Visible =false ; //hide board
            }        
        }
    }
   
    protected void lnkbtnOut_Click(object sender, EventArgs e)
    {
        //empty Session
        Session["UserID"] = null;
        Session["UserName"] = null;
        this.tabLoad.Visible = false; 
        this.tabLoading.Visible = true; 
        Response.Write(ccObj.MessageBox("Thank you for purchasing！","Default.aspx"));
    }
    protected void btnLoad_Click(object sender, ImageClickEventArgs e)
    {
       
        Session["UserID"] = null;
        Session["UsertjID"] = null;
        Session["Username"] = null;
        if (this.txtName.Text.Trim() == "" || this.txtPassword.Text.Trim() == "")
        {
            Response.Write(ccObj.MessageBoxPage("Username and password cannot be empty！"));
        }
        else
        {
            if (this.txtValid.Text.Trim() == this.labValid.Text.Trim())
            {
            
                DataTable dsTable = ucObj.UserLogin(this.txtName.Text.Trim(), this.txtPassword.Text.Trim());
                if (dsTable!=null) 
                {
                    Session["UserID"] = Convert.ToInt32(dsTable.Rows[0][0].ToString()); //save user ID
                    Session["UsertjID"] = Convert.ToInt32(dsTable.Rows[0][2].ToString()); 
                    Session["Username"] = dsTable.Rows[0][1].ToString(); //save user name
                    //Response.Redirect(Request.CurrentExecutionFilePath); 
                    Response.Redirect("Default.aspx");
                }
                else
                {
                    Response.Write(ccObj.MessageBoxPage("Your user name or password is incorrect！"));
                }
            }
            else
            {
                Response.Write(ccObj.MessageBoxPage("Please enter the correct verification code！"));
            }
        }
    }
    protected void btnRegister_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("Register.aspx");
    }
}
