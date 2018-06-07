<%@ Page Title="" Language="VB" MasterPageFile="~/Master/Account.master" AutoEventWireup="false"
    CodeFile="AspxTransactionImage.aspx.vb" Inherits="AspxPages_AspxTransactionImage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Resource/js/jquery.panzoom.js" type="text/javascript"></script>
    <script src="../Resource/js/jquery.mousewheel.js" type="text/javascript"></script>
    <link rel="stylesheet" href="https://www.w3schools.com/w3css/4/w3.css">
    <style type="text/css">
        .imgresp
        {
            width: 100%;
            float: left;
        }
    </style>
    <script>
        $(document).ready(function () {
            $(".panzoom").draggable({
                handle: "img"
            });
        });
        function ImgRotate(rotateDeg) {
            var i;
            var finalDeg=0;
            var deg = rotateDeg;
            var x = document.getElementsByClassName("mySlides");
            for (i = 0; i < x.length; i++) {
                if (x[i].style.display == "block") {                  
                    finalDeg = eval(x[i].getAttribute("data-rotate")) + eval(deg);
                    //var idimg = document.getElementById(x[i].getAttribute("id"));
                    //idimg.transform = "rotate(" + finalDeg + "deg)";
                    x[i].style.transform = "rotate(" + finalDeg + "deg)";
                      x[i].setAttribute("data-rotate", finalDeg);
                    }
            }
  }

            

        
    </script>
    <script>

        $(document).ready(function () {
showDivs(slideIndex);
        });
var slideIndex = 1;
function plusDivs(n) {
    showDivs(slideIndex += n);
    return false;
}

function showDivs(n) {
  var i;
    var x = document.getElementsByClassName("mySlides");
  if (n > x.length) {slideIndex = 1}    
  if (n < 1) {slideIndex = x.length}
  for (i = 0; i < x.length; i++) {
     x[i].style.display = "none";  
  }
    x[slideIndex - 1].style.display = "block";  
    return false;
        }


        function rotateImage(degree) {
	$('#image').animate({  transform: degree }, {
    step: function(now,fx) {
        $(this).css({
            '-webkit-transform':'rotate('+now+'deg)', 
            '-moz-transform':'rotate('+now+'deg)',
            'transform':'rotate('+now+'deg)'
        });
    }
    });
}
            
</script>
    <form id="frmAssignimage" runat="server" role="form">
    <asp:ScriptManager ID="SMLocationMaster" runat="server" EnablePageMethods="true"
        ScriptMode="Release">
    </asp:ScriptManager>
    <asp:HiddenField ID="HdnIDImage" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="HdnIdentity" runat="server" ClientIDMode="Static" />
        <%--<asp:HiddenField ID="hdnImageTag" runat="server" Value="<img class='mySlides' src='[Path]' style='width:80%' />" />--%>
    <div class="company_box">
          
        <div class="company_list company_list2">
            <label id="Label5" runat="server">
                Request New Image if you cant read it, Click here</label>
            <label id="Label7" runat="server">
                Reject Remark</label>
            <asp:TextBox ID="TxtRejectRemark" class="form-control" runat="server" placeholder=" Reject Remark"
                onblur="showtextbox();" ClientIDMode="Static" TabIndex="2" MaxLength="100" onkeypress="return IsAlphaNumeric(event);"></asp:TextBox>
            <asp:Button ID="BtnRequestNewImage" runat="server" Text="Request New Image" CssClass="submit_btn"
                TabIndex="1" OnClientClick="return validateRejectRemark()" />
            <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="submit_btn" TabIndex="2"
                OnClientClick="return validateOutSide()" />
            <br />
            <br />
            <br />
            <asp:Label ID="lblReViewRemark" runat="server" />
        </div>
        
        <div class="imgresp">
            <asp:Literal ID="ltEmbed" runat="server" />
            <section id="pan-when-zoomed">
               <div class="parent">
        <div class="panzoom">
          <img id="imgprvw" runat="server" src='../Upload/Transaction/UploadTransaction.png' alt='' style="max-width:100%;"/>
        
                   <div class="w3-content w3-display-container" id="divImageSlider" runat="server" >

                                   <span class="w3-button w3-black w3-display-left" onclick="plusDivs(-1)">&#10094;</span>
  <span class="w3-button w3-black w3-display-right"  onclick="plusDivs(1)">&#10095;</span>
                   </div>
            </div>
      </div>
      <div class="company_list buttons" style="align-content:center;position:center" >
        <button class="zoom-in">Zoom In</button>
        <button class="zoom-out">Zoom Out</button>
        <button class="reset">Reset</button>
        <img  src="../Resource/images/RotateImgArrow.jpg" onclick="ImgRotate(90)"  style="width:25px;height:25%;border:1px outset blue " />
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
    </div>
    </form>
    <script type="text/javascript" language="javascript">
        function IsAlphaNumeric(e) {
            var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
            var ret = ((keyCode == 32) || (keyCode == 39) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
            return ret;
        }
        function validateRejectRemark() {
            if (document.getElementById("<%=TxtRejectRemark.ClientID %>").value.trim() == "") {
                alert("Please Provide Reject Remark");
                document.getElementById("<%=TxtRejectRemark.ClientID %>").focus();
                return false;
            }
        }
        function validateOutSide() {
            var active = confirm('Are you sure you want to submit this image?, press OK to Proceed or press Cancel to be on Same Status.');
            if (active == true) {
                return true;
            }
            else {
                return false;
            }
        }
    </script>
</asp:Content>
