<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="AspxAssignImages.aspx.vb" Inherits="AspxPages_AspxAssignImages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    
   <!--Featured Properties-LEFT-RIGHT-->
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.8.3/jquery.min.js"></script>
    <script src="../Resource/js/left_right_scroll/responsiveCarousel.min.js" type="text/javascript"></script>
    <script type="text/javascript">
$(function(){
  $('.crsl-items').carousel({
    visible: 5,
    itemMinWidth: 180,
    itemEqualHeight: 250,
    itemMargin: 0,
  });
  
  $("a[href=#]").on('click', function(e) {
    e.preventDefault();
  });
  
});
    </script>
    <!--Featured Properties-LEFT-RIGHT-->
    <form id="frmAssignimage" runat="server" role="form">
      <asp:HiddenField ID="Hdnstatus" runat="server" ClientIDMode="Static" />
      <asp:HiddenField ID="HdnTrnType" runat="server" ClientIDMode="Static" />
    <style>
        .heading15
        {
            float: left;
            width: 98%;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #000000;
            text-transform: uppercase;
            padding: 0px 0 10px 0;
        }
        #thumbnail-slider div.inner
        {
            border: 4px solid rgba(0,0,0,0.3);
        }
    </style>
    <input type="hidden" class="form-control" runat="server" id="strDrivers" clientidmode="Static" />
    <div class="heading24">
        <strong>Assign Image</strong></div>
    <div class="company_box">
       
               <input type="checkbox" id="ChkOnlyPDF" runat="server" clientidmode="Static" tabindex="4" />
                <label class="panel_list_text" for="ChkOnlyPDF">
                    <span></span>Show Only PDF
                </label> 
        <div id="resultsel1" style="display: block" class="company_details">
            
            <div class="company_list">
                
                <label id="lblCompany" runat="server">
                    Company Name<span id="span5" runat="server" class="MandatoryMark">*</span></label>
               
                <asp:DropDownList ID="DDLCompanyname" runat="server" TabIndex="1" AutoPostBack="true"
                    class="chosen-select" />
               
            </div>
            <div class="company_list">
                <label id="Label2" runat="server">
                    Transaction Type<span id="span2" runat="server" class="MandatoryMark">*</span></label>
               
                <asp:DropDownList ID="DDLTransactiontype" runat="server" TabIndex="2" AutoPostBack="true"
                    class="chosen-select" />
               
            </div>
            <div class="company_list">
                <label id="Label1" runat="server">
                    User Name <span id="span1" runat="server" class="MandatoryMark">*</span></label>
              
                <asp:DropDownList ID="DDLUsername" runat="server" TabIndex="3" AutoPostBack="true"
                    class="chosen-select"/>
               
            </div>
            <div class="company_list">
                    <label id="Label3" runat="server">
                    UnAssigned Image <span id="span3" runat="server" class="MandatoryMark">*</span></label>
                <br />
                <asp:ListBox ID="lstUnassignedimage" TabIndex="4" runat="server" SelectionMode="Multiple" Height="250px">
                </asp:ListBox>
            </div>
            <div class="transfer_btn">
                <input type="button" name="test0" onclick="selectPush();" class="blue_btn1" /><br />
                <br />
                <input type="button" name="test2" onclick="DeletePush();" class="blue_btn2" id="Button3" />&nbsp;
            </div>
            
            <div class="company_list">
                 <label id="Label4" runat="server">
                    Assigned Image <span id="span4" runat="server" class="MandatoryMark">*</span></label>
                <asp:ListBox ID="lstAssignedimage" TabIndex="5" runat="server" SelectionMode="Multiple" Height="250px">
                </asp:ListBox>
            </div>
            
        </div>
        <div class="company_list">
            <asp:Button ID="btnSave" CssClass="submit_btn" runat="server" Text="Save" TabIndex="6"/>
        </div>
        <div class="company_list">
            <asp:Button ID="btnreset" CssClass="submit_btn" runat="server" Text="Reset" OnClientClick="return resetFields()"
                TabIndex="7" />
        </div>
        <div class="heading15" style="font-weight: bold; display:none; margin: 20px 0px 0px; color: #fff;
            padding: 9px 10px; background-color: #00bcd5">
            <h2>
                Thumbline Assigned Images:</h2>
        </div>
       
        <div id="mygallery" style="display:none;" class="stepcarousel_images" runat="server">
      
        </div>
    </div>
     <div style="float:left; width:100%; min-height:500px;">&nbsp;</div>
    <script type="text/javascript" language="javascript">
        function resetFields() {
            document.getElementById("<%=DDLCompanyname.ClientID%>").value = "0";
            document.getElementById("<%=DDLTransactiontype.ClientID%>").value = "0";
            document.getElementById("<%=DDLUsername.ClientID%>").value = "0";
            document.getElementById("<%=lstUnassignedimage.ClientId%>").options.length = 0;
            document.getElementById("<%=lstUnassignedimage.ClientID%>").value = "";
            return true;
        }
      
    </script>
    <script src="../Resource/js/Chosen/chosen.jquery.js" type="text/javascript"></script>
    <script type="text/javascript">
        var config = {
            '.chosen-select': {},
            '.chosen-select-deselect': { allow_single_deselect: true },
            '.chosen-select-no-single': { disable_search_threshold: 10 },
            '.chosen-select-no-results': { no_results_text: 'Oops, nothing found!' },
            '.chosen-select-width': { width: "95%" }
        }
        for (var selector in config) {
            $(selector).chosen(config[selector]);
        }
    </script>
  
    <script type='text/javascript'>

        function selectAll() {
            var el = document.getElementById("<%=lstUnassignedimage.ClientId%>");
            var e2 = document.getElementById("<%=lstAssignedimage.ClientId%>");

            if (el != null) {
                for (i = 0; i < el.options.length; i++) {
                    newItem = e2.options.length;
                    e2.options[newItem] = new Option(el.options[i].text);
                    e2.options[newItem].value = el.options[i].value;
                }
            }

            var e2 = document.Form1.select2;
            if (e2 != null) {
                for (i = 0; i < e2.options.length; i++) {
                    el.options[0] = null;
                }
            }
        }
        function selectPush() {
            var el = document.getElementById("<%=lstUnassignedimage.ClientId%>");
            var e2 = document.getElementById("<%=lstAssignedimage.ClientId%>");

            if (el != null) {
                for (i = 0; i < el.options.length; i++) {
                    if (el.options[i].selected == true) {
                        newItem = e2.options.length;
                        e2.options[newItem] = new Option(el.options[i].text);
                        e2.options[newItem].value = el.options[i].value;
                    }
                }
                for (i = 0; i < el.options.length; i++) {
                    if (el.options[i].selected == true) {
                        el.options[i] = null;
                        i = i - 1;
                    }
                }
            }
        }
        function DeletePush() {
            var el = document.getElementById("<%=lstUnassignedimage.ClientId%>");
            var e2 = document.getElementById("<%=lstAssignedimage.ClientId%>");

            if (e2 != null) {

                for (i = 0; i < e2.options.length; i++) {
                    if (e2.options[i].selected == true) {
                        var vVehicle = e2.options[i].value;
                        var vVehicleValues = vVehicle.split(",");
                        if (vVehicleValues[0] == "Y") {
                            alert("Sorry, the Map is already assigned to user");
                        }
                        else {
                            newItem = el.options.length;
                            el.options[newItem] = new Option(e2.options[i].text);
                            el.options[newItem].value = e2.options[i].value;
                        }
                    }
                }
                for (i = 0; i < e2.options.length; i++) {
                    if (e2.options[i].selected == true) {
                        var vVehicle = e2.options[i].value;
                        var vVehicleValues = vVehicle.split(",");
                        if (vVehicleValues[0] == "Y") {
                        }
                        else {
                            e2.options[i] = null;
                            i = i - 1;
                        }
                    }
                }
            }
        }
        function RemoveAll() {
            var el = document.getElementById("<%=lstUnassignedimage.ClientId%>");
            var e2 = document.getElementById("<%=lstAssignedimage.ClientId%>");

            if (e2 != null) {


                for (i = 0; i < e2.options.length; i++) {
                    var vVehicle = e2.options[i].value;
                    var vVehicleValues = vVehicle.split(",");
                    if (vVehicleValues[0] == "Y") {
                        alert("Sorry," + e2.options[i].text + " is already assigned to user");
                    }
                    else {
                        newItem = el.options.length;
                        el.options[newItem] = new Option(e2.options[i].text);
                        el.options[newItem].value = e2.options[i].value;
                    }
                }
            }
            var e2 = document.Form1.select2;
            if (e2 != null) {
                for (i = 0; i < e2.options.length; i++) {
                    var vVehicle = e2.options[i].value;
                    var vVehicleValues = vVehicle.split(",");
                    if (vVehicleValues[0] == "Y") {
                    }
                    else {
                        e2.options[i] = null;
                        i = i - 1;
                    }
                }
            }
        }

        function selectE2() {
//            if (document.getElementById("<%=DDLCompanyname.ClientID %>").value.trim() == "0") {
//                alert("Please Select Company");
//                document.getElementById("<%=DDLCompanyname.ClientID %>").focus();
//                return false;
//            }
            if (document.getElementById("<%=DDLTransactiontype.ClientID %>").value.trim() == "0") {
                alert("Please Select Transaction Type");
                document.getElementById("<%=DDLTransactiontype.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=DDLUsername.ClientID %>").value.trim() == "0") {
                alert("Please Select UserName");
                document.getElementById("<%=DDLUsername.ClientID %>").focus();
                return false;
            }
            var e2 = document.getElementById("<%=lstAssignedimage.ClientId%>");

            var string = "";
            if (e2 != null) {
                for (i = 0; i < e2.options.length; i++) {
                    e2.options[i].selected = true;
                    if (string != "") {
                        string = string + "','"
                        string = string + e2.options[i].value;
                    }
                    else {
                        string = string + "'"
                        string = e2.options[i].value;
                    }
                }
                document.getElementById("<%=strDrivers.ClientId%>").value = string;
            }
            var strval = document.getElementById("<%=strDrivers.ClientId%>");
            if (strval.value == "") {
                alert("Please Assign Image");
                return false;
            }
        }

    </script>
    </form>
</asp:Content>
