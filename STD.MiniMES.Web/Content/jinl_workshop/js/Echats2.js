
var myChart = echarts.init($('#main')[0]);
var option;
function LoadEquipmentDataCharts(x,y,z){

    option = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow',
                label: {
                    show: true,
                    backgroundColor: '#333'
                }
            }
        },
        legend: {
            show: true,
            data: ['总量', '效率'],
            textStyle: {
                color: '#eff1f3'
            }
        },

        xAxis: [
            {
                type: 'category',
                data: x,
                axisPointer: {
                    type: 'shadow'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,
                        type: 'solid',
                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.5)'],
                        width: 1,
                        type: 'solid',

                        shadowOffsetX: 0,
                        shadowOffsetY: 0,
                        opacity: 0.3,
                    },
                },

            }
        ],
        yAxis: [
            {
                type: 'value',
                name: '停机(min)',
                //min: 0,
                //max: 50,
                //interval: 10,
                axisLabel: {
                    formatter: '{value}'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,
                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.5)'],
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
                name: '时间利用率',
                //min: 0.00,
                //max: 100.00,
                //interval: 20.00,

                axisLabel: {
                    formatter: '{value} %'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,

                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.28)'],
                        width: 1,
                        type: 'solid',

                        shadowOffsetX: 0,
                        shadowOffsetY: 0,
                        opacity: 0.3,
                    },
                },
            }
        ],
        grid: {
            top: '20%',
            left: '3%',
            right: '1%',
            bottom: '8%',
            containLabel: true
        },
        series: [

            {
                name: '停机时间',
                type: 'bar',
                barWidth: '18%',
                color: ['rgba(87,188,255,0.9)'],
                data: y,
                itemStyle: {
                    normal: {
                        barBorderRadius: 8,
                        shadowBlur: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.14)',
                        shadowOffsetX: 4,
                        shadowOffsetY: -1 
                    },
                    emphasis: {
                        
                    }
                },
            },

            {
                name: '工时效率',
                type: 'line',
                borderwidth: '4',

                itemStyle: {
                    normal: {
                        color: '#ffcc2f',
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
                                offset: 1, color: '#dd9926' // 0% 处的颜色
                            }, {
                                offset: 0, color: '#dd9926' // 100% 处的颜色
                            }],
                            globalCoord: false // 缺省为 false
                        },
                        width: 4,
                        borderType: 'solid',
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowBlur: 0,
                        shadowOffsetX: 0,
                        shadowOffsetY: 6,

                    },

                },

                yAxisIndex: 1,
                data: z,
                axisLabel: {
                    formatter: '{value} %'
                },
                

            }, 
        ],

    }; 
    myChart.clear();
    myChart.setOption(option);  
}


var myChart3 = echarts.init($('#main3')[0]);
var option3;
function LoadExceptionWeekAnalysisCharts(x,y1,y2,y3) {
    option3 = {
    tooltip : {
        trigger: 'axis',
        axisPointer : {            // 坐标轴指示器，坐标轴触发有效
            type : 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
        }
    },
    legend: {
        data:['直接访问','邮件营销','联盟广告',]
    },
        grid: {
            top: '16%',
            left: '3%',
            right: '4%',
            bottom: '8%',
            containLabel: true
        },
    xAxis : [
        {
            type: 'category',
            data: x,
            axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#eff1f3',
                    width: 0,
                    type: 'solid',
                },
            },
            splitLine: {
                show: true,
                interval: 'auto',
                lineStyle: {
                    color: ['rgba(198,213,232,0.5)'],
                    width: 1,
                    type: 'solid',

                    shadowOffsetX: 0,
                    shadowOffsetY: 0,
                    opacity: 0.3,
                },
            },
        }
    ],
    yAxis : [
        {
            type: 'value',
            axisLine: {
                show: true,
                onZero: true,
                lineStyle: {
                    color: '#eff1f3',
                    width: 0,
                },
            },
            splitLine: {
                show: true,
                interval: 'auto',
                lineStyle: {
                    color: ['rgba(198,213,232,0.5)'],
                    width: 1,
                    type: 'solid', shadowOffsetX: 0,
                    shadowOffsetY: 0,
                    opacity: 0.3,
                },
            },
        }

    ],
    series : [


        {
            name:'品质异常',
            type:'bar',
            barWidth : 10,
            color: ['#1f6dcd'],
            stack: '异常汇总',
            data:y1
        },

        {
            name:'设备异常',
            type:'bar',
            color: ['#c6533c'],
            stack: '异常汇总',
            data:y2
        },
        {
            name:'生产异常',
            type:'bar',
            color: ['#42e0fc'],
            stack: '异常汇总',
            data:y3
        }
    ]
};
    myChart3.clear();
    myChart3.setOption(option3);  
}


