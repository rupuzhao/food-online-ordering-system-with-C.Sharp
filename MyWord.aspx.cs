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

public partial class MyWord : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    DBClass dbObj = new DBClass();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["UserName"] == null)
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
        this.dlBind();//show comments
    }
    public void dlBind()
    {
        int curpage = Convert.ToInt32(labNowPage.Text); //current page
        PagedDataSource ps = new PagedDataSource(); //define PagedDataSource object
      
        string strSql = "SELECT * FROM tb_LeaveWord WHERE Uid='" + Session["UserName"].ToString() + "'";
        SqlCommand myCmd = dbObj.GetCommandStr(strSql);
        DataTable dsTable = dbObj.GetDataSet(myCmd, "tbLeaveWord");
        ps.DataSource =dsTable.DefaultView;
        ps.AllowPaging = true; 
        ps.PageSize = 10; 
        ps.CurrentPageIndex = curpage - 1; 
        lnkbtnPrve.Enabled = true;
        lnkbtnTop.Enabled = true;
        lnkbtnNext.Enabled = true;
        lnkbtnLast.Enabled = true;
        if (curpage == 1)
        {
            lnkbtnTop.Enabled = false;
            lnkbtnPrve.Enabled = false;
        }
        if (curpage == ps.PageCount)
        {
            lnkbtnNext.Enabled = false;
            lnkbtnLast.Enabled = false;

        }
        this.labCount.Text = Convert.ToString(ps.PageCount);//total page
       
        this.dlMyWord.DataSource = ps;
        this.dlMyWord.DataKeyField = "ID";
        this.dlMyWord.DataBind();
    }

    protected void lnkbtnTop_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = "1";
        this.dlBind();
    }
    protected void lnkbtnPrve_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = Convert.ToString(Convert.ToInt32(this.labNowPage.Text) - 1);
        this.dlBind();
    }
    protected void lnkbtnNext_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = Convert.ToString(Convert.ToInt32(this.labNowPage.Text) + 1);
        this.dlBind();
    }
    protected void lnkbtnLast_Click(object sender, EventArgs e)
    {
        this.labNowPage.Text = this.labCount.Text;
        this.dlBind();
    }
   
    protected void dlMyWord_DeleteCommand(object source, DataListCommandEventArgs e)
    {
        string strSql = this.dlMyWord.DataKeys[e.Item.ItemIndex].ToString(); 
        string sqlDelSql = "Delete from tb_LeaveWord where ID='" + Convert.ToInt32(strSql) + "'";
        SqlCommand myCmd = dbObj.GetCommandStr(sqlDelSql);
        dbObj.ExecNonQuery(myCmd);
        Page.Response.Redirect("MyWord.aspx");

    }
}
