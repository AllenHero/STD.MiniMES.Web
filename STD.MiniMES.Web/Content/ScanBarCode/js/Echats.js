
//echatrs配置
var myChart = echarts.init($('#main')[0]);
//var convertData = function (data) {
//  var res = [];
//  for (var i = 0; i < data.length; i++) {
//      var dataItem = data[i];
//      var fromCoord = geoCoordMap[dataItem[0].name];
//      var toCoord = geoCoordMap[dataItem[1].name];
//      if (fromCoord && toCoord) {
//          res.push({
//              fromName: dataItem[0].name,
//              toName: dataItem[1].name,
//              coords: [fromCoord, toCoord]
//          });
//      }
//  }
//  return res;
//};

option = {
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
     color:['#EC3939', '#ECA539','#12BE12'],
    legend: {
        orient: 'vertical',
        x: 'left',
     
    },
 
    series: [
        {
            name:'延误量',
            type:'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '30',
                        fontWeight: 'bold',
                        backgroundColor: 'red', 
                    }
                }    
            },
            label:{
				normal:{
					show: true,
				    position:'inside',
				    backgroundColor:'red',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontSize: 16,
					fontFamily:'Agency FB',

                    },
				}
				},
            labelLine: {
                normal: {
                    show: false
                }
            },
            data:[
                {value:235, name:'延误量<80%',selected: false,},
                {value:410, name:'延误量<70%'},
                {value:134, name:'延误量<60%'},
               
            ]
        }
    ]
};
myChart.setOption(option);

option2 = {
    tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
     color:['#EC3939', '#ECA539','#12BE12'],
    legend: {
        orient: 'vertical',
        x: 'left',
     
    },
 
    series: [
        {
            name:'当天计划监控总体完成率',
            type:'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '30',
                        fontWeight: 'bold',
                        backgroundColor: 'red', 
                    }
                }    
            },
            label:{
				normal:{
					show: true,
				    position:'inside',
				    backgroundColor:'red',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontSize: 16,
					fontFamily:'Agency FB',

                    },
				}
				},
            labelLine: {
                normal: {
                    show: false
                }
            },
            data:[
                {value:335, name:'完成率<80%',selected: false,},
                {value:310, name:'完成率<70%'},
                {value:234, name:'完成率<60%'},
               
            ]
        }
    ]
};
myChart2 = echarts.init(document.getElementById('main2'));
myChart2.setOption(option2);

option3 = {  
     tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
     color:['#EC3939', '#ECA539','#12BE12'],
    legend: {
        orient: 'vertical',
        x: 'left',
     
    },
 
    series: [
        {
            name:'延误量',
            type:'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '30',
                        fontWeight: 'bold',
                        backgroundColor: 'red', 
                    }
                }    
            },
            label:{
				normal:{
					show: true,
				    position:'inside',
				    backgroundColor:'red',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontSize: 16,
					fontFamily:'Agency FB',

                    },
				}
				},
            labelLine: {
                normal: {
                    show: false
                }
            },
            data:[
                {value:235, name:'延误量<80%',selected: false,},
                {value:410, name:'延误量<70%'},
                {value:134, name:'延误量<60%'},
               
            ]
        }
    ]
};
myChart3 = echarts.init(document.getElementById('main3'));
myChart3.setOption(option3);

option4 = {
	
   tooltip: {
        trigger: 'item',
        formatter: "{a} <br/>{b}: {c} ({d}%)"
    },
     color:['#EC3939', '#ECA539','#12BE12'],
    legend: {
        orient: 'vertical',
        x: 'left',
     
    },
 
    series: [
        {
            name:'延误量',
            type:'pie',
            radius: ['50%', '70%'],
            avoidLabelOverlap: false,
            label: {
                normal: {
                    show: false,
                    position: 'center'
                },
                emphasis: {
                    show: true,
                    textStyle: {
                        fontSize: '30',
                        fontWeight: 'bold',
                        backgroundColor: 'red', 
                    }
                }    
            },
            label:{
				normal:{
					show: true,
				    position:'inside',
				    backgroundColor:'red',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontSize: 16,
					fontFamily:'Agency FB',

                    },
				}
				},
            labelLine: {
                normal: {
                    show: false
                }
            },
            data:[
                {value:235, name:'延误量<80%',selected: false,},
                {value:410, name:'延误量<70%'},
                {value:134, name:'延误量<60%'},
               
            ]
        }
    ]
};
myChart4 = echarts.init(document.getElementById('main4'));
myChart4.setOption(option4);


