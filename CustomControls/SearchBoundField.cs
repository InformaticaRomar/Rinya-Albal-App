using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public class SearchBoundField : System.Web.UI.WebControls.BoundField
    {
        private const string SEARCH_EXPRESSION = "SearchExpression";
        public string SearchExpression
        {
            get
            {
                if (this.ViewState[SEARCH_EXPRESSION] == null)
                {
                    this.ViewState[SEARCH_EXPRESSION] = this.DataField;
                }

                return (string)this.ViewState[SEARCH_EXPRESSION];
            }
            set
            {
                this.ViewState[SEARCH_EXPRESSION] = value;
            }
        }
    }
}
