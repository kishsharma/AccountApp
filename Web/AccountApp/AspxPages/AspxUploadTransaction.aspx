<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="AspxUploadTransaction.aspx.vb" Inherits="AspxPages_AspxUploadTransaction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <link href="../Resource/js/Chosen/chosen.css" rel="stylesheet" type="text/css" />
    <%--   <script src="../Resource/js/image_zoom/jquery.zoom.js" type="text/javascript"></script>--%>
  <%--  <script src="../Resource/js/jquery.js" type="text/javascript"></script>--%>
    <script src="../Resource/js/jquery.panzoom.js" type="text/javascript"></script>
    <script src="../Resource/js/jquery.mousewheel.js" type="text/javascript"></script>
    <script>
        $(function () {
            $("#TxtTransactionDt").datepicker({
                changeMonth: true,
                changeYear:true,
                dateFormat: 'dd-M-yy',
                onClose: function () {
                    $(this).trigger('blur');
                }
            }).val();
        });
    </script>
    <script>
        $(document).ready(function () {
            $(".panzoom").draggable({
                handle: "img"
            });
        });
</script>
    
    <style type="text/css">
        body
        {
            background-color: #f7f7f7;
            background-repeat: repeat-x;
            margin-left: 0px;
            margin-top: 0px;
            margin-right: 0px;
            margin-bottom: 0px;
        }
    </style>
    <style type="text/css">
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
        .headingleft
        {
            float: left;
            width: 50%;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #000000;
            text-transform: uppercase;
            padding: 0px 0 10px 0;
        }
        .headingright
        {
            float: left;
            width: 50%;
            height: auto;
            font-family: Arial, Helvetica, sans-serif;
            font-size: 15px;
            color: #000000;
            text-transform: uppercase;
            padding: 0px 0 10px 0;
        }
        .heading24
        {
            padding: 0px 0 0px 18px !important;
        }
    </style>
    <script>
        $(setup)
        function setup() {
            $('.intro select').zelect({ placeholder: 'Plz select...' })
        }

    </script>
    <form id="FrmUploadTransaction" runat="server" role="form">
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
    <div class="heading24">
        <strong>Upload Transaction</strong></div>
    <div class="company_box">
        <div id="resultsel1" style="display: block" class="info_image">
            <br />
            <asp:Label ID="listofuploadedfiles" runat="server"  />
            <asp:Literal ID="ltEmbed" runat="server" Visible="false" />
            <section id="pan-when-zoomed" style="display:none">
              <div class="parent">
                <div class="panzoom">
                  <img id="imgprvw" runat="server" src="../Upload/Transaction/UploadTransaction.png" style="display:inline"/>
                </div>
              </div>
              <div class="buttons">
                <button class="zoom-in">Zoom In</button>
                <button class="zoom-out">Zoom Out</button>
               <%-- <input type="range" class="zoom-range">--%>
                <button class="reset">Reset</button>
              </div>
              <script>
                  (function () {
                      var $section = $('#pan-when-zoomed');
                      $section.find('.panzoom').panzoom({
                          $zoomIn: $section.find(".zoom-in"),
                          $zoomOut: $section.find(".zoom-out"),
                          $zoomRange: $section.find(".zoom-range"),
                          $reset: $section.find(".reset"),
                          panOnlyWhenZoomed: true,
                          minScale: 1
                      });
             
                  })();
         
              </script>
	          <script>
	              (function () {
	                  var $section = $('#pan-when-zoomed');
	                  var $panzoom = $section.find('.panzoom').panzoom();
	                  $panzoom.parent().on('mousewheel.focal', function (e) {
	                      e.preventDefault();
	                      var delta = e.delta || e.originalEvent.wheelDelta;
	                      var zoomOut = delta ? delta < 0 : e.originalEvent.deltaY > 0;
	                      $panzoom.panzoom('zoom', zoomOut, {
	                          animate: false,
	                          focal: e
	                      });
	                  });
	              })();
              </script>
            </section>
        </div>
        <div id="resultselright" style="display: block" class="info_box">
            <div class="form-group col-md-6" style="overflow: scroll; height: 445px;">
                <div class="company_list">
                    <label id="Label5" runat="server">
                        Company</label>
                    <%-- <section class="intro">--%>
                    <asp:DropDownList ID="DDLCompany" runat="server" AutoPostBack="false" class="chosen-select"
                        TabIndex="1" />
                    <%-- </section>--%>
                </div>
                <div class="company_list" style="display:none">
                    <label id="Label1" runat="server">
                        Transaction Type</label>
                    <section class="intro" style="display:none" >
                        <asp:DropDownList ID="DDLTransaction" runat="server" TabIndex="2"  Visible="false" />
                    </section>
                    </div>
                <div class="company_list" >
                <div class="filter_list" style="width:95%;height: 90%" >
                    <div class="filter_content" style="height:30px;min-height:30px">Transaction Type</div>
