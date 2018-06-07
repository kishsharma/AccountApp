<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" CodeFile="AspxSearchBlock.aspx.vb" Inherits="AspxPages_AspxSearchBlock" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
 <form id="FrmSearchBlock" runat="server">
        <asp:HiddenField ID="HdnIdBlock" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="HdnIdBlockActiveType" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="LblScreenName" runat="server" ClientIDMode="Static" />
        <!--Fast Filter Input Control Starts Here-->
        <div class="filter_box">
            <div class="filter_heading">Type to Filter:</div>
            
            <input type="text" id="search_input" runat="server" placeholder="Type to filter" clientidmode="Static" />
            <input type="submit" name="button" id="BtnAddHREF" value="Add New" class="add_view_btn" runat="server" />
        </div>
        <!--Fast Filter Input Control ends Here-->
        <div class="filter_list_box">
         <ul id="search_list" class="list-inline">
            <asp:Repeater ID="RptrBlock" runat="server">
                <ItemTemplate>
                 <li >
                    <div class="filter_list">
                        <div class="filter_img"><img style="max-width: 100%; max-height: 100%;" alt="<%# DataBinder.Eval(Container.DataItem, "BlockDetail")%>" src="<%# DataBinder.Eval(Container.DataItem, "BlockImage") %>" width="75%" height="75%"/></div>
                        <div class="filter_right">
                            <div class="filter_content"><%# DataBinder.Eval(Container.DataItem, "BlockDetail")%></div>
                            <div class="filter_icon_box">
                                <a href="#" onclick="javascript:CallServerCodeEditRecord('<%# DataBinder.Eval(Container.DataItem, "IdBlock") %>');"><img src="../Resource/Images/edit.png" title="Edit Details" /></a>
                                <a href="#" onclick="javascript:CallServerCodeInactiveRecord('<%# DataBinder.Eval(Container.DataItem, "IdBlock") %>','<%# DataBinder.Eval(Container.DataItem, "BlockActiveTitle")%>');"><img src="<%# DataBinder.Eval(Container.DataItem, "BlockActive") %>" title="<%# DataBinder.Eval(Container.DataItem, "BlockActiveTitle")%>"/></a>
                                <a href="#" onclick="javascript:CallServerCodeDeleteRecord('<%# DataBinder.Eval(Container.DataItem, "IdBlock") %>');"><img src="../Resource/Images/delete.png" title="Delete Details"/></a>
                            </div>
                        </div>
                    </div>
                    </li>
                </ItemTemplate>
            </asp:Repeater>
            </ul>
        </div>
        <asp:Button ID="BtnEditRecord" runat="server" style="display:none"/>
        <asp:Button ID="BtnInactiveRecord" runat="server" style="display:none"/>
        <asp:Button ID="BtnDeleteRecord" runat="server" style="display:none"/>
    </form>
    <!--Link Clicks Event Starts Here-->
    <script type="text/javascript">
        //    Edit Record
        function CallServerCodeEditRecord(src) {
            document.getElementById('<%=HdnIdBlock.ClientId%>').value = src;
            callDummyButtonEditRecordServerEvent();
        }
        function callDummyButtonEditRecordServerEvent() {
            $('input[id$=BtnEditRecord]').click();
        }
        //        In-Active / Active Record
        function CallServerCodeInactiveRecord(src, srctype) {
            //alert("Are you sure you want to " + srctype + " ?");
            var check = confirm("Are you sure you want to " + srctype + " ?");
            if (check == true) {
                document.getElementById('<%=HdnIdBlock.ClientId%>').value = src;
                document.getElementById('<%=HdnIdBlockActiveType.ClientId%>').value = srctype;
                callDummyButtonInactiveRecordServerEvent();
                return true;
            }
            else {
                return false;
            }
        }
        function callDummyButtonInactiveRecordServerEvent() {
            $('input[id$=BtnInactiveRecord]').click();
        }
        //        Delete Record
        function CallServerCodeDeleteRecord(src) {
            var check = confirm("Are you sure you want to Delete?");
            if (check == true) {
                document.getElementById('<%=HdnIdBlock.ClientId%>').value = src;
                callDummyButtonDeleteRecordServerEvent();
                return true;
            }
            else {
                return false;
            }
            
        }
        function callDummyButtonDeleteRecordServerEvent() {
            $('input[id$=BtnDeleteRecord]').click();
        }
    </script>
    <!--Link Clicks Event Ends Here-->
    <!--Fast Filter JS Include Starts Here-->
    
    <script src="../Resource/js/FastLiveFilter/jquery.fastLiveFilter.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $('#search_input').fastLiveFilter('#search_list');
        });
    </script>
    <script type="text/javascript">
        $('#search_input').fastLiveFilter('#search_list', {
            timeout: 200,
            callback: function (total) { $('#num_results').html(total); }
        });
    </script>
    <!--Fast Filter JS Include Ends Here-->
</asp:Content>