var myChart2 = echarts.init($('#main2')[0]);
var option2;
function LoadFirstCheckCharts(RandomCount) { 
    option2 = {
        tooltip: {
            trigger: 'item',
            formatter: "{a} <br/>{b} : {c} ({d}%)"
        },
        series: [
            {
                name: '',
                type: 'pie',
                radius: '80%',
                center: ['50%', '60%'],
                data: RandomCount,
                itemStyle: {
                    emphasis: {
                        shadowBlur: 10,
                        shadowOffsetX: 0,
                        shadowColor: 'rgba(30, 144, 255，0.5)'
                    },
                    normal: {
                        label: {
                            textStyle: {
                                fontSize: 14,
                                fontWeight: 'bold',
                            }
                        },
                        color: function (params) {
                            //自定义颜色
                            var colorList = [
                                '#6cd49b', '#ff6f6d', '#F9F900', '#A8FF24', '#B766AD', '#FF2D2D', '#FF5809',
                                '#64A600', '#FF9224', '	#EAC100', '#C2FF68', '#AE0000', '#F00078',
                                '#00BB00', '#D200D2', "#D94600", "#01B468", "#009393", "#C6A300", "#A6A600", "#82D900", "#AD5A5A", "#5151A2", "#7E3D76"
                            ];
                            return colorList[params.dataIndex];
                        }
                    }
                }
                 
                 ,label: {
                    normal: {
                        position: 'inner',
                        formatter: '{d}%'
                    }
                },
                labelLine: {
                    normal: {
                        show: false
                    }
                },

            }
        ]
    }; 
    myChart2.clear();
    myChart2.setOption(option2);   
}

 


var myChart4 = echarts.init($('#main4')[0]);
var option4;
function LoadOrderNoProgressCharts(ProductCode, OrderNo, Qty, PlanQty) {
    $("#sp_ProductCode").html(ProductCode);
    $("#sp_OrderNo").html(OrderNo);
    var PlanRatio = 100;
    
    if (Qty <PlanQty) {
        if (PlanQty > 0) {
            PlanRatio = ((Qty /PlanQty) * 100).toFixed(0);
        }
      
    }
    option4 = { 
        series: [

            {
                name: '工单产量',
                type: 'pie',
                radius: ['80%', '90%'],
                center: ['50%', '50%'],
                startAngle: 225,
                
                labelLine: {
                    normal: {
                        show: false
                    }
                },
                label: {
                    normal: {
                        position: 'center'
                    }
                },
                data: [{
                    value: PlanRatio,
                    name: '工单产量',
                    label: {
                        normal: {
                            formatter: '\n\n工单产量',
                            textStyle: {
                                color: '#fff',
                                fontSize: 14
                            }
                        }
                    }
                }, {
                        value: (100 - PlanRatio),
                    name: '%',
                    label: {
                        normal: {
                            formatter: Qty+'',
                            textStyle: {
                                color: '#fff',
                                fontSize: 30

                            }
                        }
                    }
                },
                {
                    value: 0,
                    name: '%',
                    label: {
                        normal: {
                            formatter: '',
                            textStyle: {
                                color: '#fff',
                                fontSize: 16

                            }
                        }
                    }
                }]
            },

        ]
    };
    myChart4.clear();
    myChart4.setOption(option4);   
}




var myChart6 = echarts.init($('#main6')[0]);
var option6;
function LoadRandomPltoInfoCharts(x,y,z) {
    option6 = {
        tooltip: {
            trigger: 'axis',
            axisPointer: {
                type: 'shadow',
                label: {
                    show: true,
                    backgroundColor: '#333'
                }
            }
        },
        legend: {
            show: true,
            data: ['数量', '占比'],
            textStyle: {
                color: '#eff1f3'
            }
        },

        xAxis: [
            {
                type: 'category',
                data: x,
                axisPointer: {
                    type: 'shadow'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,
                        type: 'solid',
                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.5)'],
                        width: 1,
                        type: 'solid',

                        shadowOffsetX: 0,
                        shadowOffsetY: 0,
                        opacity: 0.3,
                    },
                },

            }
        ],
        yAxis: [
            {
                type: 'value',
                name: '不良数量',
                min: 0,
                max: 50,
                interval: 10,
                axisLabel: {
                    formatter: '{value}'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,
                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.5)'],
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
                name: '累计占比',
                min: 0.00,
                max: 100.00,
                interval: 20.00,

                axisLabel: {
                    formatter: '{value} %'
                },
                axisTick: {
                    show: false
                },
                axisLine: {
                    show: true,
                    onZero: true,
                    lineStyle: {
                        color: '#eff1f3',
                        width: 0,

                    },
                },
                splitLine: {
                    show: true,
                    interval: 'auto',
                    lineStyle: {
                        color: ['rgba(198,213,232,0.28)'],
                        width: 1,
                        type: 'solid',

                        shadowOffsetX: 0,
                        shadowOffsetY: 0,
                        opacity: 0.3,
                    },
                },
            }
        ],
        grid: {
            top: '20%',
            left: '3%',
            right: '1%',
            bottom: '8%',
            containLabel: true
        },
        series: [

            {
                name: '不良数量',
                type: 'bar',
                barWidth: '18%',
                color: ['rgba(87,188,255,0.9)'],
                data: y,
                itemStyle: {
                    normal: {
                        barBorderRadius: 8,
                        shadowBlur: 0,
                        shadowColor: 'rgba(0, 0, 0, 0.14)',
                        shadowOffsetX: 4,
                        shadowOffsetY: -1,
                         
                    },
                    emphasis: {
                         
                    }
                },
                
            },

            {
                name: '累计占比',
                type: 'line',
                borderwidth: '4',

                itemStyle: {
                    normal: {
                        color: '#ffcc2f',
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
                                offset: 1, color: '#dd9926' // 0% 处的颜色
                            }, {
                                offset: 0, color: '#dd9926' // 100% 处的颜色
                            }],
                            globalCoord: false // 缺省为 false
                        },
                        width: 4,
                        borderType: 'solid',
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowBlur: 0,
                        shadowOffsetX: 0,
                        shadowOffsetY: 6,

                    },

                },

                yAxisIndex: 1,
                data: z,
                axisLabel: {
                    formatter: '{value} %'
                }, 
            }, 
        ],

    };
    myChart6.clear();
    myChart6.setOption(option6);
}