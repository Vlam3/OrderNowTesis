// Set new default font family and font color to mimic Bootstrap's default styling
Chart.defaults.global.defaultFontFamily = 'Nunito', '-apple-system,system-ui,BlinkMacSystemFont,"Segoe UI",Roboto,"Helvetica Neue",Arial,sans-serif';
Chart.defaults.global.defaultFontColor = '#858796';

function number_format(number, decimals, dec_point, thousands_sep) {
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    var n = !isFinite(+number) ? 0 : +number,
        prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
        sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
        dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
        s = '',
        toFixedFix = function (n, prec) {
            var k = Math.pow(10, prec);
            return '' + Math.round(n * k) / k;
        };
    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');
    if (s[0].length > 3) {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }
    if ((s[1] || '').length < prec) {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    return s.join(dec);
}

function CountDays(month, year) {
    return new Date(year, month, 0).getDate();
}

function getDaysOnArray() {
    var d = new Date();
    var daysCount = CountDays(d.getMonth() + 1, d.getFullYear());
    var days = "";
    return Array.from({
        length: daysCount
    }, (v, i) => i + 1);
}

function getTotalsFromArray(totales) {
    var i;
    var string="";
    for (i = 0; i < totales.length; i++) {
        string += totales[i];
    }
}

function fillData(totalDia, porcentajePres, extrasNombre, extrasCantidad, InsumosNombre, InsumosCantidad) {
    //--------------------------------------------------- 
    //                 Area Chart Fill
    //---------------------------------------------------

    var ctx = document.getElementById("chartVentas");
    var myLineChart = new Chart(ctx, {
        type: 'line',
        data: {
            labels: getDaysOnArray(),
            datasets: [{
                label: "Ventas",
                lineTension: 0.3,
                backgroundColor: "rgba(78, 115, 223, 0.05)",
                borderColor: "rgba(78, 115, 223, 1)",
                pointRadius: 3,
                pointBackgroundColor: "rgba(78, 115, 223, 1)",
                pointBorderColor: "rgba(78, 115, 223, 1)",
                pointHoverRadius: 6,
                pointHoverBackgroundColor: "rgba(78, 115, 223, 1)",
                pointHoverBorderColor: "rgba(78, 115, 223, 1)",
                pointHitRadius: 10,
                pointBorderWidth: 2,
                data: [totalDia[0], totalDia[1], totalDia[2], totalDia[3], totalDia[4], totalDia[5], totalDia[6], totalDia[7], totalDia[8], totalDia[9], totalDia[10],
                    totalDia[10], totalDia[11], totalDia[12], totalDia[13], totalDia[14], totalDia[15], totalDia[16], totalDia[17], totalDia[18], totalDia[19], totalDia[20],
                totalDia[20], totalDia[21], totalDia[22], totalDia[23], totalDia[24], totalDia[25], totalDia[26], totalDia[27], totalDia[28], totalDia[29], totalDia[30], totalDia[31]],
            }],
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'date'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 7
                    }
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                            return '$' + number_format(value);
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                intersect: false,
                mode: 'index',
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {
                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + ': $' + number_format(tooltipItem.yLabel);
                    }
                }
            }
        }
    });

    //--------------------------------------------------- 
    //                 Pie Chart Fill
    //---------------------------------------------------

    ctx = document.getElementById("chartPreferenciasDeVentas");
    var myPieChart = new Chart(ctx, {
        type: 'doughnut',
        data: {
            labels: ["Web", "Presencial"],
            datasets: [{
                data: [100 - porcentajePres, porcentajePres],
                backgroundColor: ['#f6c23e', '#4e73df'],
                hoverBackgroundColor: ['#e0b138', '#4767c5'],
                hoverBorderColor: "rgba(234, 236, 244, 1)",
            }],
        },
        options: {
            maintainAspectRatio: false,
            tooltips: {
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
            },
            legend: {
                display: false
            },
            cutoutPercentage: 80,
        },
    });

    //--------------------------------------------------- 
    //                 Bar Extras Chart Fill
    //---------------------------------------------------

    ctx = document.getElementById("barChartExtras");
    var myBarChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [extrasNombre[0], extrasNombre[1], extrasNombre[2], extrasNombre[3], extrasNombre[4]],
            datasets: [{
                label: "Cantidad",
                backgroundColor: "#4e73df",
                hoverBackgroundColor: "#2e59d9",
                borderColor: "#4e73df",
                data: [extrasCantidad[0], extrasCantidad[1], extrasCantidad[2], extrasCantidad[3], extrasCantidad[4]],
            }],
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    },
                    maxBarThickness: 25,
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                            return '$' + number_format(value);
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {
                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + ': $' + number_format(tooltipItem.yLabel);
                    }
                }
            },
        }
    });

    //--------------------------------------------------- 
    //                 Bar Insumos Chart Fill
    //---------------------------------------------------

    ctx = document.getElementById("barChartInsumos");
    var myBarChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: [InsumosNombre[0], InsumosNombre[1], InsumosNombre[2], InsumosNombre[3], InsumosNombre[4]],
            datasets: [{
                label: "Cantidad",
                backgroundColor: "#4e73df",
                hoverBackgroundColor: "#2e59d9",
                borderColor: "#4e73df",
                data: [InsumosCantidad[0], InsumosCantidad[1], InsumosCantidad[2], InsumosCantidad[3], InsumosCantidad[4]],
            }],
        },
        options: {
            maintainAspectRatio: false,
            layout: {
                padding: {
                    left: 10,
                    right: 25,
                    top: 25,
                    bottom: 0
                }
            },
            scales: {
                xAxes: [{
                    time: {
                        unit: 'month'
                    },
                    gridLines: {
                        display: false,
                        drawBorder: false
                    },
                    ticks: {
                        maxTicksLimit: 6
                    },
                    maxBarThickness: 25,
                }],
                yAxes: [{
                    ticks: {
                        min: 0,
                        maxTicksLimit: 5,
                        padding: 10,
                        // Include a dollar sign in the ticks
                        callback: function (value, index, values) {
                            return '$' + number_format(value);
                        }
                    },
                    gridLines: {
                        color: "rgb(234, 236, 244)",
                        zeroLineColor: "rgb(234, 236, 244)",
                        drawBorder: false,
                        borderDash: [2],
                        zeroLineBorderDash: [2]
                    }
                }],
            },
            legend: {
                display: false
            },
            tooltips: {
                titleMarginBottom: 10,
                titleFontColor: '#6e707e',
                titleFontSize: 14,
                backgroundColor: "rgb(255,255,255)",
                bodyFontColor: "#858796",
                borderColor: '#dddfeb',
                borderWidth: 1,
                xPadding: 15,
                yPadding: 15,
                displayColors: false,
                caretPadding: 10,
                callbacks: {
                    label: function (tooltipItem, chart) {
                        var datasetLabel = chart.datasets[tooltipItem.datasetIndex].label || '';
                        return datasetLabel + ': $' + number_format(tooltipItem.yLabel);
                    }
                }
            },
        }
    });

    //--------------------------------------------------- 
    //                 Card Values Fill
    //---------------------------------------------------

    ctx = document.getElementById("Label1");
    console.log("asdasdaaa");
    console.log(ctx);
    ctx.value = 'asdasd';
    //ctx.value = '$' + number_format(ctx.value);
}