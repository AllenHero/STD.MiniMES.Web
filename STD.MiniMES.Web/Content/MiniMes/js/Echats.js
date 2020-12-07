function GetRequest() {
    var url = location.search; //获取url中含"?"符后的字串
    var theRequest = new Object();
    if (url.indexOf("?") != -1) {
        var str = url.substr(1);
        strs = str.split("&");
        for (var i = 0; i < strs.length; i++) {
            theRequest[strs[i].split("=")[0]] = unescape(strs[i].split("=")[1]);
        }
    }
    return theRequest;
}


//echatrs配置
$(function(){      
        //echatrs配置
    dataArray = [[], [], []];
    var Request = new Object();
    Request = GetRequest();
    $.ajax({
        url: "../pro_information/GetDailyReportData",
        data: {
            "LineNO": Request['LineNO'],
            "TenantID": Request['TenantId']
        },
        async: false,
        success: function (data) {

            var result = $.parseJSON(data);
            for (var i = 0; i < result["DataTable"].length; i++) {
                dataArray[0].push(result["DataTable"][i].PlanQty);
                dataArray[1].push(result["DataTable"][i].Qty);
                dataArray[2].push(result["DataTable"][i].Rate);

            }
        }
    });
        option = {        
                xAxis: [     
                    {
                        show: true,
                        type: 'category',
                        data: ['目标    实际', '目标    实际', '目标     实际', '目标    实际'],  
                        axisPointer: {
                            type: 'shadow'
                        },
        
                        axisLabel: { //调整x轴的lable  
                            textStyle: {
                                fontSize: 16,// 让字体变大
                                color: 'rgba(255,255,255,0.6)',
                            }
                        },
                        axisLine: {
                            show: true,
                            onZero: true,
                            lineStyle: {
                                color: 'rgba(255,255,255,0)',
                                width: 1,
                                type: 'solid',
                            },
                        },
                        splitLine: {
                            show: true,
                            interval: 'auto',
                            lineStyle: {
                                color: ['rgba(255,255,255,0.4)'],
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
                        name: '总量',
                        min: 0,
                        //max: 800,
                        //interval: 200,
        
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
                                color: '#B1BCCD',
                                width: 0,
                            },
                        },
                        splitLine: {
                            show: true,
        
                            interval: 'auto',
                            lineStyle: {
                                color: ['rgba(255,255,255,0.4)'],
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
                        max: 100.00,
                        interval: 25.00,
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
                                color: '#B1BCCD',
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
                    top: '10%',
                    left: '1%',
                    right: '1%',
                    bottom: '4%',
                    containLabel: true
                },
                series: [
                    {
                        name: '目标',
                        type: 'bar',
                        color: ['#135f76'],
                        barWidth: '18%',
                        data: dataArray[0],
                        itemStyle: {
                            normal: {
                                barBorderRadius: 4,
                                shadowBlur: 50,
                                shadowColor: 'rgba(0,0,0, 0.2)',
                                shadowOffsetX: 0,
                                shadowOffsetY: 6,
                                color: new echarts.graphic.LinearGradient(
                                    0, 0, 0, 1,
                                    [
                                        { offset: 0, color: '#0066FF' },
        
                                        { offset: 1, color: '#0066FF' }
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
                                    color: 'rgba(255,255,255,0.8)',
                                    fontStyle: 'normal',
                                    fontWeight: 'bold',
                                    fontSize: 20,
                                },
                            },
                        },
                    },
                    {
                        name: '总装目标数量',
                        type: 'bar',
                        color: ['#11bf64'],
                        barWidth: '18%',
                        data: dataArray[1],
                        itemStyle: {
                            normal: {
                                barBorderRadius: 4,
                                shadowBlur: 50,
                                shadowColor: 'rgba(0, 0, 0, 0.2)',
                                shadowOffsetX: 0,
                                shadowOffsetY: 6,
                                /*    color: new echarts.graphic.LinearGradient(
                                         0, 0, 0, 1,
                                         [
                                             {offset: 0, color: '#02BB5F'},
                                             {offset: 1, color: '#02BB5F'}
                                         ]
                                     ),*/
                                color: function (params) {
                                    var dataIndex=params.dataIndex;        
                                    if (option.series[2].data[dataIndex] >= 85) {
                                        return '#02BB5F';
                                    } else {
                                        return 'red';
                                    }
                                },
                            },
                            //emphasis: {
                            //    color: new echarts.graphic.LinearGradient(
                            //        0, 0, 0, 1,
                            //        [
                            //            { offset: 0, color: '#02BB5F' },
                            //            { offset: 1, color: '#02BB5F' }
                            //        ]
                            //    )
                            //}
                        },
                        label: {  //显示参数
                            normal: {
                                show: true,
                                position: 'inside',
                                textStyle: {
                                    color: '#edfffc',
                                    fontStyle: 'normal',
                                    fontWeight: 'bold',
                                    fontSize: 20,
                                },
                            },
        
                        },
                    },     
                    {
                        name: '效率达成率',
                        type: 'line',
                        borderwidth: '8',
                        label: {
                            normal: {
                                show: true,
                                position: 'top',
                                formatter: '{c} %',
        
                            }
                        },
        
                        itemStyle: {
                            normal: {
                                color: '#00CCCC',
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
                                        offset: 1, color: '#FFFF00' // 0% 处的颜色
                                    }, {
                                        offset: 0, color: '#00FF66' // 100% 处的颜色
                                    }],
        
                                    globalCoord: false // 缺省为 false
                                },
                                width: 8,
                                borderType: 'solid',
                                shadowColor: 'rgba(0, 0, 0, 0.2)',
                                shadowBlur: 0,
                                shadowOffsetX: 0,
                                shadowOffsetY: 0,
        
                            },
                        },
                        symbolSize: 6,
        
                        yAxisIndex: 1,
                        data: dataArray[2],
                        axisLabel: {
                            normal: {
                                show: true,
                                position: 'outside',
                                formatter: '{value}%'// 这里是数据展示的时候显示的数据
                            }
                        },
                        itemStyle: {
                            normal: {
                                barBorderRadius: 4,
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
                                        color: 'rgba(255,255,255,1)',
                                        fontWeight: 'normal',
                                        fontSize: 22,
        
                                    },
                                },
                            },
                        },
                    },
                ],
        
            };
        myChart = echarts.init($('#main')[0]);
        
        myChart.setOption(option); 
        window.onresize = myChart.resize;
});


function LoadCurRecordData() {
    var Request = new Object();
    Request = GetRequest();
    $.ajax({
        url: "../pro_information/GetDailyReportData",
        data: {
            "LineNO": Request['LineNO'],
            "TenantID": Request['TenantId']
        },
        async: false,
        success: function (data) {

            var result = $.parseJSON(data);

            var valueData0 = [];
            var valueData1 = [];
            var valueData2 = [];
            var valueData3 = [];
            var max = 100;
            for (var i = 0; i < result["DataTable"].length; i++) {
                valueData0.push(result["DataTable"][i].PlanQty);
                valueData1.push(result["DataTable"][i].Qty);
                valueData2.push(result["DataTable"][i].Rate);
                $("#valueData" + i).html(result["DataTable"][i].ProductModel);
                if(result["DataTable"][i].Rate>max)
                {
                    max = result["DataTable"][i].Rate;
                }
            }

            option.series[0].data = valueData0;
            option.series[1].data = valueData1;
            option.series[2].data = valueData2;
            option.yAxis[1].max = max;

            myChart.setOption(option);
            window.onresize = myChart.resize;
        }
    });
}
setInterval(LoadCurRecordData, 3000);












