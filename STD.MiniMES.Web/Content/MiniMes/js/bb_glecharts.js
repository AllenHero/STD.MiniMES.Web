
//echatrs配置
var myChart = echarts.init($('#main')[0]);


option = {
    



    tooltip : {
        trigger: 'axis',
        axisPointer : {            // 坐标轴指示器，坐标轴触发有效
            type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    legend: {
        data:['DIP','工程','自动化','仓储中心','市场部','供应商']
    },
    grid: {
		top:'16%',
        left: '3%',
        right: '4%',
        bottom: '3%',
        containLabel: true
    },
    xAxis : [
        {
            type : 'category',
            data : ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25','26','27','28','29','30',]
        }
    ],
    yAxis : [
        {
            type : 'value'
        }
    ],
    series : [
   /*     {
            name:'直接访问',
            type:'bar',
            data:[320, 332, 301, 334, 390, 330, 320]
        },*/
        {
            name:'DIP',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 98.00, 100.00, 30.00, 70.00,80.00, 50.00, 100.00, 88.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 100.00, 50.00, 70.00,80.00, 60.00, 100.00, 98.00, 80.00, 50.00, 70.00, 70.00,]
        },
        {
            name:'工程',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 60.00, 98.00, 70.00, 50.00, 70.00,80.00, 80.00, 100.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 100.00, 50.00, 70.00,80.00, 80.00, 100.00, 98.00, 90.00, 50.00, 70.00, 70.00,]
        },
        {
            name:'自动化',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 78.00, 100.00, 60.00, 70.00,80.00, 80.00, 90.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 80.00, 50.00, 70.00,80.00, 80.00, 20.00, 98.00, 80.00, 50.00, 70.00, 70.00,]
        },
		{
            name:'仓储中心',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 78.00, 100.00, 60.00, 70.00,80.00, 80.00, 90.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 80.00, 50.00, 70.00,80.00, 80.00, 20.00, 98.00, 80.00, 50.00, 70.00, 70.00,]
        },
		{
            name:'市场部',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 78.00, 100.00, 60.00, 70.00,80.00, 80.00, 90.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 80.00, 50.00, 70.00,80.00, 80.00, 20.00, 98.00, 80.00, 50.00, 70.00, 70.00,]
        },
		{
            name:'供应商',
            type:'bar',
            stack: '损失工时',
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 78.00, 100.00, 60.00, 70.00,80.00, 80.00, 90.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 80.00, 50.00, 70.00,80.00, 80.00, 20.00, 98.00, 80.00, 50.00, 70.00, 70.00,]
        },
        /*{
            name:'搜索引擎',
            type:'bar',
            data:[862, 1018, 964, 1026, 1679, 1600, 1570],
            markLine : {
                lineStyle: {
                    normal: {
                        type: 'dashed'
                    }
                },
                data : [
                    [{type : 'min'}, {type : 'max'}]
                ]
            }
        },
        {
            name:'百度',
            type:'bar',
            barWidth : 5,
            stack: '搜索引擎',
            data:[620, 732, 701, 734, 1090, 1130, 1120]
        },
        {
            name:'谷歌',
            type:'bar',
            stack: '搜索引擎',
            data:[120, 132, 101, 134, 290, 230, 220]
        },
        {
            name:'必应',
            type:'bar',
            stack: '搜索引擎',
            data:[60, 72, 71, 74, 190, 130, 110]
        },
        {
            name:'其他',
            type:'bar',
            stack: '搜索引擎',
            data:[62, 82, 91, 84, 109, 110, 120]
        }*/
    ]

};
myChart.setOption(option);