option5 = {
      
 title: {
        x: 'center',
        textStyle: {
            color: '#4db64b',
            fontStyle: 'normal',
            fontWeight: 'bolder',
            fontFamily: "微软雅黑",
            fontSize: 20,

        },

    },
    tooltip: {
        trigger: 'axis',
        axisPointer: { // 坐标轴指示器，坐标轴触发有效
            type: 'shadow' // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    grid: {
		top:'10%',
        left: '1%',
        right: '3%',
        bottom: '10%',
        containLabel: true,

    },
    
    xAxis: [
	     
        {
            type: 'category',
			 boundaryGap: false,
            data: ['5/1', '5/2', '5/3', '5/4', '5/5', '5/6', '5/7','5/8', '5/9', '5/10', '5/11', '5/12', '5/13', '5/14','5/15', '5/16', '5/17', '5/18', '5/19', '5/20', '5/21','5/21', '5/22', '5/23', '5/24', '5/25', '5/26', '5/27','5/28', '5/29', '5/30', '5/31'],
			
            axisTick: {
                alignWithLabel: true,
							
            },
           
            //设置X轴属性
		
            axisLine: {
                show: true,
                onZero: true,
                interval: 45,
                lineStyle: {
                    color: '#8C97B5',
                    width:0,
                    type: 'solid',
                },
            },
			splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#2BBF7C'],
				  width: 1,
				  type: 'solid',
				  
				  shadowOffsetX: 0,
				  shadowOffsetY: 0,
				  opacity:0.3,
				  },
				  },
			
        }
		
    ],
    yAxis: [
        {
            type: 'value',
			 min: 0,
            max: 400,
            interval: 100,
            //设置X轴属性
            axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#8C97B5',
                    width: '0',
                    type: 'solid',
                },
            },
			splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#2BBF7C'],
				  width: 1,
				  type: 'solid',
				  
				  shadowOffsetX: 0,
				  shadowOffsetY: 0,
				  opacity:0.3,
				  },
				  },
        },
    ],
    series: [
        {
            name: '产量',
            type: 'line',
			symbol: 'emptyCircle',
			symbolSize: 4,
			symbolOffset: [0, 0],
			showSymbol: true,
			showAllSymbol: true,
			borderwidth:'4',
			
            smooth: true,
            smoothMonotone:'X',
			itemStyle: {
					normal: {
					color: 'rgba(255,255,255,0.5)',
					borderWidth:10,
  areaStyle: { type: 'default', color: {
  type: 'linear',
  x: 0,
  y: 0,
  x2: 0,
  y2: 1,
  colorStops: [{
	  offset: 0, color: 'rgba(189,243,42,0.8)',// 0% 处的颜色
  }, {
	  offset: 1, color: 'rgba(57,251,103,0.1)',// 100% 处的颜色
  }],
  globalCoord: false // 缺省为 false
 }
 },
					
					},
			   },
			lineStyle: {
				 normal: { 
				    color: {
						type: 'linear',
						x: 0,
						y: 0,
						x2: 0,
						y2: 1,
						colorStops: [{
							offset: 1, color: '#b4fe53' // 0% 处的颜色
						}, {
							offset: 0, color: '#07FA6F' // 100% 处的颜色
						}],
						globalCoord: false // 缺省为 false
						  }, 
						  width:5,
						  borderType: 'solid',
						  shadowColor: 'rgba(0, 0, 0, 0.5)',
						  shadowBlur:8,
						   shadowOffsetX: 0,
						  shadowOffsetY: 8,
	  
						  },
						  
						  },
								
     
             data: [150, 282, 200, 234, 160, 330, 280,150, 282, 200, 234, 160, 330, 280,150, 282, 200, 234, 160, 330, 280,150, 282, 200, 234, 160, 330, 280,150, 282, 200, 234, ],
          
		      label: {  //显示参数
                normal: {
                    show: true,
                    position: 'top',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontWeight: 'normal',
					fontSize: 20,
					fontFamily:'Agency FB',
				    formatter: '{value} %'
                    },
					
                },
				
            },
			
            //柱状样式
             
            //显示最大和最小值
           
            //显示平均值横线
          
        }
    ]
};

myChart5 = echarts.init(document.getElementById('main5'));
myChart5.setOption(option5);

