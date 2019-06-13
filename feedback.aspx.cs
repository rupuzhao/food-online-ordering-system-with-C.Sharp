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

public partial class feedback : System.Web.UI.Page
{
    CommonClass ccObj = new CommonClass();
    DBClass dbObj = new DBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserName"] == null)
            {
                Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
                Response.End();

            }
            this.dlBind();

        }
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
   
    public void dlBind()
    {
        int curpage = Convert.ToInt32(labNowPage.Text); 
        PagedDataSource ps = new PagedDataSource(); 
      
        //string strSql = "SELECT * FROM tb_LeaveWord WHERE Uid='" + Session["UserName"].ToString() + "'";
        string strSql = "SELECT * FROM tb_LeaveWord ";
        SqlCommand myCmd = dbObj.GetCommandStr(strSql);
        DataTable dsTable = dbObj.GetDataSet(myCmd, "tbLeaveWord");
        ps.DataSource = dsTable.DefaultView;
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
        this.labCount.Text = Convert.ToString(ps.PageCount);//total pages
        //bind DataList
        this.dlMyWord.DataSource = ps;
        this.dlMyWord.DataKeyField = "ID";
        this.dlMyWord.DataBind();
    }



    protected void btnOK_Click(object sender, EventArgs e)
    {
        if (Session["UserName"] == null)
        {
            Response.Redirect("Default.aspx");
        }
        else
        {
            string strSql = "INSERT INTO tb_LeaveWord(Uid,Subject,Content,DateTime,IP)";
            strSql += " VALUES('" + Session["UserName"].ToString() + "','" + this.txtTitle.Text + "'";
            strSql += ",'" + this.FreeTextBox1.Text + "','" + DateTime.Now + "'";
            strSql += ",'" + Request.UserHostAddress + "')";
            dbObj.ExecNonQuery(dbObj.GetCommandStr(strSql));
            Response.Write(ccObj.MessageBox("Submitted!", "Default.aspx")); 
        }
        
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        this.txtTitle.Text = "";
        this.FreeTextBox1.Text = "";
    }
    protected void btnBack_Click(object sender, EventArgs e)
    {
        Response.Redirect("Default.aspx");
    }
}
