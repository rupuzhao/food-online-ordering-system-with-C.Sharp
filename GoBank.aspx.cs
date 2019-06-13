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
using System.Runtime.InteropServices;

public partial class GoBank : System.Web.UI.Page
{
    DBClass dbObj = new DBClass();
    public static BankPay bankpay = new BankPay();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            bankpay = GetPayInfo();
            /*check if login*/
            ST_check_Login();
        }
    }
    public void ST_check_Login()
    {

        if ((Session["UserName"] == null))
        {
            Response.Write("<script>alert('Please login first!');location='Default.aspx'</script>");
            Response.End();
        }
    }
 
    #region  initialize BankPay
    public BankPay  GetPayInfo()
    { 
       
         string strSql = "select Round(TotalPrice,2) as TotalPrice from tb_OrderInfo where OrderID=" + Convert.ToInt32(Page.Request["OrderID"].Trim());
        DataTable dsTable = dbObj.GetDataSetStr(strSql, "tbOI");
        bankpay.Orderid = Request["OrderID"].Trim();                                                  //order No
        bankpay.Amount = Convert.ToString(float.Parse(dsTable.Rows[0]["TotalPrice"].ToString())*100); //order amount
        bankpay.OrderDate = DateTime.Now.ToString("yyyyMMddhhmmss");                                  //order date
        bankpay.Path1 = Server.MapPath(@"bank\user.crt");
        bankpay.Path2 = Server.MapPath(@"bank\user.crt");
        bankpay.Path3 = Server.MapPath(@"bank\user.key");

        bankpay.Msg = bankpay.InterfaceName + bankpay.InterfaceVersion + bankpay.MerID + bankpay.MerAcct + bankpay.MerURL + bankpay.NotifyType + bankpay.Orderid + bankpay.Amount + bankpay.CurType + bankpay.ResultType + bankpay.OrderDate + bankpay.VerifyJoinFlag;
        //ICBCEBANKUTILLib.B2CUtil obj=new ICBCEBANKUTILLib.B2CUtil() ;
       // if (obj.init(bankpay.Path1, bankpay.Path2, bankpay.Path3, bankpay.Key) == 0)
       // {
       //     bankpay.MerSignMsg = obj.signC(bankpay.Msg, bankpay.Msg.Length);
       //     bankpay.MerCert = obj.getCert(1);        
      //  }
      //  else
      //  {
      //      Response.Write(obj.getRC());
      //  }
        return (bankpay);
    }
    #endregion
    protected void Button1_Click(object sender, EventArgs e)
    {
        Response.Write("<script>alert('Payment Confirmed！');location='Default.aspx'</script>");
    }
}