option2 = {
    
  xAxis: [
        {
            type: 'category',
            data: ['波峰焊','打包',],
			 axisLabel: {
						  show: true,
						  textStyle: {
						  fontSize:18,
                           }
                        },
            axisPointer: {
                type: 'shadow',
				
            },
			
		
        axisLine: {
            show: true,
            onZero: true,
            lineStyle: {
                color: '#333',
                width: 0,
                type: 'solid',
            },
			
        },
		 splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#999'],
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
            min: 0.00,
            max: 100.00,
            interval:10.00,
            axisLabel: {
                formatter: '{value} %'
            },
			 axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#333',
                    width: 0,
					
                },
				
            },
			 splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#999'],
				  width: 1,
				  type: 'solid',
				  
				  shadowOffsetX: 0,
				  shadowOffsetY: 0,
				  opacity:0.3,
				  },
				  },
			
        },
 
        
    ],
	  grid: {
		top:'16%',
        left: '2%',
        right: '2%',
        bottom: '4%',
        containLabel: true
    },
    series: [
	      
	  
		{
            name:'不合格数量',
            type:'bar',
		    color: ['#999'],
			barWidth:'18%',
            data:[80.00, 50.00, 70.00, 98.00,],
			
			itemStyle: {
                normal: {
					barBorderRadius:6,
					shadowBlur: 3,
					shadowColor:'rgba(0, 0, 0, 0.2)',
					shadowOffsetX: 4,
					shadowOffsetY: 0,
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: '#0981BC'},
                            {offset: 0.5, color: '#0981BC'},
                            {offset: 1, color: '#0981BC'}
                        ]
                    )
                },
                emphasis: {
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: 'green'},
                            {offset: 0.7, color: 'green'},
                            {offset: 1, color: 'green'}
                        ]
                    )
                }
            },
			 label: {  //显示参数
                normal: {
                    show: true,
                    position: 'top',
                    textStyle: {
					color: '#FF6600',
					fontStyle: 'normal',
					fontWeight: 'bold',
					fontSize: 22,
				    formatter: '{value} %'
                    },
					
                },
				
            },
			
        },
		

       
              
		
    ],

};
myChart2 = echarts.init(document.getElementById('main2'));
myChart2.setOption(option2);



