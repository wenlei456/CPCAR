// JavaScript Document

var prev;
function showCon(obj){
	//隐藏前一个
	if(prev!=null){
		document.getElementById("con"+prev).style.display="none";
		document.getElementById("m"+prev).className="pa";
	}
	
	//显示这一个
	document.getElementById("con"+obj).style.display="block";
	document.getElementById("m"+obj).className="paA";
	
	prev=obj;
}