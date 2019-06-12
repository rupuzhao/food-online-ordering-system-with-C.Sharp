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

public partial class PayWay : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*check if login*/
        ST_check_Login();
    }
    public void ST_check_Login()
    {

        if ((Session["Username"] == null))
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
    }
  
    protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GoBank.aspx?OrderID=" + Request["OrderID"].ToString());
    }
    protected void ImageButton2_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GoBank.aspx?OrderID=" + Request["OrderID"].ToString());
    }
    protected void ImageButton3_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GoBank.aspx?OrderID=" + Request["OrderID"].ToString());
    }
    protected void ImageButton4_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GoBank.aspx?OrderID=" + Request["OrderID"].ToString());
    }
    protected void ImageButton5_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("GoBank.aspx?OrderID=" + Request["OrderID"].ToString());
    }
}
