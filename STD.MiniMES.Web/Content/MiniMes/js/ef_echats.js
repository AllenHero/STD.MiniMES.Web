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

$(function () {
    var today = new Date();
    var submitTime = today.getFullYear() + '-' + (today.getMonth() + 1);
    document.getElementById("starttime").value = submitTime;
    GetEFSData();
});


function GetEFSData() {
    //echatrs配置
    Request = GetRequest();
    var TenantId = Request['TenantId'];
    var LineCode = Request['LineCode'];
    month = $("#starttime").val();
    dataArray = [[], [], [], [], []];
    $.ajax({
        url: "../efficiency_statistics/GetGetEFSData",
        data: { "TenantId": TenantId, "LineCode": LineCode, "Month": month},
        async: false,
        success: function (data) {

            var result = $.parseJSON(data);
            for (var i = 0; i < result["DataTable"].length; i++) {
                dataArray[0].push(result["DataTable"][i].Day);
                dataArray[1].push(result["DataTable"][i].Qty);
                dataArray[2].push(result["DataTable"][i].LineBalance);
                dataArray[3].push(result["DataTable"][i].Rate);
                dataArray[4].push(result["DataTable"][i].LostTime);
            }

        }
    });
    var myChart = echarts.init($('#main')[0]);
    option = {
        xAxis: [
          {
              type: 'category',
              data: dataArray[0],
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
                      opacity: 0.3,
                  },
              },
          }
        ],
        yAxis: [
            {
                type: 'value',
                min: 0.00,
                //max: 100.00,
                //interval: 10.00,
                axisLabel: {
                    formatter: '{value}'
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
                        opacity: 0.3,
                    },
                },
            },
        ],
        grid: {
            top: '12%',
            left: '2%',
            right: '2%',
            bottom: '4%',
            containLabel: true
        },
        series: [
            {
                name: '产量',
                type: 'bar',
                color: ['#999'],
                barWidth: '30%',
                data: dataArray[1],
                itemStyle: {
                    normal: {
                        barBorderRadius: 3,
                        shadowBlur: 3,
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowOffsetX: 4,
                        shadowOffsetY: 0,
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#15FD63' },
                                { offset: 0.5, color: '#09D147' },
                                { offset: 1, color: '#FFCC00' }
                            ]
                        )
                    },
                    emphasis: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: 'blue' },
                                { offset: 0.7, color: '#2378f7' },
                                { offset: 1, color: '#83bff6' }
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
    myChart.setOption(option);

    option2 = {
        xAxis: [
              {
                  type: 'category',
                  data: dataArray[0],
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
                          opacity: 0.3,
                      },
                  },
              }
        ],
        yAxis: [
            {
                type: 'value',
                min: 0.00,
                //max: 100.00,
                interval: 10.00,
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
                        opacity: 0.3,
                    },
                },

            },
        ],
        grid: {
            top: '12%',
            left: '2%',
            right: '2%',
            bottom: '4%',
            containLabel: true
        },
        series: [
            {
                name: '线平衡率',
                type: 'bar',
                color: ['#999'],
                barWidth: '40%',
                data: dataArray[2],
                itemStyle: {
                    normal: {
                        barBorderRadius: 3,
                        shadowBlur: 3,
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowOffsetX: 4,
                        shadowOffsetY: 0,
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#15FD63' },
                                { offset: 0.5, color: '#09D147' },
                                { offset: 1, color: '#FFCC00' }
                            ]
                        )
                    },
                    emphasis: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: 'blue' },
                                { offset: 0.7, color: '#2378f7' },
                                { offset: 1, color: '#83bff6' }
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
    myChart2 = echarts.init(document.getElementById('main2'));
    myChart2.setOption(option2);

    option3 = {

        xAxis: [
          {
              type: 'category',
              data: dataArray[0],
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
                      opacity: 0.3,
                  },
              },
          }
        ],
        yAxis: [
            {
                type: 'value',
                min: 0.00,
                //max: 100.00,
                interval: 10.00,
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
                        opacity: 0.3,
                    },
                },

            },


        ],
        grid: {
            top: '12%',
            left: '2%',
            right: '2%',
            bottom: '4%',
            containLabel: true
        },
        series: [
            {
                name: '达成率',
                type: 'bar',
                color: ['#999'],
                barWidth: '40%',
                data: dataArray[3],
                itemStyle: {
                    normal: {
                        barBorderRadius: 3,
                        shadowBlur: 3,
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowOffsetX: 4,
                        shadowOffsetY: 0,
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#15FD63' },
                                { offset: 0.5, color: '#09D147' },
                                { offset: 1, color: '#FFCC00' }
                            ]
                        )
                    },
                    emphasis: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: 'blue' },
                                { offset: 0.7, color: '#2378f7' },
                                { offset: 1, color: '#83bff6' }
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
    myChart3 = echarts.init(document.getElementById('main3'));
    myChart3.setOption(option3);

    option4 = {
        xAxis: [
             {
                 type: 'category',
                 data: dataArray[0],
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
                         opacity: 0.3,
                     },
                 },
             }
        ],
        yAxis: [
            {
                type: 'value',
                min: 0.00,
                //max: 100.00,
                //interval: 10.00,
                axisLabel: {
                    formatter: '{value}'
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
                        opacity: 0.3,
                    },
                },

            },


        ],
        grid: {
            top: '12%',
            left: '2%',
            right: '2%',
            bottom: '4%',
            containLabel: true
        },
        series: [
            {
                name: '损失工时',
                type: 'bar',
                color: ['#999'],
                barWidth: '40%',
                data: dataArray[4],
                itemStyle: {
                    normal: {
                        barBorderRadius: 3,
                        shadowBlur: 3,
                        shadowColor: 'rgba(0, 0, 0, 0.2)',
                        shadowOffsetX: 4,
                        shadowOffsetY: 0,
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: '#15FD63' },
                                { offset: 0.5, color: '#09D147' },
                                { offset: 1, color: '#FFCC00' }
                            ]
                        )
                    },
                    emphasis: {
                        color: new echarts.graphic.LinearGradient(
                            0, 0, 0, 1,
                            [
                                { offset: 0, color: 'blue' },
                                { offset: 0.7, color: '#2378f7' },
                                { offset: 1, color: '#83bff6' }
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

}