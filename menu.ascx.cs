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

public partial class userControl_menu : System.Web.UI.UserControl
{
    CommonClass ccObj = new CommonClass();
    protected void Page_Load(object sender, EventArgs e)
    {
       

    }
    protected void lnkbtnfeedback_Click(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Write(ccObj.MessageBox("Please login！", "Default.aspx"));
        }
        else
        {
            Response.Redirect("feedback.aspx");

        }
    }
    protected void lnkbtnMyWord_Click(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Write(ccObj.MessageBox("Please login！", "Default.aspx"));
        }
        else
        {
            Response.Redirect("MyWord.aspx");

        }
    }
    protected void lnkbtnOut_Click(object sender, EventArgs e)
    {
        if (Session["UserName"] != null)
        {
            Session["UserID"] = null; //use ID
            Session["UsertjID"] = null; 
            Session["Username"] = null;//user name
            Response.Write(ccObj.MessageBox("Thank you for ordering！", "Default.aspx"));
        }
    }
}
