<%@ Page Language="c#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head id="Head1" runat="server">
    <title>Test Page For RealEstateListingViewer</title>
</head>
<body style="height: 100%; margin: 0;">
    <form id="form1" runat="server" style="height: 100%;">
    <div id="silverlightControlHost">
        <object data="data:application/x-silverlight," type="application/x-silverlight-2"
            width="100%" height="100%">
            <param name="source" value="ClientBin/RealEstateListingViewer.xap" />
            <param name="onerror" value="onSilverlightError" />
            <param name="background" value="white" />
            <param name="culture" value="es" />
            <param name="uiculture" value="es" />
            <!-- <param name="culture" value="nl" />
            <param name="uiculture" value="nl" />-->
            <a href="http://go.microsoft.com/fwlink/?LinkID=115261" style="text-decoration: none;">
                <img src="http://go.microsoft.com/fwlink/?LinkId=108181" alt="Get Microsoft Silverlight"
                    style="border-style: none" />
            </a>
        </object>
        <iframe style='visibility: hidden; height: 0; width: 0; border: 0px'></iframe>
    </div>
    </form>
</body>
</html>