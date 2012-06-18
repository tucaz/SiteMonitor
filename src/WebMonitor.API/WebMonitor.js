google.load('visualization', '1.0', { 'packages': ['corechart'] });
google.setOnLoadCallback(Initialize);

function Initialize() {
    LoadDataForRunner('GoogleSearchRunner');
}

function LoadDataForRunner(runnerName) {
    $.get('/api/RunResults?runnerName=' + runnerName, function (result) {
        RunnerChart.initialize(document.getElementById('chart'));
        RunnerChart.render(result);
    });
}

var RunnerChart = {
    initialize: function (obj) {
        this.options = {
            title: '',
            colors: ['#CCCCCC', '#9EDAE5'],
            areaOpacity: .6,
            chartArea: { 'top': 10, 'width': '80%' },
            width: "100%",
            height: 300,
            animation: { duration: 500, easing: 'out' },
            vAxis: { minValue: 0 }
        }

        this.chart = new google.visualization.AreaChart(obj)
        this.dataTable = new google.visualization.DataTable()
        this.dataTable.addColumn('string', 'string')
        this.dataTable.addColumn('number', 'Time Taken (seconds)')
    },

    render: function (data) {
        var that = this;

        $.each(data, function (index, obj) {
            if (that.dataTable)
                that.dataTable.addRow([obj[0], obj[1]])
        });

        this.chart.draw(this.dataTable, this.options)
    }
}