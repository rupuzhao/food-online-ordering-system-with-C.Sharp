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
using System.Text.RegularExpressions;

public partial class UpdateMember : System.Web.UI.Page
{
    DBClass dbObj = new DBClass();
    CommonClass ccObj = new CommonClass();
    UserClass ucObj = new UserClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
     
            if (Session["UserID"] == null)
            {
                Response.Redirect("Default.aspx");
            }
            else
            {
                DataTable dsTable = ucObj.GetUserInfo(Convert.ToInt32(Session["UserID"].ToString()));
                this.txtName.Text = dsTable.Rows[0]["UserName"].ToString();    //user name
                this.txtPassword.Text = dsTable.Rows[0]["Password"].ToString();//password
                this.txtTrueName.Text = dsTable.Rows[0]["RealName"].ToString();//nick name
                this.ddlSex.SelectedIndex = Convert.ToInt32(dsTable.Rows[0]["Sex"]);//gender
                this.txtPhone.Text = dsTable.Rows[0]["Phonecode"].ToString();  //phone
                this.txtEmail.Text = dsTable.Rows[0]["Email"].ToString();      //Email
                this.txtAddress.Text = dsTable.Rows[0]["Address"].ToString();  //address
                this.txtPostCode.Text = dsTable.Rows[0]["PostCode"].ToString(); //WeChat
                this.ddlCategory.SelectedValue = dsTable.Rows[0]["ClassID"].ToString();
                ddlClassBind();
            }
        }     
    }






    public void ddlClassBind()
    {
        string strSql = "select * from tb_Class";
        DataTable dsTable = dbObj.GetDataSetStr(strSql, "tbClass");
    
        this.ddlCategory.DataSource = dsTable.DefaultView;
   
        this.ddlCategory.DataTextField = dsTable.Columns[1].ToString();
  
        this.ddlCategory.DataValueField = dsTable.Columns[0].ToString();
        this.ddlCategory.DataBind();
    }
    protected void btnUpdate_Click(object sender, EventArgs e)
    {

        if (this.txtName.Text.Trim() == "" && this.txtPassword.Text.Trim() == "" && this.txtTrueName.Text.Trim() == "" && this.txtPhone.Text.Trim() == "" && this.txtEmail.Text.Trim() == "" && this.txtAddress.Text.Trim() == "" && this.txtPostCode.Text.Trim() == "")
        {
            Response.Write(ccObj.MessageBoxPage("please enter required information！"));
        }
        else
        {
            if (IsValidPostCode(txtPostCode.Text.Trim()) == false)
            {
                Response.Write(ccObj.MessageBoxPage("WeChat is incorrect！"));
                return;
            }
            else if (IsValidPhone(txtPhone.Text.Trim()) == false)
            {
                Response.Write(ccObj.MessageBoxPage("Phone number is incorrect"));
                return;
            }
            else if (IsValidEmail(txtEmail.Text.Trim()) == false)
            {
                Response.Write(ccObj.MessageBoxPage("E-mail format is incorrect！"));
                return;
            }
            else
            {
                ucObj.MedifyUser(txtName.Text.Trim(), txtPassword.Text.Trim(), txtTrueName.Text.Trim(), transfer(ddlSex.SelectedItem.Text.Trim()),txtPhone.Text.Trim(), txtEmail.Text.Trim(), txtAddress.Text.Trim(), txtPostCode.Text.Trim(),Convert.ToInt32(this.ddlCategory.SelectedValue.ToString()), Convert.ToInt32(Session["UserID"].ToString()));
                Session["Username"] = "";
                Session["Username"] = txtName.Text.Trim();
                Response.Write(ccObj.MessageBox("Modify Succeed！", "Default.aspx"));
            }
        }
    }
    
    protected bool transfer(string strValue)
    {
        if (strValue == "male")
        {
            return true;
        }
        else
        {
            return false;

        }
    }
    
    public bool IsValidPostCode( string num)
    {
      return Regex.IsMatch(num, @"\d{6}");

    }
    public bool IsValidPhone( string num)
    {
      return Regex.IsMatch(num, @"(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$");
    }
    public bool IsValidEmail( string num)
    {
      return Regex.IsMatch(num, @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        this.txtName.Text = "";    //user name
        this.txtPassword.Text = "";//password
        this.txtTrueName.Text = "";//nick name
        this.txtPhone.Text = "";   //phone
        this.txtPostCode.Text = "";//WeChat
        this.txtEmail.Text = "";   //Email
        this.txtAddress.Text = ""; //address
    }
}
