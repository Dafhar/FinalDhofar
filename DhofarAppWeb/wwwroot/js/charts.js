const ctx = document.getElementById('myChart').getContext('2d');

function westernToEasternArabicNumerals(number) {
    const easternNumerals = {
        '0': '٠',
        '1': '١',
        '2': '٢',
        '3': '٣',
        '4': '٤',
        '5': '٥',
        '6': '٦',
        '7': '٧',
        '8': '٨',
        '9': '٩'
    };

    return String(number).replace(/[0-9]/g, (match) => easternNumerals[match]);
}

const labels = Array.from({ length: 20 }, (_, i) => (i * 0.5 + 0.5)).reverse();

new Chart(ctx, {
    type: 'bar',
    data: {
        labels: labels.map(label => westernToEasternArabicNumerals(label)),
        datasets: [{
            label: '# of Votes',
            data: [5, 10, 15, 10, 5, 10, 15, 10, 5, 10, 5, 10, 15, 10, 5, 10, 5, 10, 15, 10].reverse(),
            backgroundColor: '#135790',
            borderWidth: 1,
            barThickness: 10
        }]
    },
    options: {
        scales: {
            x: {
                display: true,
                ticks: {
                    color: '#6C757D',
                    font: {
                        family: "Effra",
                        weight: 'bold',
                        size: 12
                    },
                    maxRotation: 0,
                    minRotation: 0
                },
                grid: {
                    display: false
                }
            },
            y: {
                position: 'right',
                display: true,
                ticks: {
                    color: '#6C757D',
                    font: {
                        family: "Effra",
                        weight: 'bold',
                        size: 12
                    },
                    stepSize: 5,
                    beginAtZero: true,
                    max: 20,
                    callback: function (value) {
                        return westernToEasternArabicNumerals(value);
                    }
                },
                grid: {
                    display: false
                }
            }
        },
        plugins: {
            legend: {
                display: false
            }
        },
        elements: {
            bar: {
                borderColor: '#C1C8E5',
                borderRadius: 50,
            }
        },
        layout: {
            padding: {
                left: 10,
                right: 10,
                top: 10,
                bottom: 10
            }
        }
    }
});


const ctz = document.getElementById('myLineChart').getContext('2d');

const data = {
    labels: ['١٢ص', '٦ م', '١١ م'],
    datasets: [
        {
            label: 'الأعضاء',
            data: [10, 15, 12, 8, 20, 15, 12, 8, 20],
            borderColor: '#6EC1ED',
            borderWidth: 3,
            fill: false,
            pointStyle: 'circle', // Set the point style to circle
            pointRadius: 3, // Adjust the radius of the circle
            pointBackgroundColor: 'white', // Set the fill color of the circle
        },
        {
            label: 'الزوار',
            data: [8, 10, 11, 6, 15, 15, 12, 8, 20],
            borderColor: '#5DD1B1',
            borderWidth: 3,
            fill: false,
            pointStyle: 'circle', // Set the point style to circle
            pointRadius: 3, // Adjust the radius of the circle
            pointBackgroundColor: 'white', // Set the fill color of the circle
        }
    ]
};

const myChart = new Chart(ctz, {
    type: 'line',
    data: data,
    options: {
        scales: {
            x: {
                ticks: {
                    color: '#000',
                    font: {
                        family: "Effra",
                        weight: 'bold',
                        size: 12
                    },
                    maxRotation: 0,
                    minRotation: 0
                },
                grid: {
                    display: false
                }
            },
            y: {
                axis: 'y',
                display: false,
                position: 'right', // Position the y-axis on the right side
                grid: {
                    display: false // To hide the y-axis gridlines
                }

            }
        },
        plugins: {
            legend: {
                align: 'start',
                display: true,
                position: 'top',
                labels: {
                    color: '#000',
                    font: {
                        family: 'Effra',
                        size: 12,
                        weight: 'bold'
                    },
                    usePointStyle: true, // Use point style (circles)
                    generateLabels: function (chart) {
                        const original = Chart.defaults.plugins.legend.labels.generateLabels(chart);
                        original.forEach(label => {
                            label.pointStyle = 'circle'; // Set point style to circle
                            label.fillStyle = label.strokeStyle; // Fill color same as stroke color
                            label.pointRadius = 1; // Change the size of the circles in the legend
                        });
                        return original;
                    }
                }
            }
        },
        elements: {
            point: {
                radius: 4 // To hide the data points if needed
            }
        },
        layout: {
            padding: {
                left: 10,
                right: 10,
                top: 10,
                bottom: 10
            }
        }
    }
});

const cty = document.getElementById('myChart2').getContext('2d');

const labels2 = Array.from({ length: 24 }, (_, i) => i.toString()); // Creating labels for 24 hours (0 to 23)

new Chart(cty, {
    type: 'bar',
    data: {
        labels: labels2.map(label => westernToEasternArabicNumerals(label)),
        datasets: [{
            label: '# of Votes',
            data: [10, 5, 10, 15, 10, 5, 10, 15, 10, 5, 10, 5, 10, 15, 10, 5, 10, 5, 10, 15, 10, 5, 10, 15], // Adding 24 data points for the 24 labels
            backgroundColor: '#135790',
            borderWidth: 1,
            barThickness: 15 // Adjust the bar width here
        }]
    },
    options: {
        scales: {
            x: {

                display: true,
                ticks: {
                    color: '#6C757D',
                    font: {
                        family: "Effra",
                        weight: 'bold',
                        size: 12
                    },
                    maxRotation: 0,
                    minRotation: 0
                },
                grid: {
                    display: false
                }
            },
            y: {
                position: 'right',
                display: true,
                ticks: {
                    color: '#6C757D',
                    font: {
                        family: "Effra",
                        weight: 'bold',
                        size: 12
                    },
                    stepSize: 5,
                    beginAtZero: true,
                    max: 20,
                    callback: function (value) {
                        return westernToEasternArabicNumerals(value);
                    }
                },
                grid: {
                    display: false
                }
            }
        },
        plugins: {
            legend: {
                display: false
            }
        },
        elements: {
            bar: {
                borderColor: '#C1C8E5',
                borderRadius: 50,
            }
        },
        layout: {
            padding: {
                left: 10,
                right: 10,
                top: 10,
                bottom: 10
            }
        }
    }
});