option3 = {

   xAxis: [

                {
                    type: 'category',
                    data: ['DIP', '自动化', '工程', '仓储中心','市场部','供应商'],

                    axisPointer: {
                        type: 'shadow'

                    },

                    axisLabel: { //调整x轴的lable  
                        textStyle: {
                            fontSize: 18,// 让字体变大
                            color: '#333',
                        }
                    },
                    axisLine: {
                        show: true,
                        onZero: true,
                        lineStyle: {
                            color: '#999',
                            width: 1,
                            type: 'solid',

                        },


                    },
                    splitLine: {
                        show: true,
                        interval: 'auto',
                        lineStyle: {
                            color: ['#999'],
                            width: 1,
                            type: 'solid',

                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            opacity: 0.3,
                        },
                    },

                },

            ],
            yAxis: [
                {
                    type: 'value',
                    name: '总量(万)',
                    min: 0,
                    max: 60,
                    interval: 10,

                    axisLabel: { //调整x轴的lable 
                        formatter: '{value}',
                        textStyle: {
                            fontSize: 16 // 让字体变大
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    axisLine: {
                        show: true,
                        onZero: true,
                        lineStyle: {
                            color: '#333',
                            width: 0,
                        },
                    },
                    splitLine: {
                        show: true,

                        interval: 'auto',
                        lineStyle: {
                            color: ['#999'],
                            width: 1,
                            type: 'solid',

                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            opacity: 0.3,
                        },
                    },

                },
                {
                    type: 'value',
                    name: '效率',
                    min: 0.00,
                    max: 120.00,
                    interval: 20.00,

                    axisLabel: { //调整x轴的lable 
                        formatter: '{value}%',
                        textStyle: {
                            fontSize: 16 // 让字体变大
                        }
                    },
                    axisTick: {
                        show: false
                    },
                    axisLine: {
                        show: true,
                        onZero: true,
                        lineStyle: {
                            color: '#333',
                            width: 0,

                        },
                    },
                    splitLine: {
                        show: true,
                        interval: 'auto',
                        lineStyle: {
                            color: ['rgba(114,134,163,0.3)'],
                            width: 2,
                            type: 'solid',

                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            opacity: 0.3,
                        },

                    },
                }
            ],
            grid: {
                top:'16%',
                left: '2%',
                right: '2%',
                bottom: '4%',
                containLabel: true
            },
            series: [
                {
                    name: '总装完成数量',
                    type: 'bar',
                    color: ['#0981BC'],
                    barWidth: '28%',
                    data: [50, 30, 50, 50, 30, 50],

                    itemStyle: {

                        normal: {
                            barBorderRadius: 6,
                            shadowBlur: 0,
                            shadowColor: 'rgba(1, 234, 29, 0.2)',
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            color: new echarts.graphic.LinearGradient(
                                0, 0, 0, 1,
                                [
                                    { offset: 0, color: '#0981BC' },

                                    { offset: 1, color: '#0981BC' }
                                ]
                            ),

                        },
                        emphasis: {
                            color: new echarts.graphic.LinearGradient(
                                0, 0, 0, 1,
                                [
                                    { offset: 0, color: '#0066FF' },

                                    { offset: 1, color: '#0066FF' }
                                ]
                            )
                        },

                    },
                    label: {  //显示参数
                        normal: {
                            show: true,
                            position: 'inside',
                            textStyle: {
                                color: 'rgba(255,255,255,0.9)',
                                fontStyle: 'normal',
                                fontWeight: 'bold',
                                fontSize: 22,
                            },
                        },

                    },

                },
               
                {
                    name: '效率达成率',
                    type: 'line',
                    borderwidth: '6',
                    label: {
                        normal: {
                            show: true,
                            position: 'top',
                            formatter: '{c} %',

                        }
                    },

                    itemStyle: {
                        normal: {
                            color: '#00CC00',
                            borderWidth: 0,
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
                                    offset: 1, color: '#FF6600' // 0% 处的颜色
                                }, {
                                    offset: 0, color: '#FF6600' // 100% 处的颜色
                                }],
                                globalCoord: false // 缺省为 false
                            },
                            width:6,
                            borderType: 'solid',
                            shadowColor: 'rgba(0, 0, 0, 0.2)',
                            shadowBlur: 0,
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,

                        },
                    },
                    symbolSize: 1,

                    yAxisIndex: 1,
                    data: [80.00, 90.88, 102.89, 96.00, 102.89, 96.00],
                    axisLabel: {
                        normal: {
                            show: true,
                            position: 'outside',
                            formatter: '{value}%'// 这里是数据展示的时候显示的数据
                        }
                    },
                    itemStyle: {
                        normal: {
                            barBorderRadius: 8,
                            shadowBlur: 0,
                            shadowColor: 'rgba(0, 0, 0, 0.5)',
                            shadowOffsetX: 0,
                            shadowOffsetY: 0,
                            color: 'yellow',
                            borderWidth: 4,
                            borderType: 'solid',
                            label: {
                                show: true,
                                formatter: null,
                                textStyle: {
                                    color: '#FF6600',
                                    fontWeight: 'bold',
                                    fontSize: 22,

                                },
                            },
                        },
                    },
                },
            ],

};
myChart3 = echarts.init(document.getElementById('main3'));
myChart3.setOption(option3);