<label class="container">Purchase
  <input type="radio" id="RdPUR" runat="server"  name="radio" value="6">
  <span class="checkmark"></span>
</label> 
                              <label class="container">Sales
  <input type="radio"  id="RdSL" runat="server" name="radio" value="7">
  <span class="checkmark"></span>
</label> 
        
                             <label class="container">Receipt
  <input type="radio"  id="RdRCPT" runat="server"  name="radio" value="10">
  <span class="checkmark"></span>
</label> 
                              <label class="container">Payment
  <input type="radio"  id="RdPAY" runat="server"  name="radio" value="9">
  <span class="checkmark"></span>
</label> 
                                                   <label class="container">Others
  <input type="radio"  id="RdJE" runat="server"  name="radio" value="8">
  <span class="checkmark"></span>
</label> 
                    </div>
                          
                <div class="company_list" style="display:none" >
                     <asp:RadioButton ID="RdPUR1" runat="server" GroupName="TT" Text="Purchase"  />
                    <br />
                    <asp:RadioButton ID="RdSL1" runat="server" GroupName="TT" Text="Sales" />
                    <br />
                    <asp:RadioButton ID="RdRCPT1" runat="server" GroupName="TT" Text="Receipt" />
                    <br />
                    <asp:RadioButton ID="RdPAY1" runat="server" GroupName="TT" Text="Payment" />
                    <br />
                    <asp:RadioButton ID="RdJE1" runat="server" GroupName="TT" Text="Others" />
                    <br />


                    <%--<input type="radio" name="" value="PUR" id="" runat="server">
                    <br>
                    <input type="radio" name="TT" value="SL" id="" runat="server">
                    <br>
                    <input type="radio" name="TT" value="RCPT" id="" runat="server">
                    <br>
                    <input type="radio" name="TT" value="PAY" id="" runat="server">
                    <br>
                    <input type="radio" name="TT" value="JE" id="" runat="server">--%>
                </div>
                <div class="company_list">
                    <label id="Label2" runat="server">
                        Bill Number</label>
                    <asp:TextBox ID="TxtTransactionId" runat="server" class="form-control" placeholder="Bill Number"
                        ClientIDMode="Static" TabIndex="3" MaxLength="100" onblur="return LoadValuetoSerchTag()"
                        onkeypress="return IsAlphaNumeric(event);" />
                    <%--                    <input type="text" class="form-control" runat="server" id="TxtTransactionId" placeholder="Bill Number"
                        clientidmode="Static" tabindex="3" maxlength="100" onblur="return LoadValuetoSerchTag()" onkeypress="return IsAlphaNumeric(event);" />--%>
                </div>
                <div class="company_list date_box">
                    <label>
                        Transaction Date *
                    </label>
                    <asp:TextBox ID="TxtTransactionDt" runat="server" ClientIDMode="Static" class="form-control"
                        onblur="return LoadValuetoSerchTag()" placeholder="Transaction Date (DD-MMM-YYYY)"
                        TabIndex="3" MaxLength="11" />
                </div>
                <div class="company_list">
                    <label id="Label3" runat="server">
                        Transaction Remark</label>
                    <input type="text" class="form-control" runat="server" id="TxtTransactionRemark"
                        placeholder="Transaction Remark" clientidmode="Static" tabindex="3" maxlength="255"
                        onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="company_list">
                    <label id="Label4" runat="server">
                        Search Tags</label>
                    <input type="text" class="form-control" runat="server" id="TxtTransactionTags" placeholder="Transaction Search Tags"
                        clientidmode="Static" tabindex="3" maxlength="255" onkeypress="return IsAlphaNumeric(event);" />
                </div>
                <div class="heading15" style="font-weight: bold; margin: 20px 0px 0px; color: #fff;
                    padding: 9px 10px; background-color: #00bcd5">
                    <h2>
                        Image Uploads:</h2>
                </div>
                <div class="company_list" style="width: 48%;">
                    <asp:FileUpload ID="flupload" runat="server" CssClass="btnupload" TabIndex="11" Style="float: left; background-color: #fff; border: 0px none; padding: 5px; width: auto; margin: 0px 0px 10px;"  AllowMultiple="true" />
                    <legend>Transaction Image View before upload</legend>
                    <asp:Button ID="btnUpload" runat="server" CssClass="submit_btn" Text="View" OnClick="fnUpload" Style="float: left; padding: 5px 10px; font-size: 12px;" OnClientClick="return validateImageUpload();" TabIndex="12" />
                    <asp:Button ID="BtnClearImage" runat="server" CssClass="submit_btn" OnClick="fnClear" Text="Clear" Style="float: left; padding: 5px 10px; font-size: 12px;" TabIndex="13" />
                    <br />
                </div>
            </div>
            <div class="company_list">
                <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit_btn" OnClientClick="return validate()"
                    TabIndex="14" />
                <asp:Button ID="btnreset" runat="server" Text="Reset" CssClass="submit_btn" TabIndex="15"
                    OnClientClick="return resetFields()" />
            </div>
        </div>
    </div>
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
    <script type="text/javascript">
        function resetFields() {
            document.getElementById("<%=DDLCompany.ClientID%>").value = "0";
            document.getElementById("<%=DDLTransaction.ClientID%>").value = "0";
            document.getElementById("<%=TxtTransactionId.ClientID%>").value = "";
            document.getElementById("<%=TxtTransactionRemark.ClientID%>").value = "";
            document.getElementById("<%=TxtTransactionTags.ClientID%>").value = "";

            return true;
        }
    </script>
    <script type="text/javascript">
        function validate() {
            if (document.getElementById("<%=DDLCompany.ClientID %>").value.trim() == "0") {
                alert("Please Select Company");
                document.getElementById("<%=DDLCompany.ClientID %>").focus();
                return false;
            }

            if (document.getElementById("<%=DDLTransaction.ClientID %>").value.trim() == "0") {
                alert("Please Select Transaction Type");
                document.getElementById("<%=DDLTransaction.ClientID %>").focus();
                return false;
            }
            if (document.getElementById("<%=TxtTransactionId.ClientID %>").value.trim() == "") {
                alert("Please Provide Purchase ID / Sales ID / Receipt ID / Payment ID/ Journal ID as per Applicable");
                document.getElementById("<%=TxtTransactionId.ClientID %>").focus();
                return false;
            }

        }
    </script>
    <!-- Check the Keypressed is Alphabetic or Numeric or for Cursor Movement-->
    <script type="text/javascript">
        var specialKeys = new Array();
        specialKeys.push(8); //Backspace
        specialKeys.push(9); //Tab
        specialKeys.push(46); //Delete
        specialKeys.push(36); //Home
        specialKeys.push(35); //End
        specialKeys.push(37); //Left
        specialKeys.push(39); //Right
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            //alert(keyCode);
            var ret = ((keyCode = 39) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }

        function LoadValuetoSerchTag() {
            var x = document.getElementById("<%=TxtTransactionId.ClientID %>");
            var y = document.getElementById("<%=TxtTransactionDt.ClientID%>");
            document.getElementById("<%=TxtTransactionTags.ClientID%>").value = x.value + ', ' + y.value;
        }
    </script>
    </form>
</asp:Content>
