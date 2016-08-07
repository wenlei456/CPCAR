	String.prototype.trim = function()
	{
		return this.replace(/^\s+|\s+$/g, '');
	}
	
	String.prototype.len = function()
	{
		return this.replace(/[^\x00-\xff]/gi,"aa").length;
	}
	


	function GetAbsolute(vSrc)
	{
		var m = vSrc.offsetTop;
		var n = vSrc.offsetLeft;
		var vParent = vSrc.offsetParent;
		while (vParent.tagName.toUpperCase() != "BODY")
		{
			n += vParent.offsetLeft;
			m += vParent.offsetTop;
			vParent = vParent.offsetParent;
		}
		var a = new Array(2);
		a[0] = m;
		a[1] = n;
		return a;
	}
	
	function NotIsNull(obj)
	{
		var bl = false;
		if(typeof(obj) != "undefined" && typeof(obj) != null && obj != null)
		{
			bl = true;
		}
		return bl;
	}
	
	function XMLHttp()
	{
		var xmlHttp;
		if (window.ActiveXObject) //IE
		{
			xmlHttp = new ActiveXObject("Microsoft.XMLHTTP");
		} 
		else if (window.XMLHttpRequest)
		{
			xmlHttp = new XMLHttpRequest();
		}
		return xmlHttp;
		//xmlHttp.onreadystatechange = FunResultStatus;
		//if(xmlHttp.readyState==4)
		//var doc = new ActiveXObject("Microsoft.XMLDOM");
        //doc.async = "false";
        //doc.loadXML(xmlHttp.responseText);
	}

	function XMLHttpSend(xmlHttp,url,method,postData)
	{
		if(method=="GET")
		{
			xmlHttp.open("GET", url ,true);
			xmlHttp.send(null);
		}
		else
		{
			xmlHttp.open("POST", url ,true);
			xmlHttp.setRequestHeader("Content-Type","application/x-www-form-urlencoded");
			xmlHttp.send(postData);
		}
	}
	
	function XmlHttpAbort(xmlHttp)
	{
	    xmlHttp.abort();
	}
	
	function ReplaceAll(str,str1,str2)
	{
		var s = str;
		if(s=="" || s==null)
			return s;
		var i = s.indexOf(str1);
		while(i>=0)
		{
			s = s.substring(0,i) + str2 + s.substr(i+str1.length);
			i = s.indexOf(str1); 
		}
		return s;
	}
	
	function Cint(n)
	{
		return parseInt(n);
		//return parseFloat(n);
	}
	
	function IsNumber(s)
	{
		var parten = /^\d+$/; 
		return (parten.test(s));
	}
	
	function CheckEmail(email)
	{
		var parten = /\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*/; 
		return (parten.test(email));
	}
	
	function CheckUserNameValid(str)
	{
		var parten = /^[\u4e00-\u9fa5\w]+$/; //\w and chinese
		return (parten.test(str));	
	}

	function RemoveHTML(str)
	{
		return str.replace(/<[^>]*>/gi, "");
	}
	
	function Rnd()
	{
	    return Math.random();
	}
	//----------------------------IE And FireFox---------------------------------------------
	function MouseX(evt)
	{
		return (document.body.scrollLeft + evt.clientX);
	} 
	function MouseY(evt)
	{
		return (document.body.scrollTop + evt.clientY);
	}
	function MouseOffsetX(evt)
	{
		return ( evt.offsetX ? evt.offsetX : evt.layerX );
	}
	function MouseOffsetY(evt)
	{
		return ( evt.offsetY ? evt.offsetY : evt.layerY );
	}
	function SrcElement(evt) 
	{ 
		return ( evt.target ? evt.target : evt.srcElement ); 
	}
	function SetOpacity(obj, n)
	{
		if (typeof(obj.style.filter) != "undefined")
		{
			obj.style.filter = "alpha(opacity=" + n + ")";	
		}
		else
		{
			obj.style.opacity = n/100;	
		}
	}
	//----------------------------------------------------------------------------------------
	function CreateElement()
	{
		var div = document.createElement("div");
		div.setAttribute("id", "div_new");
		div.style.position = "absolute";
		div.style.filter = "alpha(opacity=30)";
		document.body.appendChild(div);
		var txtNode = document.createTextNode("123456");
		div.appendChild(txtNode);
		//$("div1").insertBefore(div, $("div2"));
		//parent.removeChild(div);
	}
	
	function ShowFlash(id, url, w, h)
	{
		var str = '';
		str += '<object width="'+ w +'" height="'+ h +'" classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=7,0,19,0">';
		str += '<param name="movie" value="'+ url +'">';
		str += '<param name="wmode" value="opaque">';
		str += '<param name="quality" value="autohigh">';
		str += '<embed width="'+ w +'" height="'+ h +'" src="'+ url +'" quality="autohigh" wmode="opaque" type="application/x-shockwave-flash" plugspace="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash" menu="false"></embed>';
		str += '</object>';
		$(id).innerHTML = str;	
	}
	
	
	function ShowFlashMov(id, url, w, h, textHeight, pics, links, texts)
	{
		var swf_height = h + textHeight;
		var pics1 = pics.replace("|", "");
		var links1 = links.replace("|", "");
		var texts1 = texts.replace("|", "");
		var str = '';
		str = '<embed width="'+ w +'" height="'+ swf_height +'" src="'+ url +'" quality="autohigh" wmode="opaque" type="application/x-shockwave-flash" plugspace="http://www.macromedia.com/shockwave/download/index.cgi?P1_Prod_Version=ShockwaveFlash" FlashVars="pics='+pics1+'&links='+links1+'&texts='+texts1+'&borderwidth='+w+'&borderheight='+h+'&textheight='+textHeight+'" menu="false" bgcolor="#DADADA"></embed>';
		$(id).innerHTML = str;	
	}
	
	function PreloadImg()
	{
		var imgs = new Array();
		var a = PreloadImg.arguments;
		for(var i=0; i<a.length; i++)
		{
			imgs[i] = new Image();
			imgs[i].src = a[i];
		}
	}
	
	function autoResize(Frame)
	{
		try
		{
			Frame.style.height = Frame.document.body.scrollHeight<300 ? 300 : Frame.document.body.scrollHeight;
		}
		catch(e){}
	}
	//<iframe id="win" name="win" onload="Javascript:autoResize(this)" frameborder="0" scrolling="no"></iframe>
	//setTimeout(function(){FunTimeout(div, url, n, t)}, 2000);
	//var option = new Option(text,value);
	//$("drp").options.add(option);
	
	<!-- Hide
	function killErrors() 
	{
		//return true;
	}
	//window.onerror = killErrors;
	// -->
	
	function GoToClick()
    {
        $("page").value = $("GoTo").value;
        __doPostBack('page','');
    }