option4 = {
   xAxis: [
        {
            type: 'category',
            data: ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19','20','21','22','23','24','25','26','27','28','29','30'],
            axisPointer: {
                type: 'shadow'
            },
		
        axisLine: {
            show: true,
            onZero: true,
            lineStyle: {
                color: '#999',
                width: 0,
                type: 'solid',
            },
        },
		 splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#999'],
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
            min: 0.00,
            max: 100.00,
            interval:10.00,
            axisLabel: {
                formatter: '{value} %'
            },
			 axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#999',
                    width: 0,
                },
            },
			 splitLine: {
				  show: true,
				  interval: 'auto',
				  lineStyle: {
				  color: ['#999'],
				  width: 1,
				  type: 'solid',
				  
				  shadowOffsetX: 0,
				  shadowOffsetY: 0,
				  opacity:0.3,
				  },
				  },
			
        },
 
        
    ],
	  grid: {
		top:'16%',
        left: '2%',
        right: '2%',
        bottom: '4%',
        containLabel: true
    },
    series: [
	      
	  
		{
            name:'不合格数量',
            type:'bar',
		    color: ['#999'],
			barWidth:'40%',
            data:[80.00, 80.00, 90.00, 98.00, 100.00, 50.00, 70.00,80.00, 80.00, 100.00, 98.00, 80.00, 50.00, 70.00, 70.00,80.00,
			 80.00, 90.00, 98.00, 100.00, 50.00, 70.00,80.00, 80.00, 100.00, 98.00, 80.00, 50.00, 70.00, 70.00],
			
			itemStyle: {
                normal: {
					barBorderRadius:3,
					shadowBlur: 3,
					shadowColor:'rgba(0, 0, 0, 0.2)',
					shadowOffsetX: 4,
					shadowOffsetY: 0,
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: '#15FD63'},
                            {offset: 0.5, color: '#09D147'},
                            {offset: 1, color: '#FFCC00'}
                        ]
                    )
                },
                emphasis: {
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: 'blue'},
                            {offset: 0.7, color: '#2378f7'},
                            {offset: 1, color: '#83bff6'}
                        ]
                    )
                }
            },
			 label: {  //显示参数
                normal: {
                    show: true,
                    position: 'top',
                    textStyle: {
					color: '#5a5a5a',
					fontStyle: 'normal',
					fontWeight: 'normal',
					fontSize: 16,
				    formatter: '{value} %'
                    },
					
                },
				
            },
			
        },
		

       
              
		
    ],

};
myChart4 = echarts.init(document.getElementById('main4'));
myChart4.setOption(option4);


option5 = {
      
   xAxis: [
        {
            type: 'category',
            data: ['5月1日','5月2日','5月3日','5月4日','5月5日','5月6日','5月7日'],
            axisPointer: {
                type: 'shadow'
            },
		
        axisLine: {
            show: true,
            onZero: true,
            lineStyle: {
                color: '#B4FCC9',
                width: 0,
                type: 'solid',
            },
        },
		
        }
    ],
    yAxis: [
        {
            type: 'value',
            min: 0.00,
            max: 120.00,
            interval:40.00,
            axisLabel: {
                formatter: '{value} %'
            },
			 axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#B4FCC9',
                    width: 0,
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
	  grid: {
		top:'16%',
        left: '2%',
        right: '2%',
        bottom: '4%',
        containLabel: true
    },
    series: [
	      
	  
		{
            name:'不合格数量',
            type:'bar',
		    color: ['#135f76'],
			barWidth:'40%',
            data:[80.00, 80.00, 102.00, 98.00, 110.00, 50.00, 70.00],
			
			itemStyle: {
                normal: {
					barBorderRadius:3,
					shadowBlur: 3,
					shadowColor:'rgba(0, 0, 0, 0.5)',
					shadowOffsetX: 4,
					shadowOffsetY: 0,
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: '#15FD63'},
                            {offset: 0.5, color: '#09D147'},
                            {offset: 1, color: '#FFCC00'}
                        ]
                    )
                },
                emphasis: {
                    color: new echarts.graphic.LinearGradient(
                        0, 0, 0, 1,
                        [
                            {offset: 0, color: 'blue'},
                            {offset: 0.7, color: '#2378f7'},
                            {offset: 1, color: '#83bff6'}
                        ]
                    )
                }
            },
			 label: {  //显示参数
                normal: {
                    show: true,
                    position: 'top',
                    textStyle: {
					color: '#fefefe',
					fontStyle: 'normal',
					fontWeight: 'normal',
					fontSize: 12,
				   formatter: '{value} %'
                    },
					
                },
				
            },
			
        },
		

       
              
		
    ],

};

myChart5 = echarts.init(document.getElementById('main5'));
myChart5.setOption(option5);

