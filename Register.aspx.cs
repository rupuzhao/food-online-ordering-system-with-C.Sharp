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

public partial class Register : System.Web.UI.Page
{
    DBClass dbObj = new DBClass();
    
    CommonClass ccObj = new CommonClass();
    UserClass ucObj = new UserClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        ddlClassBind(); //bind category
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
    protected void btnSave_Click(object sender, EventArgs e)
    {
        //check required information
        if (this.txtPostCode.Text.Trim() == "" && this.txtPhone.Text.Trim()=="" && this.txtEmail.Text.Trim() == "")
        {
            Response.Write(ccObj.MessageBoxPage("Please enter required information！"));
        }
        else
        {
      
            int IntReturnValue=ucObj.AddUser(txtName.Text.Trim(),txtPassword.Text.Trim(),txtTrueName.Text.Trim(), transfer(this.ddlSex.SelectedItem.Text),txtPhone.Text.Trim(),txtEmail.Text.Trim(),txtAddress.Text.Trim(), txtPostCode.Text.Trim(),Convert.ToInt32(this.ddlCategory.SelectedValue.ToString()));
            if (IntReturnValue == 100)
            {
                Response.Write(ccObj.MessageBox("Congratulation， you are the member！", "Default.aspx"));
            }
            else
            {
                Response.Write(ccObj.MessageBox("Sorry, user name existed！"));
            
            }
           
        }

    }
    
    protected bool transfer(string strValue)
    {
        if (strValue== "male")
        {
            return true;
        }
        else
        {
            return false;
        
        }
    }
    protected void btnReset_Click(object sender, EventArgs e)
    {
        this.txtName.Text = "";     //use name
        this.txtPassword.Text = ""; //password
        this.txtTrueName.Text = ""; //nick name
        this.txtPhone.Text = "";    //phone
        this.txtPostCode.Text = ""; //zip code
        this.txtEmail.Text = "";    //Email
        this.txtAddress.Text = "";  //address
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
