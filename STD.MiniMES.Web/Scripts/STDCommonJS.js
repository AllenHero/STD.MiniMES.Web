//���ݰ󶨶���
function ZTEBindData(){
    var viewModel = {};
	
	//���ݰ󶨷���������ΪJSON����
	this.BindData = function(modelObj){
	    if (modelObj) {
			for (var item in modelObj) {
				viewModel[item] = ko.observable(modelObj[item]);
			}
			ko.applyBindings(viewModel);
		}
	};
	
	//�����֮��Ľ����JSON������ʽ
	this.JsonResult = function(){
	    var jsonData = ko.toJS(viewModel);
		return jsonData;
	};
	
	//�����֮��Ľ����ViewModel������ʽ
	this.ViewModel = function(){
	    return viewModel;
	};
}

//
function ZTESetData(){
    var viewModel = {};

	//��URL���ֵ���󶨵�ҳ����
    this.BindDataFromUrl = function(){
	    var zteUrl = new ZTEUrl();
	    var url = location.href; 
	    var paraString = url.substring(url.indexOf("?")+1,url.length).split("&");
		for (i=0; j=paraString[i]; i++){
		    var parName = paraString[i].substring(0, paraString[i].indexOf("="));
		    viewModel[parName] = ko.observable(zteUrl.GetQueryString(parName));
        }
		ko.applyBindings(viewModel);
	};
	
	//�����֮��Ľ����JSON������ʽ
	this.JsonResult = function(){
	    var jsonData = ko.toJS(viewModel);
		return jsonData;
	};
	
	//�����֮��Ľ����ViewModel������ʽ
	this.ViewModel = function(){
	    return viewModel;
	};
}

function ZTEUrl(){
    //�Ӹ�����URL�л�ø�����������ֵ
    this.GetQueryString = function(name){
		var reg = new RegExp("(^|&)"+ name +"=([^&]*)(&|$)"); 
		var r = window.location.search.substr(1).match(reg);
		if(r!=null){
			return  unescape(r[2]);
		}
		else{
			return null;
		}
	};
	
	//��URL�е�ֵת��JSON������ʽ
	this.Url2Json = function(){
	    var url = window.location.href;
		//��ȡ���ַ�λ��
		var index = url.indexOf('?');
		//��ȡurl��������
		url = url.substr(index+1); 
		var arr = new Array();
		//������������
		arr = url.split('&'); 

		var str = '{';
		 //���������鲿��ת��ΪJSON����
		for(var i=0;i<arr.length;i++){
			var index1 = arr[i].indexOf('=');
			//�ж����һ�������Ƿ���ֵ
			if(index1 == -1){
				str+=',"'+arr[i]+'":""';
				continue;
			}
			var key = arr[i].substr(0,index1);
			var val = arr[i].substr(index1+1);
			var str1= ',';
			if(i==0){
				str1 = '';
			}
			str+=str1+'\"'+key+'\":'+'\"'+val+'\"';
		}
	    return JSON.parse(str+'}');
	};
	
	//��JSON�е�ֵת��URL��ʽ
	this.Json2Url = function(param, key){
		var paramStr = ""; 
		if(param instanceof String || param instanceof Number || param instanceof Boolean){  
			paramStr += "&" + key + "=" + encodeURIComponent(param); 
		}
		else{
			$.each(param,function(i){ 
				var k = key == null ? i : key + (param instanceof Array ? "[" + i + "]" : "." + i); 
				var temp = new ZTEUrl();
				paramStr += '&'+ temp.Json2Url(this, k);  
			});
		}
		return paramStr.substr(1);  
	};
}

//��ø�����ligerGrid��Ҳ����
function GetPageSize(grid) {
    if (grid) {
        return $(".l-bar-selectpagesize select", grid.toolbar).val() - 0;
    } else {
        return 10;
    }
}

//��ø�����ligerGrid��Ҳҳ��
function GetPageIndex(grid) {
    if (grid) {
        return $('.pcontrol input', grid.toolbar).val() - 0;
    } else {
        return 1;
    }
}

//�ӵ�ǰҳ���URL�л��ָ��name������ֵ
function GetQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return unescape(r[2]); return null;
}

//���һ��GUID
function GetGuid() {
    var guid = "";
    for (var i = 1; i <= 32; i++) {
        var n = Math.floor(Math.random() * 16.0).toString(16);
        guid += n;
        if ((i == 8) || (i == 12) || (i == 16) || (i == 20)) {
            guid += "-";
        }
    }
    return guid;
}

//JSҳ����ת
function Redirect(url) {
    window.location.href = url;
}



/******************************uditor�ؼ�html���ݵı�ǩ�ű������뷴���봦��*************************************************/
//html��ǩ��dom�ű����б���
this.REGX_HTML_ENCODE = /"|&|'|<|>|[\x00-\x20]|[\x7F-\xFF]|[\u0100-\u2700]/g;
this.encodeHtml = function (s) {
    return (typeof s != "string") ? s :
    s.replace(this.REGX_HTML_ENCODE,
    function ($0) {
        //var c = $0.charCodeAt(0), r = ["&#"];
        var c = $0.charCodeAt(0), r = ["@#"];
        c = (c == 0x20) ? 0xA0 : c;
        r.push(c); r.push(";");
        return r.join("");
    });
};

//ά�����������
this.HTML_DECODE = {
    // Add more
    "@#38;": "&",
    "@#60;": "<",
    "@62;": ">",
    "@#34;": "\"",
    "@#160;": " "
};

//html��ǩ��dom�ű����������з����봦��
//this.REGX_HTML_DECODE = /&\w+;|&#(\d+);/g;
this.REGX_HTML_DECODE = /@\w+;|@#(\d+);/g;
this.decodeHtml = function (s) {
    return (typeof s != "string") ? s :
    s.replace(this.REGX_HTML_DECODE,
    function ($0, $1) {
        var c = this.HTML_DECODE[$0]; // ���Բ��
        if (c === undefined) {
            // Maybe is Entity Number
            if (!isNaN($1)) {
                c = String.fromCharCode(($1 == 160) ? 32 : $1);
            } else {
                // Not Entity Number
                c = $0;
            }
        }
        return c;
    });
};

//������ʱȥ�ո�
this.trim = function (s) {
    s = (s != undefined) ? s : this.toString();
    return (typeof s != "string") ? s :
    s.replace(this.REGX_TRIM, "");
};
//���ҷ�����ά�����Ƿ���ڸñ���
this.hashCode = function () {
    var hash = this.__hash__, _char;
    if (hash == undefined || hash == 0) {
        hash = 0;
        for (var i = 0, len = this.length; i < len; i++) {
            _char = this.charCodeAt(i);
            hash = 31 * hash + _char;
            hash = hash & hash; // Convert to 32bit integer
        }
        hash = hash & 0x7fffffff;
    }
    this.__hash__ = hash;
    return this.__hash__;
};



//js���ڸ�ʽת��
function formatTen(num) {
    return num > 9 ? num : "0" + num;
}
//���ڸ�ʽ������
function formatDate(value) {
    //var year = value.getFullYear();
    //var month = value.getMonth() + 1;
    //var date = value.getDate();
    //return year + "-" + formatTen(month) + "-" + formatTen(date);
    return value.split('T')[0];
}
